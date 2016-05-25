using CTLib;
using Google.Api.Maps.Service;
using IngressoRapido.Lib.CSS;
using IRLib.Cinema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;


namespace IngressoRapido.Lib
{
    public class Filme
    {
        private string URLImagem
        {
            get
            {
                return ConfigurationManager.AppSettings["DiretorioImagensFilmes"] == null ? string.Empty : ConfigurationManager.AppSettings["DiretorioImagensFilmes"].ToString();
            }
        }

        private DAL oDal { get; set; }
        public int ID { get; set; }
        public int LocalID { get; set; }
        public string Nome { get; set; }
        public int Duracao { get; set; }
        public int Idade { get; set; }
        public string IdadeJustificativa { get; set; }
        public bool Dublado { get; set; }
        public string IMDB { get; set; }
        public string Sinopse { get; set; }
        public int EventoID { get; set; }
        public string Genero { get; set; }
        public string Imagem
        {
            get
            {
                return URLImagem + ID.ToString("ifil000000.jpg");
            }
        }

        public Filme GetByID(int id, int localID)
        {
            try
            {
                oDal = new DAL();
                using (IDataReader dr = oDal.SelectToIDataReader(
                        @"SELECT TOP 1 
                                f.FilmeID AS ID, f.Nome, f.Duracao, f.Idade, f.IdadeJustificativa, f.IMDB, f.Sinopse, f.Dublado, e.IR_EventoID AS EventoID
                            FROM Filme f (NOLOCK)
                            INNER JOIN Evento e (NOLOCK) ON e.FilmeID = f.FilmeID
                            WHERE f.FilmeID = " + id + (localID > 0 ? " AND e.LocalID = " + localID : string.Empty)))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível localizar o filme informado.");

                    this.ID = id;
                    this.EventoID = dr["EventoID"].ToInt32();
                    this.Nome = dr["Nome"].ToString();
                    this.LocalID = localID;
                    this.Duracao = dr["Duracao"].ToInt32();
                    this.Idade = dr["Idade"].ToInt32();
                    this.IdadeJustificativa = dr["IdadeJustificativa"].ToString();
                    this.Dublado = dr["Dublado"].ToBoolean();
                    this.IMDB = dr["IMDB"].ToString();
                    this.Sinopse = dr["Sinopse"].ToString();
                }

                return this;
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public Filme GetByID(int id)
        {
            return this.GetByID(id, 0);
        }

        public List<Calendario> MontarCalendario(int filmeID, int localID, int ano, int mes, DateTime menorHorario)
        {
            try
            {
                List<Calendario> dias = new List<Calendario>();
                int primeiroDiaSemana = 0; //domingo
                int ultimoDiaSemana = 6; //sabado
                int linha = 1;

                List<int> diasSemana = new List<int>();

                DateTime dataInicial = new DateTime(DateTime.Now.Year, mes, 1);
                DateTime dataAtual = dataInicial;

                //Se não comecar no Domingo, precisa adicionar os itens anteriores no dia da semana.
                int dayOfWeek = Convert.ToInt32(dataInicial.DayOfWeek);
                if (primeiroDiaSemana < dayOfWeek)
                    for (int i = 0; i < dayOfWeek; i++)
                        dias.Add(new Calendario() { Line = 1 });

                for (int i = 0; i <= 31; i++)
                {
                    dataAtual = dataInicial.AddDays(i);

                    if (dataAtual.Month != dataInicial.Month)
                        break;

                    linha = dias.Count >= 7 && dias.Count % 7 == 0 ? linha + 1 : linha;
                    dias.Add(new Calendario()
                    {
                        Value = dataAtual.Day,
                        Line = linha,
                    });
                }

                DateTime dataFinal = new DateTime(ano, mes, dias.LastOrDefault().Value);

                //Se não terminar no Sabado, precisa adicionar os itens posteriores no dia da semana.
                dayOfWeek = Convert.ToInt32(dataFinal.DayOfWeek);
                if (dayOfWeek < ultimoDiaSemana)
                    for (int i = dayOfWeek; i < ultimoDiaSemana; i++)
                        dias.Add(new Calendario() { Line = linha });

                var calendarioDisponivel = this.MontarCalendarioApresentacoes(filmeID, localID, ano, mes, menorHorario);

                if (calendarioDisponivel == null || calendarioDisponivel.Count == 0)
                    return dias;

                //Se os dias estão disponiveis no calendario de ingressos, disponibilida no linq abaixo
                (from d in dias
                 join c in calendarioDisponivel on d.Value equals c.Day
                 where c.Available
                 select d).ToList().ForEach(
                    delegate(Calendario calendario)
                    {
                        calendario.IsValid = true;
                        calendario.HasValue = true;
                    });

                return dias;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CalendarioIngressos> MontarCalendarioApresentacoes(int filmeID, int localID, int ano, int mes, DateTime menorHorario)
        {
            oDal = new DAL();
            try
            {
                List<CalendarioIngressos> lista = new List<CalendarioIngressos>();

                DateTime inicio = new DateTime(ano, mes, 1);
                menorHorario = (menorHorario - inicio).Milliseconds <= 0 ? inicio : menorHorario;

                //Tentando ver retroativo, retorna sem nada...
                if (menorHorario.Year <= DateTime.Now.Year && menorHorario.Month < DateTime.Now.Month)
                    return null;

                using (IDataReader dr = oDal.SelectToIDataReader(
                    string.Format(@"
						SELECT
	                            ap.Horario
	                        FROM Evento e
	                        INNER JOIN Filme f ON f.FilmeID = e.FilmeID
	                        INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID
	                        INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = ap.IR_ApresentacaoID
	                        INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = e.LocalID
                            WHERE f.FilmeID = {0} {1} AND ap.Horario BETWEEN '{2}' AND '{3}'
					   ", filmeID, localID > 0 ? "AND l.IR_LocalID = " + localID : string.Empty, menorHorario.ToString("yyyyMMddHHmmss"), inicio.ToString("yyyyMM") + "31999999")))
                {
                    while (dr.Read())
                    {
                        lista.Add(new CalendarioIngressos()
                        {
                            Day = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao).Day,
                            Available = true,
                        });
                    }
                }

                return lista.Distinct().ToList();
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public List<Filmes.Programacao> CarregarProgramacoes(int filmeID, int localID, int mes, int dia, string estado, string cidade, string cep, string local)
        {
            oDal = new DAL();
            try
            {
                DateTime data = new DateTime(DateTime.Now.Year, mes, dia);

                var lista = new List<Filmes.Programacao>();

                bool calcularDistancia = !string.IsNullOrEmpty(cep.ToCEP());
                GeographicPosition coordenadas = null;
                if (calcularDistancia)
                    coordenadas = IRLib.CEP.BuscarCoordenadas(cep.ToCEP());

                string filtroLocal = string.Empty;
                bool somenteLocalID = !calcularDistancia && string.IsNullOrEmpty(estado) && !string.IsNullOrEmpty(cidade);

                if (!somenteLocalID && !string.IsNullOrEmpty(local))
                    filtroLocal = "AND l.Nome LIKE '" + local.ToSafeStringWithQuote() + "'";
                else if (somenteLocalID)
                    filtroLocal = "AND l.IR_LocalID = " + localID;

                string filtroEstadoCidade = string.Empty;
                if (!calcularDistancia)
                    if (!string.IsNullOrEmpty(estado) && !string.IsNullOrEmpty(cidade))
                        filtroEstadoCidade = string.Format("AND l.Estado = '{0}' AND l.Cidade = '{1}'", estado, cidade);


                string sql = string.Format(
                    @"
                        SELECT
	                           DISTINCT l.IR_LocalID AS LocalID, l.Nome AS Local, s.IR_SetorID AS SetorID, s.Nome AS Setor, IsNull(s.NVendeLugar, 0) AS NVendeLugar, ap.CodigoProgramacao, ap.Horario, l.Latitude, l.Longitude
	                        FROM Evento e
	                        INNER JOIN Filme f ON f.FilmeID = e.FilmeID
	                        INNER JOIN Apresentacao ap (NOLOCK) ON ap.EventoID = e.IR_EventoID
	                        INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = ap.IR_ApresentacaoID
	                        INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = e.LocalID
	                        WHERE f.FilmeID = {0} {1} AND ap.Horario BETWEEN '{2}' AND '{3}'
	                        ORDER BY l.Nome, s.Nome, ap.Horario
                    ", filmeID, filtroLocal, data.Date.ToString("yyyyMMddHHmmss"), data.AddDays(1).Date.ToString("yyyyMMddHHmmss"));

                using (IDataReader dr = oDal.SelectToIDataReader(sql))
                {
                    var horarios = new List<IngressoRapido.Lib.Filmes.Horarios>();
                    while (dr.Read())
                    {
                        if (lista.Where(c => c.LocalID == dr["LocalID"].ToInt32()).Count() == 0)
                        {
                            horarios = new List<Filmes.Horarios>();
                            int distancia = 0;

                            if (calcularDistancia)
                                distancia = IRLib.CEP.CalcularDistancia(Convert.ToDouble(coordenadas.Latitude), Convert.ToDouble(coordenadas.Longitude), Convert.ToDouble(dr["Latitude"]), Convert.ToDouble(dr["Longitude"]));

                            lista.Add(new Filmes.Programacao()
                            {
                                LocalID = dr["LocalID"].ToInt32(),
                                Local = dr["Local"].ToString(),
                                SetorID = dr["SetorID"].ToInt32(),
                                Setor = dr["Setor"].ToString(),
                                Horarios = horarios,
                                Distancia = distancia,
                            });
                        }

                        DateTime horario = dr["Horario"].ToDateTimeFromDB();

                        horarios.Add(new Filmes.Horarios()
                        {
                            Disponivel = !dr["NVendeLugar"].ToBoolean() && horario > DateTime.Now.AddHours(1),
                            Horario = horario.ToString("HH:mm"),
                            ProgramacaoID = dr["CodigoProgramacao"].ToString()
                        });
                    }

                };

                if (calcularDistancia)
                    return lista.Where(c => c.Distancia <= 30).OrderBy(c => c.Distancia).ToList();

                return lista;
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public string EscolherIngresso(int filmeID, int localID, string codigoProgramacao, int setorID, int mes, int dia)
        {
            return JsonConvert.SerializeObject(new { Ret = 1, Precos = this.Precos(codigoProgramacao, setorID) });
        }

        public List<IngressoRapido.Lib.Filmes.Preco> Precos(string codigoProgramacao, int setorID)
        {
            VagasProg vagas = Service.getVagasProgramacao(SincronizarCinemas.MontarAuth(), new Filtros.GetVagasProg() { IDProg = codigoProgramacao });
            if (vagas.Disponivel == 0)
            {
                //TODO: Setar como ESGOTADO!
                throw new Exception("A programação selecionada não possui mais assentos disponíveis, por favor, selecione outro horário.");
            }

            oDal = new DAL();
            List<IngressoRapido.Lib.Filmes.Preco> lista = new List<Filmes.Preco>();
            try
            {
                string sql = string.Format(@"
                    SELECT 
                        ap.IR_ApresentacaoID AS ApresentacaoID, SetorID, IR_PrecoID AS ID, Nome, Valor, CodigoCinema
                    FROM Preco p
                    INNER JOIN Apresentacao ap ON ap.IR_ApresentacaoID = p.ApresentacaoID
                    WHERE ap.CodigoProgramacao = '{0}'
                    ORDER BY p.Nome
                    ", codigoProgramacao);


                using (IDataReader dr = oDal.SelectToIDataReader(sql))
                {
                    while (dr.Read())
                        lista.Add(new Filmes.Preco()
                        {
                            ID = dr["ID"].ToInt32(),
                            Nome = dr["Nome"].ToString(),
                            Valor = dr["Valor"].ToDecimal(),
                            Codigo = dr["CodigoCinema"].ToString(),
                            ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                            SetorID = dr["SetorID"].ToInt32()
                        });
                }

                return lista;
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public void Reservar()
        {

        }

    }
}
