using CTLib;
using IngressoRapido.Lib.CSS;
using IRLib;
using IRLib.ClientObjects.Serie;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace IngressoRapido.Lib
{
    public class Serie
    {
        private DAL oDal { get; set; }

        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Regras { get; set; }
        public bool TemRegras { get; set; }
        public int QuantidadeMinimaGrupo { get; set; }
        public int QuantidadeMaximaGrupo { get; set; }
        public int QuantidadeMinimaApresentacao { get; set; }
        public int QuantidadeMaximaApresentacao { get; set; }
        public int QuantidadeMinimaIngressosPorApresentacao { get; set; }
        public int QuantidadeMaximaIngressosPorApresentacao { get; set; }
        public int Pagina { get; set; }

        public static string ToImage(int ID)
        {
            return "pa" + ID.ToString("000000") + ".jpg";
        }

        public SerieItemLista SerieItemLista { get; set; }

        public void GetByID(int SerieID)
        {
            oDal = new DAL();
            try
            {
                using (IDataReader dr = oDal.SelectToIDataReader(
                    @"SELECT Titulo, Nome, QuantidadeMinimaGrupo, QuantidadeMaximaGrupo,
                                QuantidadeMinimaApresentacao, QuantidadeMaximaApresentacao,
                                QuantidadeMinimaIngressosPorApresentacao, QuantidadeMaximaIngressosPorApresentacao,
                                Descricao, Regras
                        FROM Serie WHERE IR_SerieID = " + SerieID))
                {
                    if (!dr.Read())
                        throw new Exception("A série informada não foi encontrada, por favor, tente novamente.");

                    this.ID = SerieID;
                    this.Titulo = dr["Titulo"].ToString();
                    this.Nome = dr["Nome"].ToString();
                    this.QuantidadeMinimaGrupo = dr["QuantidadeMinimaGrupo"].ToInt32();
                    this.QuantidadeMaximaGrupo = dr["QuantidadeMaximaGrupo"].ToInt32();
                    this.QuantidadeMaximaApresentacao = dr["QuantidadeMaximaApresentacao"].ToInt32();
                    this.QuantidadeMinimaApresentacao = dr["QuantidadeMinimaApresentacao"].ToInt32();
                    this.QuantidadeMinimaIngressosPorApresentacao = dr["QuantidadeMinimaIngressosPorApresentacao"].ToInt32();
                    this.QuantidadeMaximaIngressosPorApresentacao = dr["QuantidadeMaximaIngressosPorApresentacao"].ToInt32();
                    this.Descricao = dr["Descricao"].ToString();
                    this.Regras = dr["Regras"].ToString();
                }
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public void CarregarSerie(int SerieID, int ClienteID, string SessionID, bool SoReservados)
        {
            oDal = new DAL();
            SerieItemLista = new SerieItemLista();
            try
            {
                string sql = @"SELECT
                                TOP 1
                                s.IR_SerieID AS SerieID, s.Titulo, s.Nome, 
                                s.QuantidadeMinimaGrupo, s.QuantidadeMaximaGrupo,
                                s.QuantidadeMinimaApresentacao, s.QuantidadeMaximaApresentacao,
                                s.QuantidadeMinimaIngressosPorApresentacao, s.QuantidadeMaximaIngressosPorApresentacao,
                                s.Regras, s.Descricao
                                FROM Serie s (NOLOCK) 
                                WHERE IR_SerieID = " + SerieID;

                using (IDataReader dr = oDal.SelectToIDataReader(sql))
                {
                    if (!dr.Read())
                        throw new Exception("A série informada não foi encontrada.");

                    this.ID = dr["SerieID"].ToInt32();
                    this.Titulo = dr["Titulo"].ToString();
                    this.Nome = dr["Nome"].ToString();
                    this.QuantidadeMinimaGrupo = dr["QuantidadeMinimaGrupo"].ToInt32();
                    this.QuantidadeMaximaGrupo = dr["QuantidadeMaximaGrupo"].ToInt32();
                    this.QuantidadeMaximaApresentacao = dr["QuantidadeMaximaApresentacao"].ToInt32();
                    this.QuantidadeMinimaApresentacao = dr["QuantidadeMinimaApresentacao"].ToInt32();
                    this.QuantidadeMinimaIngressosPorApresentacao = dr["QuantidadeMinimaIngressosPorApresentacao"].ToInt32();
                    this.QuantidadeMaximaIngressosPorApresentacao = dr["QuantidadeMaximaIngressosPorApresentacao"].ToInt32();
                    this.Regras = dr["Regras"].ToString();
                    this.Descricao = dr["Descricao"].ToString();
                    dr.Close();
                }

                sql = string.Format(@"SELECT
	                                e.IR_EventoID AS EventoID, e.Nome AS Evento, e.EscolherLugarMarcado,
                                    ap.IR_ApresentacaoID AS ApresentacaoID, ap.Horario,
                                    st.IR_SetorID AS SetorID, st.Nome AS Setor, st.LugarMarcado, st.AprovadoPublicacao,
                                    p.IR_PrecoID AS PrecoID, p.Nome AS Preco, p.Valor, p.QuantidadePorCliente, st.QtdeDisponivel,
                                    si.Promocional, si.QuantidadePorPromocional,
	                                COUNT(c.ID) AS Quantidade
	                            FROM Serie s (NOLOCK)
	                            INNER JOIN SerieItem si (NOLOCK) ON si.SerieID = s.IR_SerieID
	                            INNER JOIN Preco p (NOLOCK) ON p.IR_PrecoID = si.PrecoID
	                            INNER JOIN Setor st (NOLOCK) ON st.IR_SetorID = p.SetorID AND st.ApresentacaoID = si.ApresentacaoID
	                            INNER JOIN Apresentacao ap (NOLOCK) ON ap.IR_ApresentacaoID = st.ApresentacaoID
	                            INNER JOIN Evento e (NOLOCK) ON e.IR_EventoID = ap.EventoID
	                            {3} JOIN Carrinho c (NOLOCK) ON c.PrecoID = si.PrecoID AND c.ClienteID = {0} AND c.SessionID = '{1}' AND c.Status = '{2}'
	                                WHERE s.IR_SerieID = {4} 
	                            GROUP BY
	                                e.IR_EventoID, e.Nome, e.EscolherLugarMarcado,
                                    ap.IR_ApresentacaoID, ap.Horario,st.Nome,
                                    st.IR_SetorID, st.LugarMarcado, st.AprovadoPublicacao, st.QtdeDisponivel,
                                    p.IR_PrecoID, p.Nome, p.Valor, p.QuantidadePorCliente,
                                    si.Promocional, si.QuantidadePorPromocional
                                ORDER BY e.Nome, ap.Horario, st.Nome, p.Nome", ClienteID, SessionID, IRLib.Ingresso.RESERVADO, (SoReservados ? "INNER" : "LEFT"), SerieID);

                using (IDataReader dr = oDal.SelectToIDataReader(sql))
                {
                    while (dr.Read())
                        this.SerieItemLista.Add(new SerieItem()
                        {
                            EventoID = dr["EventoID"].ToInt32(),
                            Evento = dr["Evento"].ToString(),
                            ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                            Horario = dr["Horario"].ToParseDateTime(),
                            SetorID = dr["SetorID"].ToInt32(),
                            Setor = dr["Setor"].ToString(),
                            PrecoID = dr["PrecoID"].ToInt32(),
                            Preco = dr["Preco"].ToString(),
                            Valor = dr["Valor"].ToDecimal().ToString("c"),
                            QuantidadePorCliente = dr["QuantidadePorCliente"].ToInt32(),
                            Quantidade = dr["Quantidade"].ToInt32(),
                            EscolherLugarMarcado =
                                dr["EscolherLugarMarcado"].ToBoolean() &&
                                dr["LugarMarcado"].ToChar() == Setor.LugarTipo.Cadeira.ToChar() &&
                                dr["AprovadoPublicacao"].ToBoolean(),
                            Disponivel = dr["QtdeDisponivel"].ToInt32() > 0,
                            Promocional = dr["Promocional"].ToBoolean(),
                            QuantidadePorPromocional = dr["QuantidadePorPromocional"].ToInt32(),
                        });

                    dr.Close();
                }

                //if (this.SerieItemLista.Count > 0)
                //    this.SerieItemLista.Insert(0, new SerieItem { Evento = "Todos", Horario = DateTime.MinValue, Setor = "Todos", Preco = "Todos", PrecoID = 0 });
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public string ToJson()
        {
            if (this.ID == 0)
                throw new Exception("A série informada não foi encontrada.");

            StringBuilder stb = new StringBuilder();
            stb.Append("{");
            stb.AppendFormat(@" 'Ret' : 0,
                                'ID' : {0},
                                'Titulo' : '{1}', 
                                'Nome' : '{2}',
                                'QtdGrupoMin' : {3},
                                'QtdGrupoMax' : {4},
                                'QtdMin' : {5},
                                'QtdMax' : {6},
                                'QtdIngressoMin' : {7},
                                'QtdGrupoMax' : {8},
                                'Regras' : '{9}',
                                'Descricao' : '{10}',
                                'Items' : {11}                       
                                ", this.ID, this.Titulo, this.Nome,
                                 this.QuantidadeMinimaGrupo, this.QuantidadeMaximaGrupo,
                                 this.QuantidadeMinimaApresentacao, this.QuantidadeMaximaApresentacao,
                                 this.QuantidadeMinimaIngressosPorApresentacao, this.QuantidadeMaximaIngressosPorApresentacao,
                                 this.Regras.Replace(Environment.NewLine, "<br/>"), this.Descricao.Replace(Environment.NewLine, "<br/>"),
                                 this.SerieItemLista == null ? string.Empty : this.SerieItemLista.ToJson());
            stb.Append("}");


            return stb.ToString();
        }

        public EstruturaValidacaoSerieSite MontarGrupo(int SerieID)
        {
            return new EstruturaValidacaoSerieSite().Converter(this.MontarGrupo(SerieID, true));
        }

        public EstruturaValidacaoSeries MontarGrupo(int SerieID, bool site)
        {
            oDal = new DAL();
            try
            {
                this.CarregarSerie(SerieID,
                    HttpContext.Current.Session["ClienteID"].ToInt32(),
                    HttpContext.Current.Session.SessionID,
                    true);

                if (this.SerieItemLista.Count == 0)
                    return null;

                EstruturaValidacaoSeries series = new EstruturaValidacaoSeries();

                foreach (var item in this.SerieItemLista.OrderBy(c => c.EventoID).ThenBy(c => c.ApresentacaoID))
                {
                    for (int i = 0; i < item.Quantidade; i++)
                    {
                        series.AdicionarItem(
                                this.Nome,
                                SerieID,
                                item.EventoID,
                                item.Evento,
                                item.ApresentacaoID,
                                item.HorarioStr,
                                this.QuantidadeMinimaGrupo,
                                this.QuantidadeMaximaGrupo,
                                this.QuantidadeMinimaApresentacao,
                                this.QuantidadeMaximaApresentacao,
                                this.QuantidadeMinimaIngressosPorApresentacao,
                                this.QuantidadeMaximaIngressosPorApresentacao,
                                item.Promocional,
                                item.QuantidadePorPromocional);
                    }
                }
                //Aplica o ULTIMO item, a iteração do Foreach ignora o ultimo associado.
                series.AplicarItem();
                series.MontarGrupos();

                return series;
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public string Verificar()
        {
            this.MontarGrupo(this.ID);

            string mensagemRetorno = string.Empty;


            return mensagemRetorno;
        }

        public int MontarGrupo(IEnumerable<CarrinhoLista.CarrinhoContador> carrinhoContador)
        {
            this.GetByID(carrinhoContador.FirstOrDefault().SerieID);
            EstruturaValidacaoSeries series = new EstruturaValidacaoSeries();

            foreach (var carrinho in carrinhoContador)
            {
                series.AdicionarItem(
                               this.Nome,
                               this.ID,
                               0,
                               string.Empty,
                               carrinho.ApresentacaoID,
                               string.Empty,
                               this.QuantidadeMinimaGrupo,
                               this.QuantidadeMaximaGrupo,
                               this.QuantidadeMinimaApresentacao,
                               this.QuantidadeMaximaApresentacao,
                               this.QuantidadeMinimaIngressosPorApresentacao,
                               this.QuantidadeMaximaIngressosPorApresentacao,
                               carrinho.ItemPromocional,
                               carrinho.QuantidadePorPromocional);
            }

            //Aplica o ULTIMO item, a iteração do Foreach ignora o ultimo associado.
            series.AplicarItem();
            series.MontarGrupos();

            return series.Series.FirstOrDefault().Grupos.Count;
        }

        public EstruturaValidacaoSeries MontarGrupo(IEnumerable<Carrinho> carrinhoLista)
        {
            this.GetByID(carrinhoLista.FirstOrDefault().SerieID);
            EstruturaValidacaoSeries series = new EstruturaValidacaoSeries();

            foreach (var carrinho in carrinhoLista)
            {
                series.AdicionarItem(
                               this.Nome,
                               this.ID,
                               0,
                               string.Empty,
                               carrinho.ApresentacaoID,
                               string.Empty,
                               this.QuantidadeMinimaGrupo,
                               this.QuantidadeMaximaGrupo,
                               this.QuantidadeMinimaApresentacao,
                               this.QuantidadeMaximaApresentacao,
                               this.QuantidadeMinimaIngressosPorApresentacao,
                               this.QuantidadeMaximaIngressosPorApresentacao,
                               carrinho.ItemPromocional,
                               carrinho.QuantidadePorPromocional);
            }

            //Aplica o ULTIMO item, a iteração do Foreach ignora o ultimo associado.
            series.AplicarItem();
            series.MontarGrupos();

            return series;
        }


        public int MontarQuantidadeGrupo(IEnumerable<Carrinho> carrinhoLista)
        {
            return this.MontarGrupo(carrinhoLista).Series.FirstOrDefault().Grupos.Count;
        }

        /*
         * 
         *  ABAIXO ESTÃO OS MÉTODOS DA VERSÃO CRIE SUA SÉRIE!!!!! 
         * 
         * 
         */


        public List<Calendario> MontarCalendario(int serieID, int ano, int mes, DateTime menorHorario)
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

                    linha = dias.Count % 7 == 0 ? linha + 1 : linha;
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
                        dias.Add(new Calendario() { Line = 1 });




                var calendarioDisponivel = this.MontarCalendarioIngressos(serieID, ano, mes, menorHorario);

                if (calendarioDisponivel == null || calendarioDisponivel.Count == 0)
                    return dias;

                //Se os dias estão disponiveis no calendario de ingressos, disponibilida no linq abaixo
                (from d in dias
                 join c in calendarioDisponivel on d.Value equals c.Day
                 where c.Available
                 select d
                 ).ToList().ForEach(
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

        public List<CalendarioIngressos> MontarCalendarioIngressos(int serieID, int ano, int mes, DateTime menorHorario)
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
	                            DISTINCT ap.Horario
	                        FROM Serie s (NOLOCK)
	                        INNER JOIN SerieItem si (NOLOCK) ON si.SerieID = s.IR_SerieID
	                        INNER JOIN Apresentacao ap (NOLOCK) ON si.ApresentacaoID = ap.IR_ApresentacaoID
	                        WHERE s.IR_SerieID = {0} AND ap.Horario BETWEEN '{1}' AND '{2}'
					   ", serieID, menorHorario.Date.ToString("yyyyMMddHHmmss"), inicio.ToString("yyyyMM") + "31999999")))
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

                return lista;
            }
            finally
            {
                oDal.ConnClose();
            }
        }



        public List<SerieItemCSS> CarregarApresentacoes(int serieID, int mes, int dia)
        {
            oDal = new DAL();
            try
            {
                DateTime data = new DateTime(DateTime.Now.Year, mes, dia);


                string sql = string.Format(@"
                        
						SELECT
	                            DISTINCT e.Nome AS Evento, ap.IR_ApresentacaoID AS ApresentacaoID, ap.Horario, ap.Programacao, st.Nome AS Setor		
	                        FROM Serie s (NOLOCK)
	                        INNER JOIN SerieItem si (NOLOCK) ON si.SerieID = s.IR_SerieID
	                        INNER JOIN Apresentacao ap (NOLOCK) ON si.ApresentacaoID = ap.IR_ApresentacaoID
	                        INNER JOIN Evento e (NOLOCK) ON ap.EventoID = e.IR_EventoID
	                        INNER JOIN Setor st (NOLOCK) ON st.IR_SetorID = si.SetorID
	                        WHERE s.IR_SerieID = {0} AND ap.Horario BETWEEN '{1}' AND '{2}'
	                        ORDER BY e.Nome, ap.Horario, ap.IR_ApresentacaoID, st.Nome
					   
                    ", serieID, data.Date.ToString("yyyyMMddHHmmss"), data.AddDays(1).Date.ToString("yyyyMMddHHmmss"));

                List<SerieItemCSS> lista = new List<SerieItemCSS>();

                using (IDataReader dr = oDal.SelectToIDataReader(sql))
                {
                    List<string> setores = new List<string>();
                    while (dr.Read())
                    {
                        if (lista.Where(c => c.ApresentacaoID == dr["ApresentacaoID"].ToInt32()).Count() == 0)
                        {
                            setores = new List<string>();
                            lista.Add(new SerieItemCSS()
                            {
                                Evento = dr["Evento"].ToString(),
                                ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                                Data = dr["Horario"].ToDateTimeFromDB().ToString("dd/MM/yyyy à\\s HH:mm"),
                                Setores = setores,
                                Programacao = dr["Programacao"].ToString(),
                            });
                        }
                        setores.Add(dr["Setor"].ToString());
                    }
                };
                return lista;

            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public List<SetorInfo> CarregarEsquematico(int serieID, int apresentacaoID, IRLib.SingleTonObjects singleTon, ref int mapaEsquematicoID)
        {
            oDal = new DAL();
            try
            {
                List<int> setoresID = new List<int>();
                using (IDataReader dr = oDal.SelectToIDataReader("SELECT DISTINCT SetorID FROM SerieItem WHERE SerieID = " + serieID + " AND ApresentacaoID = " + apresentacaoID))
                    while (dr.Read())
                        setoresID.Add(dr["SetorID"].ToInt32());

                var mapaEsquematicoInfo = singleTon.bufferMapaEsquematico.BuscarInformacaoEstrutura(apresentacaoID);
                mapaEsquematicoID = mapaEsquematicoInfo.ID;

                return
                   (from me in mapaEsquematicoInfo.Setores
                    join s in setoresID on me.ID equals s
                    where me.Coordenadas.Count > 0
                    select me).ToList();
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public string Tipo { get; set; }
    }
}