using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using CTLib;
using IRCore.Util;
using IRLib;
using IRLib.Cinema;
using IRLib.ClientObjects;

namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for CarrinhoLista
    /// </summary>
    public class CarrinhoLista : List<Carrinho>
    {

        DAL oDAL = new DAL();
        BD oBD = new BD();

        public bool TemSerie
        {
            get
            {
                return this.Where(c => c.SerieID > 0).Count() > 0 ? true : false;
            }
        }

        private Carrinho oCarrinho { get; set; }
        private IRLib.CotaItem oCotaItem { get; set; }
        public List<EstruturaPrecoReservaSite> Precos { get; set; }
        private List<EstruturaIDNome> listaPrecos { get; set; }
        public List<EstruturaPrecoQuantidade> ListQuantidadePreco { get; set; }

        public CarrinhoLista()
        {
            this.Clear();
        }
        public bool Verificar()
        {
            String strSql = "select count(a.ID) from tApresentacao(NOLOCK) a where a.ID in (" + String.Join(",", this.Select(t => t.ApresentacaoID).ToList()) + ") and DisponivelVenda <> 'T'";
            return (int)oBD.ConsultaValor(strSql) == 0;
        }

        public CarrinhoLista(Carrinho.TipoReserva tipoReserva)
        {
            this.TipoReserva = tipoReserva;
        }
        private bool Preenchido { get; set; }
        private bool CarregouCompleto { get; set; }
        /// <summary>
        /// Funcao Interna: Retorna uma Lista de Carrinhos de Compra do tipo Carrinho, 
        /// a partir de uma clausula WHERE 
        /// </summary>
        public CarrinhoLista CarregarLista(string clausula)
        {
            string strSql = String.Empty;

            if (clausula != "")
            {
                strSql = "SELECT ID, ClienteID, Codigo, LugarID, IngressoID, TipoLugar, ApresentacaoID, SetorID, PrecoID, Local, Evento, ApresentacaoDataHora, Setor, PrecoNome, PrecoValor, TimeStamp, TaxaConveniencia, Status, GerenciamentoIngressosID " +
                         "FROM Carrinho (NOLOCK) " +
                         "WHERE " + clausula + " ORDER BY ApresentacaoDataHora";
            }

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oCarrinho = new Carrinho(Convert.ToInt32(dr["ID"].ToString()));
                        oCarrinho.ClienteID = Convert.ToInt32(dr["ClienteID"].ToString());
                        oCarrinho.Codigo = dr["Codigo"].ToString();
                        oCarrinho.LugarID = Convert.ToInt32(dr["LugarID"].ToString());
                        oCarrinho.IngressoID = Convert.ToInt32(dr["IngressoID"].ToString());
                        oCarrinho.TipoLugar = dr["TipoLugar"].ToString();
                        oCarrinho.ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"].ToString());
                        oCarrinho.SetorID = Convert.ToInt32(dr["SetorID"].ToString());
                        oCarrinho.PrecoID = Convert.ToInt32(dr["PrecoID"].ToString());
                        oCarrinho.Local = dr["Local"].ToString();
                        oCarrinho.Evento = dr["Evento"].ToString();
                        oCarrinho.ApresentacaoDataHora = DateTime.ParseExact(dr["ApresentacaoDataHora"].ToString(), "yyyyMMddHHmmss", null);
                        oCarrinho.Setor = dr["Setor"].ToString();
                        oCarrinho.PrecoNome = dr["PrecoNome"].ToString();
                        oCarrinho.PrecoValor = (decimal)dr["PrecoValor"];
                        oCarrinho.Timestamp = DateTime.ParseExact(dr["TimeStamp"].ToString(), "yyyyMMddHHmmss", null);
                        oCarrinho.TaxaConveniencia = (decimal)dr["TaxaConveniencia"];
                        oCarrinho.Status = dr["Status"].ToString();
                        oCarrinho.GerenciamentoIngressosID = Convert.ToInt32(dr["GerenciamentoIngressosID"].ToString());
                        this.Add(oCarrinho);
                    }
                }

                oDAL.ConnClose(); // Fecha conexão da classe DataAccess
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public enum Status
        {
            Expirado,
            Reservado,
            Vendido,
            VendidoEmail,
            ReservadoInternet,
            Todos
        }

        private List<CarrinhoContador> carrinhoContadorLista { get; set; }

        private int qtdIngressos { get; set; }

        //Este caso vai buscar do banco
        private int QtdIngressos
        {
            get
            {
                if (this.Preenchido)
                    return this.qtdIngressos;

                switch (this.TipoReserva)
                {
                    case Carrinho.TipoReserva.ValeIngresso:
                        this.PreencherQuantidadeValoresVIR();
                        break;
                    default:
                    case Carrinho.TipoReserva.Ingresso:

                        this.PreencherCarrinhoContador(this[0].ClienteID, this[0].SessionID);

                        this.qtdIngressos += carrinhoContadorLista.Where(c => c.TipoLugar != "M" && string.IsNullOrEmpty(c.PacoteGrupo) && c.SerieID == 0).Count();

                        this.qtdIngressos += this.MontarQuantidadeGrupos();

                        this.qtdIngressos += carrinhoContadorLista.Where(c => c.TipoLugar == "M").Select(c => new { c.ApresentacaoID, c.LugarID }).Distinct().Count();
                        this.qtdIngressos += carrinhoContadorLista.Where(c => !string.IsNullOrEmpty(c.PacoteGrupo)).Select(c => c.PacoteGrupo).Distinct().Count();
                        break;

                }

                return this.qtdIngressos;
            }
        }

        private int MontarQuantidadeGrupos()
        {
            int qtd = 0;
            foreach (int serieID in carrinhoContadorLista.Where(c => c.SerieID > 0).Select(c => c.SerieID).Distinct())
                qtd += new Serie().MontarGrupo(carrinhoContadorLista.Where(c => c.SerieID == serieID));

            return qtd;
        }

        //Este é o caso de quando o Carrinho completo é carregado
        public int QtdIngressosInterno
        {
            get
            {
                int qtd = 0;
                qtd += this.Where(c => c.TipoLugar != "M" && c.PacoteID == 0 && string.IsNullOrEmpty(c.PacoteGrupo) && c.SerieID == 0)
                            .Select(c => c.Quantidade == 0 ? 1 : c.Quantidade).Sum(c => c);


                if (this.Where(c => c.SerieID > 0).Count() > 0)
                    qtd += this.MontarQuantidadeGruposInterno();


                qtd += this.Where(c => c.TipoLugar == "M").Select(c => new { c.ApresentacaoID, c.LugarID, c.Quantidade })
                    .Distinct().Select(c => c.Quantidade == 0 ? 1 : c.Quantidade).Sum(c => c);

                qtd += this.Where(c => !string.IsNullOrEmpty(c.PacoteGrupo)).Select(c => new { c.Quantidade, c.PacoteGrupo }).Distinct()
                    .Select(c => c.Quantidade == 0 ? 1 : c.Quantidade).Sum(c => c);

                return qtd;
            }
        }

        private int MontarQuantidadeGruposInterno()
        {
            int qtd = 0;
            foreach (int serieID in this.Where(c => c.SerieID > 0).Select(c => c.SerieID).Distinct())
                qtd += new Serie().MontarQuantidadeGrupo(this.Where(c => c.SerieID == serieID));

            return qtd;
        }

        private decimal valorTotal { get; set; }
        public decimal ValorTotal
        {
            get
            {
                if (!this.Preenchido)
                    this.PreencherCarrinhoContador(this[0].ClienteID, this[0].SessionID);

                return this.valorTotal;
            }
        }

        private Carrinho.TipoReserva TipoReserva { get; set; }

        public int QuantidadeReservasBanco()
        {
            return QtdIngressos;
        }

        public int QuantidadeReservasMemoria(bool reservando, int itensCarrinho)
        {
            if (reservando)
                return itensCarrinho + QtdIngressosInterno;

            return QtdIngressosInterno;
        }

        public decimal ValorReservas(decimal valorAReservar)
        {
            return this.ValorTotal + valorAReservar;
        }

        private void PreencherCarrinhoContador(int clienteID, string sessionID)
        {
            try
            {
                this.carrinhoContadorLista = new List<CarrinhoContador>();

                if (!CarregouCompleto)
                    using (IDataReader dr = oDAL.SelectToIDataReader
                        (string.Format(
                        @"
                        SELECT 
                            c.EventoID, c.ApresentacaoID, LugarID,
                            l.Estado,
                            IsNull(TipoLugar, '') AS TipoLugar, IsNull(PacoteGrupo, '') AS PacoteGrupo, IsNull(c.SerieID, 0) AS SerieID,
                            IsNull(si.Promocional, 0) AS ItemPromocional, IsNull(si.IR_SerieItemID, 0) AS SerieItemID,
                            IsNull(si.QuantidadePorPromocional, 0) AS QuantidadePorPromocional, ValorTaxaProcessamento, LimiteMaximoIngressosEvento, LimiteMaximoIngressosEstado, PacoteID
                            FROM Carrinho c (NOLOCK)
                            INNER JOIN Evento e (NOLOCK) ON e.IR_EventoID = c.EventoID
                            INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = c.LocalID 
                            LEFT JOIN Serie s(NOLOCK) ON s.ID = c.SerieID
                            LEFT JOIN SerieItem si (NOLOCK) ON si.SerieID = s.ID AND si.PrecoID = c.PrecoID
                            WHERE ClienteID = {0} AND SessionID = '{1}' AND Status = 'R'", clienteID, sessionID)))
                    {
                        while (dr.Read())
                            this.carrinhoContadorLista.Add(new CarrinhoContador
                            {
                                EventoID = dr["EventoID"].ToInt32(),
                                ApresentacaoID = dr["ApresentacaoID"].ToInt32(),
                                LugarID = dr["LugarID"].ToInt32(),
                                TipoLugar = dr["TipoLugar"].ToString(),
                                PacoteGrupo = dr["PacoteGrupo"].ToString(),
                                SerieID = dr["SerieID"].ToInt32(),
                                SerieItemID = dr["SerieItemID"].ToInt32(),
                                ItemPromocional = dr["ItemPromocional"].ToBoolean(),
                                QuantidadePorPromocional = dr["QuantidadePorPromocional"].ToInt32(),
                                Estado = dr["Estado"].ToString(),
                                PossuiTaxaProcessamento = dr["ValorTaxaProcessamento"].ToDecimal() > 0,
                                LimiteMaximoIngressosEvento = dr["LimiteMaximoIngressosEvento"].ToInt32(),
                                LimiteMaximoIngressosEstado = dr["LimiteMaximoIngressosEstado"].ToInt32(),
                                PacoteID = dr["PacoteID"].ToInt32(),
                            });
                    }


                foreach (Carrinho carrinho in this.Where(c => c.PacoteID == 0 && string.IsNullOrEmpty(c.PacoteGrupo)))
                {
                    carrinho.CarregarSerieItem = true;
                    for (int i = 0; i < (carrinho.Quantidade == 0 ? 1 : carrinho.Quantidade); i++)
                        this.carrinhoContadorLista.Add(new CarrinhoContador
                        {
                            EventoID = carrinho.EventoID,
                            ApresentacaoID = carrinho.ApresentacaoID,
                            LugarID = carrinho.LugarID,
                            PacoteID = carrinho.PacoteID,
                            Quantidade = carrinho.Quantidade,
                            PacoteGrupo = string.Empty,
                            SerieID = carrinho.SerieID,
                            TipoLugar = carrinho.TipoLugar,
                            SerieItemID = carrinho.SerieItemID,
                            ItemPromocional = carrinho.ItemPromocional,
                            QuantidadePorPromocional = carrinho.QuantidadePorPromocional,
                            Estado = carrinho.Estado,
                            PossuiTaxaProcessamento = carrinho.PossuiTaxaProcessamento,
                            LimiteMaximoIngressosEvento = carrinho.LimiteMaximoIngressosEvento,
                            LimiteMaximoIngressosEstado = carrinho.LimiteMaximoIngressosEstado
                        });
                }

                foreach (Carrinho carrinho in this.Where(c => !string.IsNullOrEmpty(c.PacoteGrupo)))
                    this.carrinhoContadorLista.Add(new CarrinhoContador()
                    {
                        PacoteID = carrinho.PacoteID,
                        EventoID = carrinho.EventoID,
                        Quantidade = carrinho.Quantidade,
                        Estado = carrinho.Estado,
                        PacoteGrupo = carrinho.PacoteGrupo,
                        PossuiTaxaProcessamento = carrinho.PossuiTaxaProcessamento,
                        LimiteMaximoIngressosEvento = carrinho.LimiteMaximoIngressosEvento,
                        LimiteMaximoIngressosEstado = carrinho.LimiteMaximoIngressosEstado
                    });

                //Esses aqui são os que estão na reserva
                PacoteItemLista pacoteItemLista = new PacoteItemLista();
                foreach (Carrinho carrinho in this.Where(c => c.PacoteID > 0 && string.IsNullOrEmpty(c.PacoteGrupo)))
                {
                    pacoteItemLista.CarregarPorPacoteID(carrinho.PacoteID);
                    for (int i = 0; i < carrinho.Quantidade; i++)
                    {
                        string grupo = Guid.NewGuid().ToString();
                        foreach (var item in pacoteItemLista)
                            this.carrinhoContadorLista.Add(new CarrinhoContador()
                            {
                                PacoteID = item.PacoteID,
                                EventoID = item.EventoID,
                                Estado = item.Estado,
                                PacoteGrupo = grupo, //Só um place holder pra falar q eh um pacote e pegar os distintos
                                PossuiTaxaProcessamento = item.PossuiTaxaProcessamento,
                                LimiteMaximoIngressosEstado = item.LimiteMaximoIngressosEstado,
                                LimiteMaximoIngressosEvento = item.LimiteMaximoIngressosEvento,
                            });
                    }
                }
                this.Preenchido = true;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        private void PreencherQuantidadeValoresVIR()
        {
            try
            {
                string strSQL =
                    string.Format(@"SELECT Count(ValeIngressoID) AS Quantidade, IsNull(SUM(PrecoValor + IsNull(TaxaConveniencia, 0)), 0) AS Valor FROM Carrinho (NOLOCK) WHERE  ClienteID = {0} AND SessionID = '{1}' AND Status = '{2}'",
                    HttpContext.Current.Session["ClienteID"].ToInt32(), HttpContext.Current.Session.SessionID, "R");

                using (IDataReader dr = oDAL.SelectToIDataReader(strSQL))
                {
                    while (dr.Read())
                    {
                        this.qtdIngressos += dr["Quantidade"].ToInt32();
                        this.valorTotal += dr["Valor"].ToDecimal();
                    }
                }
                this.Preenchido = true;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public string VerificarLimitesPrecoValorQuantidade(decimal valorAReservar, Carrinho.TipoReserva reserva, bool serie, int itensCarrinho)
        {
            return VerificarLimitesPrecoValorQuantidade(valorAReservar, reserva, serie,
                    (reserva == Carrinho.TipoReserva.Ingresso && Precos != null && Precos.Sum(c => c.Valor * c.Quantidade) > 0),
                    this.Any(c => c.PossuiTaxaProcessamento || c.PacoteID > 0), itensCarrinho);
        }

        public string VerificarLimitesPrecoValorQuantidade(decimal valorAReservar, Carrinho.TipoReserva reserva, bool serie, bool reservando, bool carregarBanco, int itensCarrinho)
        {
            var limiteCarrinho = Convert.ToInt32(ConfigurationManager.AppSettings["LimiteCarrinho"]);

            var retorno = String.Empty;
            int numItensCarrinho;

            TipoReserva = reserva;

            if (serie || carregarBanco)
                numItensCarrinho = QuantidadeReservasBanco();
            else
                numItensCarrinho = QuantidadeReservasMemoria(reservando, itensCarrinho);

            // Verifica se ultrapassou o limite de quantidade de ingressos por compra.
            if (numItensCarrinho > limiteCarrinho || itensCarrinho >= limiteCarrinho)
                retorno = string.Format("O limite de compras através do site é de {0} itens.", limiteCarrinho);

            if (carrinhoContadorLista == null || !carrinhoContadorLista.Any(c => c.PossuiTaxaProcessamento))
                return retorno;

            VerificaExcecaoLimiteIngressosEstados();
            VerificaExcecaoLimiteIngressosEvento();

            return retorno;
        }

        public void VerificaExcecaoLimiteIngressosEstados()
        {
            foreach (var estado in carrinhoContadorLista.Where(c => c.LimiteMaximoIngressosEstado > 0 && c.PossuiTaxaProcessamento && !string.IsNullOrEmpty(c.Estado)).Select(c => new { c.Estado, c.LimiteMaximoIngressosEstado }).Distinct())
            {
                var qtd = 0;
                qtd += carrinhoContadorLista.Count(c => c.LimiteMaximoIngressosEstado > 0 && c.PossuiTaxaProcessamento && c.Estado == estado.Estado && c.TipoLugar != ((char)Setor.LugarTipo.MesaFechada).ToString() && string.IsNullOrEmpty(c.PacoteGrupo) && c.SerieID == 0);

                //Quantidade de ingressos por mesa
                var jaAdicionouMesa = false;
                foreach (var mesa in carrinhoContadorLista.Where(c => c.LimiteMaximoIngressosEstado > 0 && c.PossuiTaxaProcessamento && c.Estado == estado.Estado && c.TipoLugar == ((char)Setor.LugarTipo.MesaFechada).ToString()).Select(c => new { c.ApresentacaoID, c.LugarID }).Distinct())
                {
                    var temMaisMesas = carrinhoContadorLista.Where(c => c.LimiteMaximoIngressosEstado > 0 && c.PossuiTaxaProcessamento && c.Estado == estado.Estado && c.TipoLugar == ((char)Setor.LugarTipo.MesaFechada).ToString()).Select(c => new { c.ApresentacaoID, c.LugarID }).Distinct().Count() >= 2;

                    var qtdIngressosMesa = carrinhoContadorLista.Where(c => c.LimiteMaximoIngressosEstado > 0 && c.PossuiTaxaProcessamento && c.ApresentacaoID == mesa.ApresentacaoID && c.LugarID == mesa.LugarID && c.Estado == estado.Estado && c.TipoLugar == ((char)Setor.LugarTipo.MesaFechada).ToString()).Select(c => new { c.ApresentacaoID, c.LugarID }).Count();

                    if (jaAdicionouMesa && temMaisMesas && qtd + qtdIngressosMesa > estado.LimiteMaximoIngressosEstado)
                        throw new Exception(string.Format("O limite de ingressos para {0} é de {1} por compra. No entanto, é permitido efetuar a compra de uma única mesa fechada.", estado.Estado, estado.LimiteMaximoIngressosEstado));

                    jaAdicionouMesa = true;

                    qtd++;
                }

                var jaAdicionouPacote = false;
                foreach (var pacote in carrinhoContadorLista.Where(c => c.LimiteMaximoIngressosEstado > 0 && c.PossuiTaxaProcessamento && c.Estado == estado.Estado && !string.IsNullOrEmpty(c.PacoteGrupo)).Select(c => new { c.PacoteID, c.PacoteGrupo }).Distinct())
                {
                    var temMaisPacotes = carrinhoContadorLista.Where(c => c.LimiteMaximoIngressosEstado > 0 && c.PossuiTaxaProcessamento && c.Estado == estado.Estado && !string.IsNullOrEmpty(c.PacoteGrupo)).Select(c => c.PacoteGrupo).Distinct().Count() >= 2;

                    var qtdIngressosPacote = carrinhoContadorLista.Where(c => c.LimiteMaximoIngressosEstado > 0 && c.PossuiTaxaProcessamento && c.PacoteID == pacote.PacoteID && c.PacoteGrupo == pacote.PacoteGrupo).Distinct().Count();

                    if (jaAdicionouPacote && temMaisPacotes && qtd + qtdIngressosPacote > estado.LimiteMaximoIngressosEstado)
                        throw new Exception(string.Format("O limite de ingressos para {0} é de {1} por compra. No entanto, é possível efetuar a compra de um único pacote.", estado.Estado, estado.LimiteMaximoIngressosEstado));

                    jaAdicionouPacote = true;
                    qtd++;
                }

                if (qtd > estado.LimiteMaximoIngressosEstado)
                    throw new Exception(string.Format("O limite de ingressos para {0} é de {1} por compra.", estado.Estado, estado.LimiteMaximoIngressosEstado));
            }
        }

        public void VerificaExcecaoLimiteIngressosEvento()
        {
            foreach (var evento in carrinhoContadorLista.Where(c => c.LimiteMaximoIngressosEvento > 0 && c.PossuiTaxaProcessamento && c.EventoID > 0).Select(c => new { c.EventoID, c.LimiteMaximoIngressosEvento }).Distinct())
            {
                var qtd = 0;
                qtd += carrinhoContadorLista.Count(c => c.PossuiTaxaProcessamento && c.EventoID == evento.EventoID && c.TipoLugar != ((char)Setor.LugarTipo.MesaFechada).ToString() && string.IsNullOrEmpty(c.PacoteGrupo) && c.SerieID == 0);

                //Quantidade de ingressos por mesa
                var jaAdicionouMesa = false;
                foreach (var mesa in carrinhoContadorLista.Where(c => c.PossuiTaxaProcessamento && c.EventoID == evento.EventoID && c.TipoLugar == ((char)Setor.LugarTipo.MesaFechada).ToString()).Select(c => new { c.ApresentacaoID, c.LugarID }).Distinct())
                {
                    var temMaisMesas = carrinhoContadorLista.Where(c => c.PossuiTaxaProcessamento && c.EventoID == evento.EventoID && c.TipoLugar == ((char)Setor.LugarTipo.MesaFechada).ToString()).Select(c => new { c.ApresentacaoID, c.LugarID }).Distinct().Count() >= 2;

                    var qtdIngressosMesa = carrinhoContadorLista.Where(c => c.PossuiTaxaProcessamento && c.ApresentacaoID == mesa.ApresentacaoID && c.LugarID == mesa.LugarID && c.EventoID == evento.EventoID && c.TipoLugar == ((char)Setor.LugarTipo.MesaFechada).ToString()).Select(c => new { c.ApresentacaoID, c.LugarID }).Count();

                    if (jaAdicionouMesa && temMaisMesas && qtd + qtdIngressosMesa > evento.LimiteMaximoIngressosEvento)
                        throw new Exception(string.Format("O limite de ingressos para este Evento é de {0} por compra. No entanto, é permitido efetuar a compra de uma única mesa fechada.", evento.LimiteMaximoIngressosEvento));

                    jaAdicionouMesa = true;

                    qtd++;
                }

                var jaAdicionouPacote = false;
                foreach (var pacote in carrinhoContadorLista.Where(c => c.PossuiTaxaProcessamento && c.EventoID == evento.EventoID && !string.IsNullOrEmpty(c.PacoteGrupo)).Select(c => new { c.PacoteID, c.PacoteGrupo }).Distinct())
                {
                    var temMaisPacotes = carrinhoContadorLista.Where(c => c.PossuiTaxaProcessamento && c.EventoID == evento.EventoID && !string.IsNullOrEmpty(c.PacoteGrupo)).Select(c => c.PacoteGrupo).Distinct().Count() >= 2;

                    var qtdIngressosPacote = carrinhoContadorLista.Where(c => c.PossuiTaxaProcessamento && c.PacoteID == pacote.PacoteID && c.PacoteGrupo == pacote.PacoteGrupo).Distinct().Count();

                    if (jaAdicionouPacote && temMaisPacotes && qtd + qtdIngressosPacote > evento.LimiteMaximoIngressosEvento)
                        throw new Exception(string.Format("O limite de ingressos para este Evento é de {0} por compra. No entanto, é permitido efetuar a compra de um único pacote.", evento.LimiteMaximoIngressosEvento));

                    jaAdicionouPacote = true;
                    qtd++;
                }

                if (qtd > evento.LimiteMaximoIngressosEvento)
                    throw new Exception(string.Format("O limite de ingressos para este Evento é de {0} por compra.", evento.LimiteMaximoIngressosEvento));
            }
        }

        /// <summary>
        /// Método que verifica se a reserva que está sendo realizada é de um Acompanhante de PNE
        /// Executa a chamada de uma Stored Procedure que realiza a checagem da regra
        /// Se o retorno for igual a zero, método retorna FALSE, que significa que a reserva não deve ser realizada.
        /// Se o retorno for igual a 1, método retorna TRUE, que significa que a reserva pode ser realizada
        /// </summary>
        /// <param name="ingressoID">Parâmetro obrigatório, do IngressoID que está se desejando reservar. Relacionado ao campo ID da tabela tIngresso</param>
        /// <param name="sessionid">Parâmetro Opcional. Em caso de existir a sessão do usuario, passar para que seja verificado no Carrinho da tabela SiteIR</param>
        /// <param name="usuarioID">Parâmetro Opcional. Em caso de não existir a sessão do usuário, passar o UsuarioID que está realizando a reserva. Usado para o caso do IRBilheteria, onde não existe Sessão mas existe o usuário do Caixa logado</param>
        /// <returns>bool</returns>
        public bool VerificaReservaPNE(int ingressoID, string sessionID = "", int usuarioID = 0)
        {

            BD bd = new BD();
            string sql = string.Format("EXEC CheckAcompanhantePNE_Reserva {0},'{1}',{2}", ingressoID, sessionID, usuarioID);
            bool retorno = false;
            try
            {
                bd.Consulta(sql);
                while (bd.Consulta().Read())
                {
                    retorno = bd.LerInt("Retorno") == 1 ? true : false;
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                bd.Fechar();
            }
        }

        /// <summary>
        /// Retorna uma Lista de Carrinhos de Compra do tipo Carrinho, 
        /// a partir de um ClienteID com a VendaBilheteriaID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionID"></param>
        /// <param name="status"></param>
        /// <param name="vendaBilheteriaID"></param>
        /// <returns></returns>
        public CarrinhoLista CarregarDadosPorClienteID(int id, string sessionID, Status status, long vendaBilheteriaID)
        {
            LogUtil.Debug(string.Format("##CarrinhoLista.CarregandoDadosDeCota## SESSION {0}", sessionID));

            this.Clear(); // Limpa a collection
            string whereStatus = "";
            switch (status)
            {
                case Status.Expirado:
                    whereStatus = " AND Status = 'E' ";
                    break;

                case Status.Vendido:
                    whereStatus = " AND Status = 'V' ";
                    break;

                case Status.VendidoEmail:
                    whereStatus = " AND Status = 'VV' ";
                    break;

                case Status.Reservado:
                    whereStatus = " AND Status = 'R' ";
                    break;
                case Status.ReservadoInternet:
                    whereStatus = " AND Status = 'R' OR Status = 'S'";
                    break;

                case Status.Todos:
                default:
                    whereStatus = " AND (Status = 'R' OR Status = 'E')";
                    break;
            }

            //Busca Tudo menos mesa Fechada e Pacote
            StringBuilder stbSQL = new StringBuilder();

            string strBusca = string.Format(
                @"SELECT DISTINCT
                c.ID, ClienteID, Codigo, LugarID, IngressoID, TipoLugar, ApresentacaoID, SetorID, PrecoID, LocalID, EventoID,c.GerenciamentoIngressosID, 
                ISNULL(PrecoExclusivoCodigoID, 0) as PrecoExclusivoCodigoID, Local, Evento, ApresentacaoDataHora, Setor, PrecoNome, PrecoValor, TimeStamp, TaxaConveniencia, Status, 
                IsNull(CotaItemID, 0) AS CotaItemID, IsNull(CotaItemIDAPS, 0) AS CotaItemIDAPS, IsNull(SerieID, 0) AS SerieID, IsNull(l.TaxaMaximaEmpresa, 0) AS TaxaMaximaEmpresa,
                IsNull(ci.IR_CotaItemID, 0) AS CotaItemID, IsNull(ci.ValidaBin, 0) AS ValidaBin, IsNull(ci.ParceiroID, 0) AS ParceiroID, 
                IsNull(ci.Nominal, 0) AS Nominal, IsNull(ci.TextoValidacao, '') AS TextoValidacao, IsNull(ci.ObrigatoriedadeID, 0) AS ObrigatoriedadeID,
                IsNull(DonoID, 0) AS DonoID, IsNull(DonoCPF, '') AS DonoCPF, IsNull(CodigoPromocional, '') AS CodigoPromocional,
                CASE WHEN ci.Termo IS NOT NULL AND LEN(ci.Termo) > 0
                    THEN 1
                    ELSE 0
                END AS TemTermo, IsNull(CotaVerificada, 0) AS CotaVerificada,
                c.CotaItemID AS CotaItemIDCarrinho, c.CotaItemIDAPS AS CotaItemIDAPSCarrinho,
                IsNull(PacoteNome, '') AS PacoteNome, IsNull(PacoteGrupo, '') AS PacoteGrupo, IsNull(IsSpecial, 0) AS SpecialEvent, c.EmpresaID,
                c.ValorTaxaProcessamento, Estado, ISNULL(PacoteID, 0) AS PacoteID
                FROM Carrinho AS c (NOLOCK)
                INNER JOIN Local l (NOLOCK) on c.LocalID = l.IR_LocalID
                LEFT JOIN CotaItem ci (NOLOCK) ON (c.CotaItemID = ci.IR_CotaItemID AND c.CotaItemIDAPS = 0) OR c.CotaItemIDAPS = ci.IR_CotaItemID
                WHERE ClienteID = {0} AND SessionID = '{1}' {2} AND ValeIngressoTipoID IS NULL {3}", id, sessionID, vendaBilheteriaID > 0 ? "AND VendaBilheteriaID = " + vendaBilheteriaID : string.Empty,
                whereStatus);

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strBusca);

                while (dr.Read()) // while --> Tudo menos mesa Fechada e Pacote
                {
                    oCarrinho = new Carrinho(dr["ID"].ToInt32());
                    oCarrinho.ClienteID = dr["ClienteID"].ToInt32();
                    oCarrinho.Codigo = dr["Codigo"].ToString();
                    oCarrinho.PacoteNome = dr["PacoteNome"].ToString();
                    oCarrinho.PacoteGrupo = dr["PacoteGrupo"].ToString();
                    oCarrinho.PacoteID = dr["PacoteID"].ToInt32();
                    oCarrinho.LugarID = dr["LugarID"].ToInt32();
                    oCarrinho.IngressoID = dr["IngressoID"].ToInt32();
                    oCarrinho.TipoLugar = dr["TipoLugar"].ToString();
                    oCarrinho.ApresentacaoID = dr["ApresentacaoID"].ToInt32();
                    oCarrinho.SetorID = dr["SetorID"].ToInt32();
                    oCarrinho.PrecoID = dr["PrecoID"].ToInt32();
                    oCarrinho.GerenciamentoIngressosID = dr["GerenciamentoIngressosID"].ToInt32();
                    oCarrinho.LocalID = dr["LocalID"].ToInt32();
                    oCarrinho.Local = dr["Local"].ToString();
                    oCarrinho.Evento = dr["Evento"].ToString();
                    oCarrinho.EventoID = dr["EventoID"].ToInt32();
                    oCarrinho.PrecoExclusivoCodigoID = dr["PrecoExclusivoCodigoID"].ToInt32();
                    oCarrinho.ApresentacaoDataHora = DateTime.ParseExact(dr["ApresentacaoDataHora"].ToString(), "yyyyMMddHHmmss", null);
                    oCarrinho.Setor = dr["Setor"].ToString();
                    oCarrinho.PrecoNome = dr["PrecoNome"].ToString();
                    oCarrinho.PrecoValor = dr["PrecoValor"].ToDecimal();
                    oCarrinho.Timestamp = DateTime.ParseExact(dr["TimeStamp"].ToString(), "yyyyMMddHHmmss", null);
                    oCarrinho.TaxaConveniencia = dr["TaxaConveniencia"].ToDecimal();
                    oCarrinho.CotaItemID = dr["CotaItemIDCarrinho"].ToInt32();
                    oCarrinho.CotaItemIDAPS = dr["CotaItemIDAPSCarrinho"].ToInt32();
                    oCarrinho.Status = dr["Status"].ToString();
                    oCarrinho.SerieID = dr["SerieID"].ToInt32();
                    oCarrinho.TaxaMaximaEmpresa = dr["TaxaMaximaEmpresa"].ToDecimal();
                    oCarrinho.CodigoPromocional = dr["CodigoPromocional"].ToString();
                    oCarrinho.SpecialEvent = dr["SpecialEvent"].ToInt32();
                    oCarrinho.EmpresaID = dr["EmpresaID"].ToInt32();
                    oCarrinho.Estado = dr["Estado"].ToString();
                    oCarrinho.TaxaProcessamento = dr["ValorTaxaProcessamento"].ToDecimal();

                    oCarrinho.Precos = new List<EstruturaIDNome>();

                    var ParceiroID = dr["ParceiroID"].ToInt32();
                    var ValidaBin = dr["ValidaBin"].ToBoolean();

                    var cotaItemControle = new CotaItemControle();
                    var qtd = cotaItemControle.getQuantidadeCota(dr["CotaItemID"].ToInt32(), dr["ApresentacaoID"].ToInt32());

                    oCarrinho.CotaItem = new CotaItem
                    {
                        ID = dr["CotaItemID"].ToInt32(),
                        ParceiroID = ParceiroID,
                        Quantidade = qtd,
                        Nominal = dr["Nominal"].ToBoolean(),
                        ValidaBin = ValidaBin,
                        TextoValidacao = dr["TextoValidacao"].ToString(),
                        DonoID = dr["DonoID"].ToInt32(),
                        DonoCPF = dr["DonoCPF"].ToString(),
                        CodigoPromocional = dr["CodigoPromocional"].ToString(),
                        TemTermo = dr["TemTermo"].ToBoolean(),
                        IngressoID = dr["IngressoID"].ToInt32(),
                        Verificado = dr["CotaVerificada"].ToBoolean(),
                        ObrigatoriedadeID = dr["ObrigatoriedadeID"].ToInt32(),
                        ValidaCodidoPromocional = (ParceiroID > 0 && !ValidaBin ? true : false)
                    };
                    this.Add(oCarrinho);
                }

                oDAL.ConnClose();

                dr.Close();
                dr.Dispose();
                this.CarregouCompleto = true;

                LogUtil.Debug(string.Format("##CarrinhoLista.CarregandoDadosDeCota.SUCCESS## SESSION {0}", sessionID));

                return this;
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##CarrinhoLista.CarregandoDadosDeCota.EXCEPTION## SESSION {0}, MSG {1}", sessionID, ex.Message), ex);

                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public CarrinhoLista CarregarDadosPorSessionID(string sessionID, Status status, long vendaBilheteriaID)
        {
            this.Clear(); // Limpa a collection
            string whereStatus = "";
            switch (status)
            {
                case Status.Expirado:
                    whereStatus = " AND Status = 'E' ";
                    break;

                case Status.Vendido:
                    whereStatus = " AND Status = 'V' ";
                    break;

                case Status.VendidoEmail:
                    whereStatus = " AND Status = 'VV' ";
                    break;

                case Status.Reservado:
                    whereStatus = " AND Status = 'R' ";
                    break;
                case Status.ReservadoInternet:
                    whereStatus = " AND Status = 'R' OR Status = 'S'";
                    break;

                case Status.Todos:
                default:
                    whereStatus = " AND (Status = 'R' OR Status = 'E')";
                    break;
            }

            //Busca Tudo menos mesa Fechada e Pacote
            StringBuilder stbSQL = new StringBuilder();

            string strBusca = string.Format(
                @"SELECT DISTINCT
                c.ID, ClienteID, Codigo, LugarID, IngressoID, TipoLugar, ApresentacaoID, SetorID, PrecoID, LocalID, EventoID,c.GerenciamentoIngressosID, 
                ISNULL(PrecoExclusivoCodigoID, 0) as PrecoExclusivoCodigoID, Local, Evento, ApresentacaoDataHora, Setor, PrecoNome, PrecoValor, TimeStamp, TaxaConveniencia, Status, 
                IsNull(CotaItemID, 0) AS CotaItemID, IsNull(CotaItemIDAPS, 0) AS CotaItemIDAPS, IsNull(SerieID, 0) AS SerieID, IsNull(l.TaxaMaximaEmpresa, 0) AS TaxaMaximaEmpresa,
                IsNull(ci.IR_CotaItemID, 0) AS CotaItemID, IsNull(ci.ValidaBin, 0) AS ValidaBin, IsNull(ci.ParceiroID, 0) AS ParceiroID, 
                IsNull(ci.Nominal, 0) AS Nominal, IsNull(ci.TextoValidacao, '') AS TextoValidacao, IsNull(ci.ObrigatoriedadeID, 0) AS ObrigatoriedadeID,
                IsNull(DonoID, 0) AS DonoID, IsNull(DonoCPF, '') AS DonoCPF, IsNull(CodigoPromocional, '') AS CodigoPromocional,
                CASE WHEN ci.Termo IS NOT NULL AND LEN(ci.Termo) > 0
                    THEN 1
                    ELSE 0
                END AS TemTermo, IsNull(CotaVerificada, 0) AS CotaVerificada,
                c.CotaItemID AS CotaItemIDCarrinho, c.CotaItemIDAPS AS CotaItemIDAPSCarrinho,
                IsNull(PacoteNome, '') AS PacoteNome, IsNull(PacoteGrupo, '') AS PacoteGrupo, IsNull(IsSpecial, 0) AS SpecialEvent, c.EmpresaID,
                c.ValorTaxaProcessamento, Estado, ISNULL(PacoteID, 0) AS PacoteID
                FROM Carrinho AS c (NOLOCK)
                INNER JOIN Local l (NOLOCK) on c.LocalID = l.IR_LocalID
                LEFT JOIN CotaItem ci (NOLOCK) ON (c.CotaItemID = ci.IR_CotaItemID AND c.CotaItemIDAPS = 0) OR c.CotaItemIDAPS = ci.IR_CotaItemID
                WHERE SessionID = '{0}' {1} AND ValeIngressoTipoID IS NULL {2}", sessionID, vendaBilheteriaID > 0 ? "AND VendaBilheteriaID = " + vendaBilheteriaID : string.Empty,
                whereStatus);

            try
            {
                IDataReader dr = oDAL.SelectToIDataReader(strBusca);

                while (dr.Read()) // while --> Tudo menos mesa Fechada e Pacote
                {
                    oCarrinho = new Carrinho(dr["ID"].ToInt32());
                    oCarrinho.ClienteID = dr["ClienteID"].ToInt32();
                    oCarrinho.Codigo = dr["Codigo"].ToString();
                    oCarrinho.PacoteNome = dr["PacoteNome"].ToString();
                    oCarrinho.PacoteGrupo = dr["PacoteGrupo"].ToString();
                    oCarrinho.PacoteID = dr["PacoteID"].ToInt32();
                    oCarrinho.LugarID = dr["LugarID"].ToInt32();
                    oCarrinho.IngressoID = dr["IngressoID"].ToInt32();
                    oCarrinho.TipoLugar = dr["TipoLugar"].ToString();
                    oCarrinho.ApresentacaoID = dr["ApresentacaoID"].ToInt32();
                    oCarrinho.SetorID = dr["SetorID"].ToInt32();
                    oCarrinho.PrecoID = dr["PrecoID"].ToInt32();
                    oCarrinho.GerenciamentoIngressosID = dr["GerenciamentoIngressosID"].ToInt32();
                    oCarrinho.LocalID = dr["LocalID"].ToInt32();
                    oCarrinho.Local = dr["Local"].ToString();
                    oCarrinho.Evento = dr["Evento"].ToString();
                    oCarrinho.EventoID = dr["EventoID"].ToInt32();
                    oCarrinho.PrecoExclusivoCodigoID = dr["PrecoExclusivoCodigoID"].ToInt32();
                    oCarrinho.ApresentacaoDataHora = DateTime.ParseExact(dr["ApresentacaoDataHora"].ToString(), "yyyyMMddHHmmss", null);
                    oCarrinho.Setor = dr["Setor"].ToString();
                    oCarrinho.PrecoNome = dr["PrecoNome"].ToString();
                    oCarrinho.PrecoValor = dr["PrecoValor"].ToDecimal();
                    oCarrinho.Timestamp = DateTime.ParseExact(dr["TimeStamp"].ToString(), "yyyyMMddHHmmss", null);
                    oCarrinho.TaxaConveniencia = dr["TaxaConveniencia"].ToDecimal();
                    oCarrinho.CotaItemID = dr["CotaItemIDCarrinho"].ToInt32();
                    oCarrinho.CotaItemIDAPS = dr["CotaItemIDAPSCarrinho"].ToInt32();
                    oCarrinho.Status = dr["Status"].ToString();
                    oCarrinho.SerieID = dr["SerieID"].ToInt32();
                    oCarrinho.TaxaMaximaEmpresa = dr["TaxaMaximaEmpresa"].ToDecimal();
                    oCarrinho.CodigoPromocional = dr["CodigoPromocional"].ToString();
                    oCarrinho.SpecialEvent = dr["SpecialEvent"].ToInt32();
                    oCarrinho.EmpresaID = dr["EmpresaID"].ToInt32();
                    oCarrinho.Estado = dr["Estado"].ToString();
                    oCarrinho.TaxaProcessamento = dr["ValorTaxaProcessamento"].ToDecimal();

                    oCarrinho.Precos = new List<EstruturaIDNome>();

                    var ParceiroID = dr["ParceiroID"].ToInt32();
                    var ValidaBin = dr["ValidaBin"].ToBoolean();

                    var cotaItemControle = new CotaItemControle();
                    var qtd = cotaItemControle.getQuantidadeCota(dr["CotaItemID"].ToInt32(), dr["ApresentacaoID"].ToInt32());

                    oCarrinho.CotaItem = new CotaItem
                    {
                        ID = dr["CotaItemID"].ToInt32(),
                        ParceiroID = ParceiroID,
                        Nominal = dr["Nominal"].ToBoolean(),
                        Quantidade = qtd,
                        ValidaBin = ValidaBin,
                        TextoValidacao = dr["TextoValidacao"].ToString(),
                        DonoID = dr["DonoID"].ToInt32(),
                        DonoCPF = dr["DonoCPF"].ToString(),
                        CodigoPromocional = dr["CodigoPromocional"].ToString(),
                        TemTermo = dr["TemTermo"].ToBoolean(),
                        IngressoID = dr["IngressoID"].ToInt32(),
                        Verificado = dr["CotaVerificada"].ToBoolean(),
                        ObrigatoriedadeID = dr["ObrigatoriedadeID"].ToInt32(),
                        ValidaCodidoPromocional = (ParceiroID > 0 && !ValidaBin ? true : false)
                    };
                    this.Add(oCarrinho);
                }

                oDAL.ConnClose();

                dr.Close();
                dr.Dispose();
                this.CarregouCompleto = true;
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        /// <summary>
        /// ESTE MÉTODO SÓ DEVE SER UTILIZADO CASO SEJA NECESSÁRIO EFETUAR A TROCA OU VISUALIZAÇÃO DOS PREÇOS,
        /// ELE TORNA A ROTINA MAIS LENTA JÁ QUE TEM QUE CARREGAR TODOS OS PREÇOS DO SETOR!!!!!!!!!!!!!!!!!!
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public CarrinhoLista CarregarDadosComListaPrecos(int id, string sessionID, Status status = Status.Todos)
        {
            this.CarregarDadosPorClienteID(id, sessionID, status, 0);
            this.PreencherPrecos();
            return this;
        }

        public void PreencherPrecos()
        {
            try
            {
                string sql = string.Empty;
                List<EstruturaIDNome> lstPrecos = new List<EstruturaIDNome>();
                DataTable dttBulk = new DataTable();
                dttBulk.Columns.Add("ApresentacaoID");
                dttBulk.Columns.Add("SetorID");
                DataRow dtr;

                foreach (var oCarrinho in this.Where(c => string.IsNullOrEmpty(c.PacoteGrupo) && c.SerieID == 0 && c.SpecialEvent == 0 && c.StatusDetalhado.Length == 0)
                    .Select(c => new { ApresentacaoID = c.ApresentacaoID, SetorID = c.SetorID })
                    .Distinct())
                {
                    dtr = dttBulk.NewRow();
                    dtr["ApresentacaoID"] = oCarrinho.ApresentacaoID;
                    dtr["SetorID"] = oCarrinho.SetorID;
                    dttBulk.Rows.Add(dtr);
                }

                oDAL.Execute("CREATE TABLE #tempApresentacaoSetor (ApresentacaoID INT, SetorID INT)");
                oDAL.BulkInsert(dttBulk, "#tempApresentacaoSetor", false);

                using (IDataReader dr = oDAL.SelectToIDataReader(
                            @"SELECT p.IR_PrecoID, p.Nome, p.ApresentacaoID, p.SetorID, p.Valor
                                FROM Preco p (NOLOCK)
                                INNER JOIN #tempApresentacaoSetor t ON p.ApresentacaoID = t.ApresentacaoID AND p.SetorID = t.SetorID
                                WHERE Pacote = 0 AND Serie = 0
                                ORDER BY p.Nome"))
                {
                    while (dr.Read())
                    {
                        foreach (Carrinho carrinho in this.Where(c => c.ApresentacaoID == Convert.ToInt32(dr["ApresentacaoID"]) && c.SetorID == Convert.ToInt32(dr["SetorID"])))
                        {
                            if (carrinho.Precos.Where(c => c.ID == Convert.ToInt32(dr["IR_PrecoID"])).Count() > 0)
                                continue;

                            carrinho.Precos.Add(new EstruturaIDNome()
                            {
                                ID = Convert.ToInt32(dr["IR_PrecoID"]),
                                Nome = dr["Nome"].ToString(),
                                Valor = dr["Valor"].ToDecimal(),
                            });
                        }
                    }
                }

                dttBulk.Clear();
                dttBulk.Columns.Add("SerieID");

                foreach (var oCarrinho in this.Where(c => c.SerieID > 0 && c.StatusDetalhado.Length == 0)
                     .Select(c => new { ApresentacaoID = c.ApresentacaoID, SetorID = c.SetorID, SerieID = c.SerieID })
                     .Distinct())
                {
                    dtr = dttBulk.NewRow();
                    dtr["ApresentacaoID"] = oCarrinho.ApresentacaoID;
                    dtr["SetorID"] = oCarrinho.SetorID;
                    dtr["SerieID"] = oCarrinho.SerieID;
                    dttBulk.Rows.Add(dtr);
                }

                if (dttBulk.Rows.Count == 0)
                    return;

                oDAL.Execute("CREATE TABLE #tempApresentacaoSetorSerie (ApresentacaoID INT, SetorID INT, SerieID INT)");
                oDAL.BulkInsert(dttBulk, "#tempApresentacaoSetorSerie", false);


                using (IDataReader dr = oDAL.SelectToIDataReader(
                            @"SELECT p.IR_PrecoID, p.Nome, p.ApresentacaoID, p.SetorID, si.SerieID, p.Valor
                                FROM #tempApresentacaoSetorSerie t
                                INNER JOIN SerieItem si ON si.SerieID = t.SerieID AND si.ApresentacaoID = t.ApresentacaoID AND si.SetorID = t.SetorID
                                INNER JOIN Preco p ON p.IR_PrecoID = si.PrecoID
                                ORDER BY p.Nome DESC"))
                {
                    while (dr.Read())
                    {
                        foreach (Carrinho carrinho in this.Where(c => c.ApresentacaoID == Convert.ToInt32(dr["ApresentacaoID"]) && c.SetorID == Convert.ToInt32(dr["SetorID"]) && c.SerieID == dr["SerieID"].ToInt32()))
                        {
                            if (carrinho.Precos.Where(c => c.ID == Convert.ToInt32(dr["IR_PrecoID"])).Count() > 0)
                                continue;

                            carrinho.Precos.Add(new EstruturaIDNome()
                            {
                                ID = Convert.ToInt32(dr["IR_PrecoID"]),
                                Nome = dr["Nome"].ToString(),
                                Valor = dr["Valor"].ToDecimal(),
                            });
                        }
                    }
                }
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public List<EstruturaIDNome> PreencherPrecosPorReserva(int apresentacaoID, int setorID, int serieID)
        {
            try
            {
                List<EstruturaIDNome> lstPrecos = new List<EstruturaIDNome>();
                if (serieID == 0)
                {
                    using (IDataReader dr = oDAL.SelectToIDataReader(
                        string.Format(@"SELECT
                            p.IR_PrecoID AS ID, p.Nome, p.Valor
                            FROM Preco p (NOLOCK)
                            WHERE p.ApresentacaoID = {0} AND p.SetorID = {1} AND Pacote = 0 AND Serie = 0", apresentacaoID, setorID)))
                    {
                        while (dr.Read())
                            lstPrecos.Add(new EstruturaIDNome()
                            {
                                ID = dr["ID"].ToInt32(),
                                Nome = dr["Nome"].ToString(),
                                Valor = dr["Valor"].ToDecimal(),
                            });
                    }
                }
                else
                {
                    using (IDataReader dr = oDAL.SelectToIDataReader(
                        string.Format(@"SELECT
                            p.IR_PrecoID AS ID, p.Nome, p.Valor
                            FROM SerieItem si (NOLOCK)
                            INNER JOIN Preco p (NOLOCK) ON p.IR_PrecoID = si.PrecoID
                            WHERE si.SerieID = {0} AND p.ApresentacaoID = {1} AND p.SetorID = {2}

                        ", serieID, apresentacaoID, setorID)))
                    {
                        while (dr.Read())
                            lstPrecos.Add(new EstruturaIDNome()
                            {
                                ID = dr["ID"].ToInt32(),
                                Nome = dr["Nome"].ToString(),
                                Valor = dr["Valor"].ToDecimal(),
                            });
                    }
                }
                return lstPrecos;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        /// <summary>
        /// Busca Somente os Vale Ingressos que estão reservados
        /// </summary>
        /// <param name="clienteID"></param>
        /// <param name="sessionID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public CarrinhoLista CarregarDadosVIRporClienteIDSessionID(int clienteID, string sessionID, Status status)
        {
            this.Clear();

            string whereStatus = string.Empty;
            switch (status)
            {
                case Status.Expirado:
                    whereStatus = " AND Status = 'E' ";
                    break;

                case Status.Vendido:
                    whereStatus = " AND Status = 'V' ";
                    break;

                case Status.VendidoEmail:
                    whereStatus = " AND Status = 'VV' ";
                    break;

                case Status.Reservado:
                    whereStatus = " AND Status = 'R' ";
                    break;
                case Status.ReservadoInternet:
                    whereStatus = " AND Status = 'R' OR Status = 'S'";
                    break;

                case Status.Todos:
                default:
                    whereStatus = " AND (Status = 'R' OR Status = 'E')";
                    break;
            }
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Carrinho.ValeIngressoID, ValeIngressoTipoID, vi.ValidadeData, IsNull(vi.ValidadeDiasImpressao, 0) AS ValidadeDiasImpressao, vi.Nome, vi.ValorPagamento, Status,vi.Valor,vi.ValorTipo, vi.TrocaConveniencia,vi.TrocaEntrega,vi.TrocaIngresso ");
                stbSQL.Append("FROM Carrinho (NOLOCK) ");
                stbSQL.Append("INNER JOIN ValeIngressoTipo vi (NOLOCK) ON Carrinho.ValeIngressoTipoID = vi.IR_ValeIngressoTipoID ");
                stbSQL.Append("WHERE SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " AND ValeIngressoTipoID IS NOT NULL " + whereStatus + " ");
                IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString());

                while (dr.Read())
                {
                    oCarrinho = new Carrinho();
                    oCarrinho.ValeIngressoID = Convert.ToInt32(dr["ValeIngressoID"]);
                    oCarrinho.ValeIngressoTipoID = Convert.ToInt32(dr["ValeIngressoTipoID"]);
                    oCarrinho.ValeIngressoNome = dr["Nome"].ToString();
                    oCarrinho.ValorTipo = Convert.ToChar(dr["ValorTipo"].ToString());
                    oCarrinho.ValorTroca = Convert.ToDecimal(dr["Valor"]);
                    oCarrinho.TrocaConveniencia = dr["TrocaConveniencia"].ToString() == "T";
                    oCarrinho.TrocaEntrega = dr["TrocaEntrega"].ToString() == "T";
                    oCarrinho.TrocaIngresso = dr["TrocaIngresso"].ToString() == "T";
                    oCarrinho.PrecoValor = Convert.ToDecimal(dr["ValorPagamento"]);

                    if (Convert.ToInt32(dr["ValidadeDiasImpressao"]) != 0)
                        oCarrinho.ValidadeData = "Válido por " + Convert.ToInt32(dr["ValidadeDiasImpressao"]) + " dias após a impressão";
                    else
                        oCarrinho.ValidadeData = "Válido até: " + DateTime.ParseExact(dr["ValidadeData"].ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao).ToString("dd/MM/yyyy");

                    oCarrinho.Total = oCarrinho.PrecoValor;
                    oCarrinho.Status = dr["Status"].ToString();
                    oCarrinho.SessionID = sessionID;

                    this.Add(oCarrinho);
                }
                dr.Close();
                return this;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public bool taxaEntregaGratuita(int clienteID, string sessionID)
        {
            bool retorno = false;
            int itens = 0;

            string strSql = "SELECT COUNT(ID) FROM Carrinho (NOLOCK) WHERE (SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + ") AND (Status = 'R')";

            try
            {
                itens = (int)oDAL.Scalar(strSql, null);
                if (itens >= 4)
                    retorno = true;
                return retorno;
            }
            catch (Exception)
            {
                return retorno;
            }
        }

        public bool zerarCodigoExclusivoCarrinho(int id, string sessionID)
        {
            bool retorno = true;
            string strSql = "";
            try
            {
                strSql = "UPDATE Carrinho SET PrecoExclusivoCodigoID = '' WHERE SessionID = '" + sessionID + "' AND ClienteID = " + id;
                oDAL.Execute(strSql);
            }
            catch (Exception)
            {
                retorno = false;
            }
            finally
            {
                oDAL.ConnClose();
            }

            return retorno;
        }

        public bool AtualizaCodigoExclusivoItem(int carrinhoID, int precoExclusivoCodigoID)
        {
            bool retorno = true;
            string strSql = "UPDATE Carrinho SET PrecoExclusivoCodigoID = " + precoExclusivoCodigoID.ToString() + " WHERE ID = " + carrinhoID.ToString();

            try
            {
                oDAL.Scalar(strSql, null);
            }
            catch (Exception)
            {
                retorno = false;
            }
            finally
            {
                oDAL.ConnClose();
            }
            return retorno;
        }

        public Carrinho InserirLugarMarcado(EstruturaReservaInternet estrutura)
        {
            DAL oDAL = new DAL();

            Ingresso ingresso = new Ingresso();
            ingresso.SessionID.Valor = this[0].SessionID;
            ingresso.ClienteID.Valor = this[0].ClienteID;
            ingresso.ApresentacaoID.Valor = this[0].ApresentacaoID;
            ingresso.SetorID.Valor = this[0].SetorID;
            ingresso.EventoID.Valor = this[0].EventoID;
            ingresso.LugarID.Valor = this[0].LugarID;
            ingresso.Control.ID = this[0].IngressoID;
            ingresso.EmpresaID.Valor = this[0].EmpresaID;
            try
            {
                EstruturaPrecoReservaSite preco = new EstruturaPrecoReservaSite()
                {
                    ID = this[0].PrecoID,
                    Quantidade = 1,
                    Valor = this[0].PrecoValor,
                    PrecoNome = this[0].PrecoNome,
                    QuantidadeMapa = this[0].QuantidadeMapa,
                };

                //Bilheteria oBilheteria = new Bilheteria();
                new Bilheteria().ReservarInternetLugarMarcado(ref ingresso, preco, this[0].SerieID, estrutura);

                decimal auxiliarTaxaMaximaTotal = 0, valorAux = 0;

                //Carrega o total de taxa já reservado para essa empresa
                auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(ingresso.EmpresaID.Valor, this[0].ClienteID, this[0].SessionID);

                //Calcula a taxa máxima por empresa
                if (this[0].TaxaMaximaEmpresa > 0 && ingresso.TaxaProcessamentoValor == 0)
                {
                    valorAux = this[0].TaxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                    if (valorAux > 0)
                    {
                        if (valorAux < ingresso.TxConv)
                            ingresso.TxConv = valorAux;

                    }
                    else
                        ingresso.TxConv = 0;

                    auxiliarTaxaMaximaTotal += ingresso.TxConv;
                }


                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("@ClienteID", this[0].ClienteID));
                parametros.Add(new SqlParameter("@Codigo", this[0].Codigo));
                parametros.Add(new SqlParameter("@LugarID", ingresso.LugarID.Valor));
                parametros.Add(new SqlParameter("@IngressoID", ingresso.Control.ID));
                parametros.Add(new SqlParameter("@TipoLugar", this[0].TipoLugar));
                parametros.Add(new SqlParameter("@ApresentacaoID", ingresso.ApresentacaoID.Valor));
                parametros.Add(new SqlParameter("@SetorID", ingresso.SetorID.Valor));
                parametros.Add(new SqlParameter("@PrecoID", ingresso.PrecoID.Valor));
                parametros.Add(new SqlParameter("@LocalID", this[0].LocalID));
                parametros.Add(new SqlParameter("@Local", this[0].Local));
                parametros.Add(new SqlParameter("@EventoID", ingresso.EventoID.Valor));
                parametros.Add(new SqlParameter("@Evento", this[0].Evento));
                parametros.Add(new SqlParameter("@ApresentacaoDataHora", this[0].ApresentacaoDataHora.ToString("yyyyMMddHHmmss")));
                parametros.Add(new SqlParameter("@Setor", this[0].Setor));
                parametros.Add(new SqlParameter("@PrecoNome", this[0].PrecoNome));
                parametros.Add(new SqlParameter("@PrecoValor", this[0].PrecoValor));
                parametros.Add(new SqlParameter("@TimeStamp", DateTime.Now.ToString("yyyyMMddHHmmss")));
                parametros.Add(new SqlParameter("@TaxaConveniencia", ingresso.TxConv));
                parametros.Add(new SqlParameter("@SessionID", this[0].SessionID));
                parametros.Add(new SqlParameter("@TagOrigem", this[0].TagOrigem ?? string.Empty));
                parametros.Add(new SqlParameter("@Grupo", ingresso.Grupo.Valor));
                parametros.Add(new SqlParameter("@Classificacao", ingresso.Classificacao.Valor));
                parametros.Add(new SqlParameter("@CotaItemID", ingresso.CotaItemID));
                parametros.Add(new SqlParameter("@CotaItemIDAPS", ingresso.CotaItemIDAPS));
                parametros.Add(new SqlParameter("@IsSpecial", this[0].SpecialEvent));
                parametros.Add(new SqlParameter("@SerieID", this[0].SerieID));
                parametros.Add(new SqlParameter("@EmpresaID", this[0].EmpresaID));
                parametros.Add(new SqlParameter("@ValorTaxaProcessamento", ingresso.TaxaProcessamentoValor));

                this[0].ID = Convert.ToInt32(oDAL.Scalar(@"INSERT INTO Carrinho (ClienteID, Codigo, LugarID, IngressoID, TipoLugar, ApresentacaoID,
                                            SetorID, PrecoID, LocalID, Local, EventoID, Evento, ApresentacaoDataHora, Setor, PrecoNome,
                                            PrecoValor, TimeStamp, TaxaConveniencia, SessionID, TagOrigem, Grupo, Classificacao,
                                            CotaItemID, CotaItemIDAPS, IsSpecial, SerieID, EmpresaID, DonoID, DonoCPF, CodigoPromocional, ValorTaxaProcessamento) 
                                        VALUES (@ClienteID, @Codigo, @LugarID, @IngressoID, @TipoLugar, @ApresentacaoID,
                                                @SetorID, @PrecoID, @LocalID, @Local, @EventoID, @Evento, @ApresentacaoDataHora, @Setor, @PrecoNome,
                                                @PrecoValor, @TimeStamp, @TaxaConveniencia, @SessionID, @TagOrigem, @Grupo, @Classificacao, @CotaItemID,
                                                @CotaItemIDAPS, @IsSpecial, @SerieID, @EmpresaID, 0, '', '', @ValorTaxaProcessamento); SELECT SCOPE_IDENTITY();", parametros.ToArray()));


                if (this[0].ID == 0)
                    throw new Exception("Não foi possível inserir o item no carrinho");

                return new Carrinho()
                {
                    ID = this[0].ID,
                    Local = this[0].Local,
                    IngressoID = ingresso.Control.ID,
                    Codigo = this[0].Codigo,
                    TipoLugar = this[0].TipoLugar,
                    EventoID = this[0].EventoID,
                    Evento = this[0].Evento,
                    PrecoID = this[0].PrecoID,
                    PrecoNome = this[0].PrecoNome,
                    PrecoValor = this[0].PrecoValor,
                    TaxaConveniencia = ingresso.TxConv,
                    Setor = this[0].Setor,
                    SerieID = this[0].SerieID,
                    ApresentacaoID = this[0].ApresentacaoID,
                    ApresentacaoDataHora = this[0].ApresentacaoDataHora,
                    Precos = (this.PreencherPrecosPorReserva(this[0].ApresentacaoID, this[0].SetorID, this[0].SerieID)),
                    CotaItem = new CotaItem(ingresso.Control.ID).GetByID(ingresso.CotaItemIDAPS > 0 ? ingresso.CotaItemIDAPS : ingresso.CotaItemID),
                    LugarID = this[0].LugarID,
                    TaxaProcessamento = ingresso.TaxaProcessamentoValor,
                    Estado = this[0].Estado,
                };
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public CarrinhoLista InserirMesa(EstruturaReservaInternet estrutura)
        {
            this.oDAL = new DAL();
            try
            {
                List<Ingresso> lstIngresso = new List<Ingresso>();
                Ingresso oIngresso;
                foreach (Carrinho carrinho in this)
                {
                    oIngresso = new Ingresso();
                    oIngresso.SessionID.Valor = this[0].SessionID;
                    oIngresso.ClienteID.Valor = this[0].ClienteID;
                    oIngresso.ApresentacaoID.Valor = carrinho.ApresentacaoID;
                    oIngresso.SetorID.Valor = carrinho.SetorID;
                    oIngresso.EventoID.Valor = carrinho.EventoID;
                    oIngresso.LugarID.Valor = carrinho.LugarID;
                    oIngresso.Control.ID = carrinho.IngressoID;
                    oIngresso.Codigo.Valor = carrinho.Codigo;
                    oIngresso.EmpresaID.Valor = carrinho.EmpresaID;
                    lstIngresso.Add(oIngresso);
                }


                EstruturaPrecoReservaSite preco = new EstruturaPrecoReservaSite()
                {
                    ID = this[0].PrecoID,
                    Quantidade = 1,
                    Valor = this[0].PrecoValor,
                    PrecoNome = this[0].PrecoNome,
                    QuantidadeMapa = this[0].QuantidadeMapa,
                };

                //Bilheteria oBilheteria = new Bilheteria();
                new Bilheteria().ReservarInternetMesa(ref lstIngresso, preco, estrutura);

                List<SqlParameter> parametros = new List<SqlParameter>();
                CarrinhoLista lstCarrinho = new CarrinhoLista();

                decimal auxiliarTaxaMaximaTotal = 0, valorAux = 0;

                if (lstIngresso.Count > 0)
                    auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(lstIngresso[0].EmpresaID.Valor, this[0].ClienteID, this[0].SessionID);

                CotaItem cotaItem = new CotaItem().GetByID(lstIngresso[0].CotaItemIDAPS > 0 ? lstIngresso[0].CotaItemIDAPS : lstIngresso[0].CotaItemID);
                var PrecosPorReserva = this.PreencherPrecosPorReserva(this[0].ApresentacaoID, this[0].SetorID, 0);

                foreach (Ingresso ingresso in lstIngresso)
                {

                    //Calcula a taxa máxima por empresa
                    if (this[0].TaxaMaximaEmpresa > 0 && ingresso.TaxaProcessamentoValor == 0)
                    {
                        valorAux = this[0].TaxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                        if (valorAux > 0)
                        {
                            if (valorAux < ingresso.TxConv)
                                ingresso.TxConv = valorAux;

                        }
                        else
                            ingresso.TxConv = 0;

                        auxiliarTaxaMaximaTotal += ingresso.TxConv;
                    }


                    parametros.Clear();
                    parametros.Add(new SqlParameter("@ClienteID", this[0].ClienteID));
                    parametros.Add(new SqlParameter("@Codigo", ingresso.Codigo.Valor));
                    parametros.Add(new SqlParameter("@LugarID", ingresso.LugarID.Valor));
                    parametros.Add(new SqlParameter("@IngressoID", ingresso.Control.ID));
                    parametros.Add(new SqlParameter("@TipoLugar", this[0].TipoLugar));
                    parametros.Add(new SqlParameter("@ApresentacaoID", this[0].ApresentacaoID));
                    parametros.Add(new SqlParameter("@SetorID", this[0].SetorID));
                    parametros.Add(new SqlParameter("@PrecoID", preco.ID));
                    parametros.Add(new SqlParameter("@LocalID", this[0].LocalID));
                    parametros.Add(new SqlParameter("@Local", this[0].Local));
                    parametros.Add(new SqlParameter("@EventoID", this[0].EventoID));
                    parametros.Add(new SqlParameter("@Evento", this[0].Evento));
                    parametros.Add(new SqlParameter("@ApresentacaoDataHora", this[0].ApresentacaoDataHora.ToString("yyyyMMddHHmmss")));
                    parametros.Add(new SqlParameter("@Setor", this[0].Setor));
                    parametros.Add(new SqlParameter("@PrecoNome", preco.PrecoNome));
                    parametros.Add(new SqlParameter("@PrecoValor", preco.Valor));
                    parametros.Add(new SqlParameter("@TimeStamp", DateTime.Now.ToString("yyyyMMddHHmmss")));
                    parametros.Add(new SqlParameter("@TaxaConveniencia", ingresso.TxConv));
                    parametros.Add(new SqlParameter("@SessionID", this[0].SessionID));
                    parametros.Add(new SqlParameter("@TagOrigem", this[0].TagOrigem ?? string.Empty));
                    parametros.Add(new SqlParameter("@Grupo", ingresso.Grupo.Valor));
                    parametros.Add(new SqlParameter("@Classificacao", ingresso.Classificacao.Valor));
                    parametros.Add(new SqlParameter("@CotaItemID", ingresso.CotaItemID));
                    parametros.Add(new SqlParameter("@CotaItemIDAPS", ingresso.CotaItemIDAPS));
                    parametros.Add(new SqlParameter("@IsSpecial", this[0].SpecialEvent));
                    parametros.Add(new SqlParameter("@EmpresaID", this[0].EmpresaID));
                    parametros.Add(new SqlParameter("@ValorTaxaProcessamento", ingresso.TaxaProcessamentoValor));

                    if (cotaItem != null)
                    {
                        cotaItem = (CotaItem)cotaItem.Clone();
                        cotaItem.IngressoID = ingresso.Control.ID;
                    }

                    lstCarrinho.Add(new Carrinho()
                    {
                        ID = Convert.ToInt32(oDAL.Scalar(
                            @"INSERT INTO Carrinho (ClienteID, Codigo, LugarID, IngressoID, TipoLugar, ApresentacaoID,
                                                SetorID, PrecoID, LocalID, Local, EventoID, Evento, ApresentacaoDataHora, Setor, PrecoNome,
                                                PrecoValor, TimeStamp, TaxaConveniencia, SessionID, TagOrigem, Grupo, Classificacao,
                                                CotaItemID, CotaItemIDAPS, IsSpecial,EmpresaID, SerieID, ValorTaxaProcessamento) 
                                        VALUES (@ClienteID, @Codigo, @LugarID, @IngressoID, @TipoLugar, @ApresentacaoID,
                                                @SetorID, @PrecoID, @LocalID, @Local, @EventoID, @Evento, @ApresentacaoDataHora, @Setor, @PrecoNome,
                                                @PrecoValor, @TimeStamp, @TaxaConveniencia, @SessionID, @TagOrigem, @Grupo, @Classificacao, @CotaItemID,
                                                @CotaItemIDAPS, @IsSpecial,@EmpresaID, 0, @ValorTaxaProcessamento); SELECT SCOPE_IDENTITY();", parametros.ToArray())),

                        IngressoID = ingresso.Control.ID,
                        Codigo = ingresso.Codigo.Valor,
                        TipoLugar = this[0].TipoLugar,
                        EventoID = this[0].EventoID,
                        Evento = this[0].Evento,
                        Local = this[0].Local,
                        PrecoID = preco.ID,
                        PrecoNome = preco.PrecoNome,
                        PrecoValor = preco.Valor,
                        TaxaConveniencia = ingresso.TxConv,
                        Setor = this[0].Setor,
                        LugarID = ingresso.LugarID.Valor,
                        ApresentacaoID = this[0].ApresentacaoID,
                        ApresentacaoDataHora = this[0].ApresentacaoDataHora,
                        CotaItem = cotaItem,
                        Precos = PrecosPorReserva,
                        TaxaProcessamento = ingresso.TaxaProcessamentoValor,
                        Estado = this[0].Estado,
                    });

                    valorAux = 0;
                }
                return lstCarrinho;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public CarrinhoLista InserirItensNovo(EstruturaReservaInternet estrutura, bool isEntradaFranca = false)
        {
            oDAL = new DAL();
            var lstCarrinho = new CarrinhoLista();

            try
            {
                var bilheteria = new Bilheteria();
                var cinema = Precos.Any(c => !string.IsNullOrEmpty(c.CodigoProgramacao));

                if (cinema)
                {
                    var vagas = Service.getVagasProgramacao(SincronizarCinemas.MontarAuth(), new Filtros.GetVagasProg() { IDProg = Precos.FirstOrDefault().CodigoProgramacao });

                    if (vagas.Disponivel == 0 || vagas.Disponivel < Precos.Sum(c => c.Quantidade))
                        throw new Exception("A programação selecionada não possui mais assentos disponíveis, por favor, selecione outro horário.");
                }

                var infoReserva = bilheteria.ReservarInternet(this[0].SessionID, this[0].ClienteID, this[0].ApresentacaoID, this[0].SetorID, Precos,
                        this[0].EventoID, this[0].SerieID, estrutura, isEntradaFranca);

                if (infoReserva.Count == 0)
                    throw new Exception("Ingresso selecionado não está mais disponível. Tente novamente mais tarde.");

                var parametros = new List<SqlParameter>();

                decimal auxiliarTaxaMaximaTotal = 0;

                if (infoReserva.Count > 0 && infoReserva.Sum(c => c.TaxaProcessamentoValor) == 0)
                    auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(this[0].EmpresaID, this[0].ClienteID, this[0].SessionID);

                //Depois da reserva adiciona os ingressos ao carrinho
                foreach (var ingresso in infoReserva)
                {
                    var preco = Precos.FirstOrDefault(c => c.ID == ingresso.PrecoID.Valor);

                    if (preco == null || preco.ID == 0)
                        throw new Exception("O Preço selecionado não foi encontrado na base de dados.");

                    //Calcula a taxa m·xima por empresa
                    if (this[0].TaxaMaximaEmpresa > 0 && ingresso.TaxaProcessamentoValor == 0)
                    {
                        var valorAux = this[0].TaxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                        if (valorAux > 0)
                        {
                            if (valorAux < ingresso.TxConv)
                                ingresso.TxConv = valorAux;
                        }
                        else
                            ingresso.TxConv = 0;

                        auxiliarTaxaMaximaTotal += ingresso.TxConv;
                    }

                    // Insere no carrinho (BD SiteIR) os itens que realmente foram reservados.
                    parametros.Clear();
                    parametros.Add(new SqlParameter("@ClienteID", this[0].ClienteID));
                    parametros.Add(new SqlParameter("@Codigo", ingresso.Codigo.Valor));
                    parametros.Add(new SqlParameter("@LugarID", ingresso.LugarID.Valor));
                    parametros.Add(new SqlParameter("@IngressoID", ingresso.Control.ID));
                    parametros.Add(new SqlParameter("@TipoLugar", this[0].TipoLugar));
                    parametros.Add(new SqlParameter("@ApresentacaoID", this[0].ApresentacaoID));
                    parametros.Add(new SqlParameter("@SetorID", this[0].SetorID));
                    parametros.Add(new SqlParameter("@PrecoID", preco.ID));
                    parametros.Add(new SqlParameter("@GerenciamentoIngressosID", preco.GerenciamentoIngressosID));
                    parametros.Add(new SqlParameter("@LocalID", this[0].LocalID));
                    parametros.Add(new SqlParameter("@Local", this[0].Local));
                    parametros.Add(new SqlParameter("@EventoID", this[0].EventoID));
                    parametros.Add(new SqlParameter("@Evento", this[0].Evento));
                    parametros.Add(new SqlParameter("@ApresentacaoDataHora", this[0].ApresentacaoDataHora.ToString("yyyyMMddHHmmss")));
                    parametros.Add(new SqlParameter("@Setor", this[0].Setor));
                    parametros.Add(new SqlParameter("@PrecoNome", preco.PrecoNome));
                    parametros.Add(new SqlParameter("@PrecoValor", preco.Valor));
                    parametros.Add(new SqlParameter("@TimeStamp", DateTime.Now.ToString("yyyyMMddHHmmss")));
                    parametros.Add(new SqlParameter("@TaxaConveniencia", ingresso.TxConv));
                    parametros.Add(new SqlParameter("@PacoteGrupo", ingresso.PacoteGrupo.Valor));
                    parametros.Add(new SqlParameter("@SessionID", this[0].SessionID));
                    parametros.Add(new SqlParameter("@TagOrigem", this[0].TagOrigem ?? string.Empty));
                    parametros.Add(new SqlParameter("@Grupo", ingresso.Grupo.Valor));
                    parametros.Add(new SqlParameter("@Classificacao", ingresso.Classificacao.Valor));
                    parametros.Add(new SqlParameter("@CotaItemID", ingresso.CotaItemID));
                    parametros.Add(new SqlParameter("@CotaItemIDAPS", ingresso.CotaItemIDAPS));
                    parametros.Add(new SqlParameter("@IsSpecial", this[0].SpecialEvent));
                    parametros.Add(new SqlParameter("@SerieID", this[0].SerieID));
                    parametros.Add(new SqlParameter("@EmpresaID", this[0].EmpresaID));
                    parametros.Add(new SqlParameter("@ValorTaxaProcessamento", ingresso.TaxaProcessamentoValor));

                    lstCarrinho.Add(new Carrinho()
                    {
                        ID = Convert.ToInt32(oDAL.Scalar(
                            @"INSERT INTO Carrinho (ClienteID, Codigo, LugarID, IngressoID, TipoLugar, ApresentacaoID,
                                            SetorID, PrecoID, LocalID, Local, EventoID, Evento, ApresentacaoDataHora, Setor, PrecoNome,
                                            PrecoValor, TimeStamp, TaxaConveniencia, SessionID, TagOrigem, Grupo, Classificacao,
                                            CotaItemID, CotaItemIDAPS, IsSpecial, SerieID, EmpresaID, ValorTaxaProcessamento, PacoteGrupo, GerenciamentoIngressosID) 
                                    VALUES (@ClienteID, @Codigo, @LugarID, @IngressoID, @TipoLugar, @ApresentacaoID,
                                            @SetorID, @PrecoID, @LocalID, @Local, @EventoID, @Evento, @ApresentacaoDataHora, @Setor, @PrecoNome,
                                            @PrecoValor, @TimeStamp, @TaxaConveniencia, @SessionID, @TagOrigem, @Grupo, @Classificacao, @CotaItemID,
                                            @CotaItemIDAPS, @IsSpecial, @SerieID, @EmpresaID, @ValorTaxaProcessamento, @PacoteGrupo, @GerenciamentoIngressosID); SELECT SCOPE_IDENTITY();", parametros.ToArray())),
                        EventoID = this[0].EventoID,
                        Evento = this[0].Evento,
                        IngressoID = ingresso.Control.ID,
                        Codigo = ingresso.Codigo.Valor,
                        Local = this[0].Local,
                        PrecoID = preco.ID,
                        GerenciamentoIngressosID = preco.GerenciamentoIngressosID,
                        PrecoNome = preco.PrecoNome,
                        PrecoValor = preco.Valor,
                        TaxaConveniencia = ingresso.TxConv,
                        LugarID = ingresso.LugarID.Valor,
                        Setor = this[0].Setor,
                        ApresentacaoID = this[0].ApresentacaoID,
                        ApresentacaoDataHora = this[0].ApresentacaoDataHora,
                        Precos = (this.PreencherPrecosPorReserva(this[0].ApresentacaoID, this[0].SetorID, this[0].SerieID)),
                        TipoLugar = this[0].TipoLugar,
                        CotaItem = new CotaItem(ingresso.Control.ID).GetByID(ingresso.CotaItemIDAPS > 0 ? ingresso.CotaItemIDAPS : ingresso.CotaItemID),
                        TaxaProcessamento = ingresso.TaxaProcessamentoValor,
                        Estado = this[0].Estado,
                    });
                }

                return lstCarrinho;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public List<Carrinho> InserirAssinaturasNovo()
        {
            List<Carrinho> lstRetorno = new List<Carrinho>();
            try
            {
                Bilheteria oBilheteria = new Bilheteria();
                List<SqlParameter> parametros = new List<SqlParameter>();
                //Como só tem 1 item, pega pos 0
                this.oCarrinho = this[0];

                EstruturaReservaInternet estruturaReservaInternet = MontarEstruturaReserva();

                if (oCarrinho.Quantidade != oBilheteria.GetPacotesQPodeReservar(oCarrinho.PacoteID, oCarrinho.Quantidade, this[0].SessionID, this[0].ClienteID, estruturaReservaInternet))
                    throw new Exception("Não foi possivel reservar esta assinatura, limite excedido");

                List<Ingresso> retornoReserva = oBilheteria.ReservarAssinaturaInternet(oCarrinho.PacoteID, oCarrinho.LugarID, this[0].SessionID, this[0].ClienteID, estruturaReservaInternet);
                if (retornoReserva == null || retornoReserva.Count == 0)
                    throw new Exception("Não foi possível reservar a assinatura selecionada.");

                Reserva oReserva = new Reserva();

                Preco oPreco;

                foreach (Ingresso ingresso in retornoReserva)
                {
                    oPreco = new Preco().CarregarEstruturaPorPreco(ingresso.PrecoID.Valor, oCarrinho.PacoteID);

                    parametros.Clear();
                    parametros.Add(new SqlParameter("@ClienteID", this[0].ClienteID));
                    parametros.Add(new SqlParameter("@Codigo", ingresso.Codigo.Valor));
                    parametros.Add(new SqlParameter("@LugarID", ingresso.LugarID.Valor));
                    parametros.Add(new SqlParameter("@IngressoID", ingresso.Control.ID));
                    parametros.Add(new SqlParameter("@TipoLugar", IRLib.Setor.Cadeira));
                    parametros.Add(new SqlParameter("@ApresentacaoID", oPreco.Apresentacao.Id));
                    parametros.Add(new SqlParameter("@SetorID", oPreco.Setor.Id));
                    parametros.Add(new SqlParameter("@PrecoID", ingresso.PrecoID.Valor));
                    parametros.Add(new SqlParameter("@LocalID", oPreco.Local.ID));
                    parametros.Add(new SqlParameter("@Local", oPreco.Local.Nome));
                    parametros.Add(new SqlParameter("@EventoID", ingresso.EventoID.Valor));
                    parametros.Add(new SqlParameter("@Evento", oPreco.Evento.Nome));
                    parametros.Add(new SqlParameter("@ApresentacaoDataHora", oPreco.Apresentacao.Horario.ToString("yyyyMMddHHmmss")));
                    parametros.Add(new SqlParameter("@Setor", oPreco.Setor.Nome));
                    parametros.Add(new SqlParameter("@PrecoNome", oPreco.Nome));
                    parametros.Add(new SqlParameter("@PrecoValor", oPreco.Valor));
                    parametros.Add(new SqlParameter("@TimeStamp", DateTime.Now.ToString("yyyyMMddHHmmss")));
                    parametros.Add(new SqlParameter("@TaxaConveniencia", ingresso.TxConv));
                    parametros.Add(new SqlParameter("@SessionID", this[0].SessionID));
                    parametros.Add(new SqlParameter("@TagOrigem", oCarrinho.TagOrigem ?? string.Empty));
                    parametros.Add(new SqlParameter("@PacoteGrupo", ingresso.PacoteGrupo.Valor));
                    parametros.Add(new SqlParameter("@PacoteNome", oPreco.Pacote));
                    parametros.Add(new SqlParameter("@CotaItemID", ingresso.CotaItemID));
                    parametros.Add(new SqlParameter("@CotaItemIDAPS", ingresso.CotaItemIDAPS));
                    parametros.Add(new SqlParameter("@EmpresaID", oCarrinho.EmpresaID));
                    parametros.Add(new SqlParameter("@ValorTaxaProcessamento", ingresso.TaxaProcessamentoValor));
                    parametros.Add(new SqlParameter("@PacoteID", oCarrinho.PacoteID));

                    oDAL.Execute(@"INSERT INTO Carrinho
                                (ClienteID, Codigo, LugarID, IngressoID, TipoLugar, ApresentacaoID, SetorID, PrecoID, LocalID, Local,
                                EventoID, Evento, ApresentacaoDataHora, Setor, PrecoNome, PrecoValor, TimeStamp, TaxaConveniencia,
                                SessionID, TagOrigem, PacoteGrupo, PacoteNome, CotaItemID, CotaItemIDAPS,EmpresaID, SerieID, ValorTaxaProcessamento, PacoteID)
                                VALUES (
                                @ClienteID, @Codigo, 0, @IngressoID, @TipoLugar, @ApresentacaoID, @SetorID, @PrecoID, @LocalID, @Local, 
                                @EventoID, @Evento, @ApresentacaoDataHora, @Setor, @PrecoNome, @PrecoValor, @TimeStamp, @TaxaConveniencia,
                                @SessionID, @TagOrigem, @PacoteGrupo, @PacoteNome, @CotaItemID, @CotaItemIDAPS,@EmpresaID, 0, @ValorTaxaProcessamento, @PacoteID);
                                ", parametros.ToArray());

                    lstRetorno.Add(new Carrinho()
                    {
                        IngressoID = ingresso.Control.ID,
                        Codigo = ingresso.Codigo.Valor,
                        PrecoNome = oPreco.Nome,
                        PrecoValor = oPreco.Valor,
                        TaxaConveniencia = ingresso.TxConv,
                        PacoteGrupo = ingresso.PacoteGrupo.Valor,
                        PacoteID = oCarrinho.PacoteID,
                        PacoteNome = oPreco.Pacote,
                        ApresentacaoDataHora = oPreco.Apresentacao.Horario,
                        Precos = null,
                        CotaItem = new CotaItem(ingresso.Control.ID).GetByID(ingresso.CotaItemIDAPS > 0 ? ingresso.CotaItemIDAPS : ingresso.CotaItemID),
                    });
                }

                return lstRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public List<Carrinho> InserirPacotesNovo(EstruturaReservaInternet estruturaReservaInternet = null)
        {
            var lstRetorno = new List<Carrinho>();

            try
            {
                var oBilheteria = new Bilheteria();
                var parametros = new List<SqlParameter>();

                if (estruturaReservaInternet == null)
                {
                    estruturaReservaInternet = MontarEstruturaReserva();
                }

                foreach (var carrinho in this)
                {

                    if (carrinho.Quantidade != oBilheteria.GetPacotesQPodeReservar(carrinho.PacoteID, carrinho.Quantidade, this[0].SessionID, this[0].ClienteID, estruturaReservaInternet))
                        throw new Exception("Não foi possivel reservar este pacote, limite excedido");

                    var retornoReserva = oBilheteria.ReservarPacoteInternet(carrinho.PacoteID, carrinho.Quantidade, this[0].SessionID, this[0].ClienteID, estruturaReservaInternet);

                    if (retornoReserva == null)
                        throw new Exception("Não foi possível reservar os pacotes selecionados.");


                    foreach (var ingresso in retornoReserva)
                    {
                        var oPreco = new Preco().CarregarEstruturaPorPreco(ingresso.PrecoID.Valor, carrinho.PacoteID);

                        parametros.Clear();
                        parametros.Add(new SqlParameter("@ClienteID", this[0].ClienteID));
                        parametros.Add(new SqlParameter("@Codigo", ingresso.Codigo.Valor));
                        parametros.Add(new SqlParameter("@IngressoID", ingresso.Control.ID));
                        parametros.Add(new SqlParameter("@TipoLugar", "P"));
                        parametros.Add(new SqlParameter("@ApresentacaoID", oPreco.Apresentacao.Id));
                        parametros.Add(new SqlParameter("@SetorID", oPreco.Setor.Id));
                        parametros.Add(new SqlParameter("@PrecoID", ingresso.PrecoID.Valor));
                        parametros.Add(new SqlParameter("@LocalID", oPreco.Local.ID));
                        parametros.Add(new SqlParameter("@Local", oPreco.Local.Nome));
                        parametros.Add(new SqlParameter("@EventoID", ingresso.EventoID.Valor));
                        parametros.Add(new SqlParameter("@Evento", oPreco.Evento.Nome));
                        parametros.Add(new SqlParameter("@ApresentacaoDataHora", oPreco.Apresentacao.Horario.ToString("yyyyMMddHHmmss")));
                        parametros.Add(new SqlParameter("@Setor", oPreco.Setor.Nome));
                        parametros.Add(new SqlParameter("@PrecoNome", oPreco.Nome));
                        parametros.Add(new SqlParameter("@PrecoValor", oPreco.Valor));
                        parametros.Add(new SqlParameter("@TimeStamp", DateTime.Now.ToString("yyyyMMddHHmmss")));
                        parametros.Add(new SqlParameter("@TaxaConveniencia", ingresso.TxConv));
                        parametros.Add(new SqlParameter("@SessionID", this[0].SessionID));
                        parametros.Add(new SqlParameter("@TagOrigem", carrinho.TagOrigem ?? string.Empty));
                        parametros.Add(new SqlParameter("@PacoteGrupo", ingresso.PacoteGrupo.Valor));
                        parametros.Add(new SqlParameter("@PacoteNome", oPreco.Pacote));
                        parametros.Add(new SqlParameter("@CotaItemID", ingresso.CotaItemID));
                        parametros.Add(new SqlParameter("@CotaItemIDAPS", ingresso.CotaItemIDAPS));
                        parametros.Add(new SqlParameter("@EmpresaID", carrinho.EmpresaID));
                        parametros.Add(new SqlParameter("@ValorTaxaProcessamento", ingresso.TaxaProcessamentoValor));
                        parametros.Add(new SqlParameter("@PacoteID", carrinho.PacoteID));

                        oDAL.Execute(@"INSERT INTO Carrinho
                                (ClienteID, Codigo, LugarID, IngressoID, TipoLugar, ApresentacaoID, SetorID, PrecoID, LocalID, Local,
                                EventoID, Evento, ApresentacaoDataHora, Setor, PrecoNome, PrecoValor, TimeStamp, TaxaConveniencia,
                                SessionID, TagOrigem, PacoteGrupo, PacoteNome, CotaItemID, CotaItemIDAPS,EmpresaID, SerieID, ValorTaxaProcessamento, PacoteID)
                                VALUES (
                                @ClienteID, @Codigo, 0, @IngressoID, @TipoLugar, @ApresentacaoID, @SetorID, @PrecoID, @LocalID, @Local, 
                                @EventoID, @Evento, @ApresentacaoDataHora, @Setor, @PrecoNome, @PrecoValor, @TimeStamp, @TaxaConveniencia,
                                @SessionID, @TagOrigem, @PacoteGrupo, @PacoteNome, @CotaItemID, @CotaItemIDAPS,@EmpresaID, 0, @ValorTaxaProcessamento, @PacoteID);
                                ", parametros.ToArray());

                        lstRetorno.Add(new Carrinho()
                        {
                            IngressoID = ingresso.Control.ID,
                            Codigo = ingresso.Codigo.Valor,
                            PrecoNome = oPreco.Nome,
                            PrecoValor = oPreco.Valor,
                            TaxaConveniencia = ingresso.TxConv,
                            PacoteGrupo = ingresso.PacoteGrupo.Valor,
                            PacoteID = carrinho.PacoteID,
                            PacoteNome = oPreco.Pacote,
                            ApresentacaoDataHora = oPreco.Apresentacao.Horario,
                            Precos = null,
                            CotaItem = new CotaItem(ingresso.Control.ID).GetByID(ingresso.CotaItemIDAPS > 0 ? ingresso.CotaItemIDAPS : ingresso.CotaItemID),
                            TaxaProcessamento = ingresso.TaxaProcessamentoValor,
                        });
                    }
                }
                return lstRetorno.OrderBy(c => c.PacoteID).ThenBy(c => c.PacoteGrupo).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        // <summary>
        //Metodo para validar as cotas no momento de fechar o pedido (FormaDePagamento.aspx)
        //</summary>
        //<param name="clienteID"></param>
        //<param name="BIN"></param>
        //<param name="formaPagamentoID"></param>
        //<param name="msgCota"></param>
        public string[] ValidarCotas(int clienteID, int BIN, int formaPagamentoID, bool somenteVIR, bool somenteCortesias)
        {
            try
            {
                string[] msgCota = new string[2];
                oCotaItem = new IRLib.CotaItem();
                CotaItemControle oCotaItemControle = new CotaItemControle();
                List<EstruturaCotaItemReserva> lstItens = new List<EstruturaCotaItemReserva>();

                EstruturaCotaItemReserva item = new EstruturaCotaItemReserva();

                EstruturaCotasInfo cotasInfo = new EstruturaCotasInfo();
                List<int> lstItemAux = new List<int>();
                List<int> lstApresentacaoAux = new List<int>();

                int qtdAP = 0;
                int qtdAPS = 0;
                int qtdCotaItemAP = 0;
                int qtdCotaItemAPS = 0;

                foreach (Carrinho oCarrinho in this.OrderBy(c => c.ApresentacaoID).OrderBy(c => c.Setor).OrderBy(c => c.CotaItemID).OrderBy(c => c.CotaItemIDAPS).Where(c => c.isCota.Length > 0))
                {

                    cotasInfo.ApresentacaoID = oCarrinho.ApresentacaoID;
                    cotasInfo.SetorID = oCarrinho.SetorID;
                    cotasInfo.CotaItemID = oCarrinho.CotaItemID;
                    cotasInfo.CotaItemID_APS = oCarrinho.CotaItemIDAPS;

                    qtdAP = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.isCota.Length > 0).Count();
                    qtdCotaItemAP = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemID == oCarrinho.CotaItemID && c.CotaItemID > 0).Count();

                    qtdAPS = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.SetorID == oCarrinho.SetorID && c.isCota.Length > 0).Count();
                    qtdCotaItemAPS = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.SetorID == oCarrinho.SetorID && c.CotaItemIDAPS == oCarrinho.CotaItemIDAPS && c.CotaItemIDAPS > 0).Count();

                    #region Encontra Itens da Apresentacao
                    if (lstItens.Where(c => c.ID == cotasInfo.CotaItemID && c.ApresentacaoID == cotasInfo.ApresentacaoID).Count() == 0 && cotasInfo.CotaItemID > 0)
                    {
                        item = oCotaItem.getCotaItemPorID(cotasInfo.CotaItemID, oCarrinho.ApresentacaoID, oCarrinho.SetorID);
                        cotasInfo.ApresentacaoSetorID = item.ApresentacaoSetorID;
                        lstItens.Add(item);

                        cotasInfo.ParceiroID = item.ParceiroID;
                        cotasInfo.ValidaBin = item.ValidaBin;
                        cotasInfo.MaximaApresentacao = item.QuantidadeApresentacao;
                        cotasInfo.MaximaPorClienteApresentacao = item.QuantidadePorClienteApresentacao;
                        cotasInfo.MaximaCotaItem = item.Quantidade;
                        cotasInfo.MaximaPorClienteCotaItem = item.QuantidadePorCliente;
                        cotasInfo.MaximaCodigo = item.QuantidadePorCodigo;
                    }
                    else if (cotasInfo.CotaItemID > 0)
                    {
                        EstruturaCotaItemReserva itemEncontrado = lstItens.Where(c => c.ID == cotasInfo.CotaItemID && c.isApresentacao).FirstOrDefault();
                        cotasInfo.ApresentacaoSetorID = itemEncontrado.ApresentacaoSetorID;
                        cotasInfo.ParceiroID = itemEncontrado.ParceiroID;
                        cotasInfo.ValidaBin = itemEncontrado.ValidaBin;
                        cotasInfo.MaximaApresentacao = itemEncontrado.QuantidadeApresentacao;
                        cotasInfo.MaximaPorClienteApresentacao = itemEncontrado.QuantidadePorClienteApresentacao;
                        cotasInfo.MaximaCotaItem = itemEncontrado.Quantidade;
                        cotasInfo.MaximaPorClienteCotaItem = itemEncontrado.QuantidadePorCliente;
                        cotasInfo.MaximaCodigo = item.QuantidadePorCodigo;
                    }
                    #endregion

                    #region Encontra os Itens da ApresentacaoSetor
                    if (lstItens.Where(c => c.ID == cotasInfo.CotaItemID_APS && c.ApresentacaoID == cotasInfo.ApresentacaoID && c.SetorID == cotasInfo.SetorID).Count() == 0 && cotasInfo.CotaItemID_APS > 0)
                    {
                        item = oCotaItem.getCotaItemPorID(cotasInfo.CotaItemID_APS, cotasInfo.ApresentacaoID, cotasInfo.SetorID, cotasInfo.ApresentacaoSetorID);
                        cotasInfo.ApresentacaoSetorID = item.ApresentacaoSetorID;
                        lstItens.Add(item);

                        cotasInfo.ParceiroID = item.ParceiroID;
                        cotasInfo.ValidaBin = item.ValidaBin;
                        cotasInfo.MaximaCotaItemAPS = item.Quantidade;
                        cotasInfo.MaximaPorClienteCotaItemAPS = item.QuantidadePorCliente;
                        cotasInfo.MaximaApresentacaoSetor = item.QuantidadeApresentacaoSetor;
                        cotasInfo.MaximaPorClienteApresentacaoSetor = item.QuantidadePorClienteApresentacaoSetor;
                    }
                    else if (cotasInfo.CotaItemID_APS > 0)
                    {
                        EstruturaCotaItemReserva itemEncontrado = lstItens.Where(c => c.ID == cotasInfo.CotaItemID_APS && !c.isApresentacao).FirstOrDefault();

                        cotasInfo.ApresentacaoSetorID = itemEncontrado.ApresentacaoSetorID;
                        cotasInfo.ParceiroID = itemEncontrado.ParceiroID;
                        cotasInfo.ValidaBin = itemEncontrado.ValidaBin;
                        cotasInfo.MaximaCotaItemAPS = itemEncontrado.Quantidade;
                        cotasInfo.MaximaPorClienteCotaItemAPS = itemEncontrado.QuantidadePorCliente;
                        cotasInfo.MaximaApresentacaoSetor = itemEncontrado.QuantidadeApresentacaoSetor;
                        cotasInfo.MaximaPorClienteApresentacaoSetor = itemEncontrado.QuantidadePorClienteApresentacaoSetor;
                    }
                    #endregion

                    int[] qtds = oCotaItemControle.getQuantidadeNovo(cotasInfo.CotaItemID, cotasInfo.CotaItemID_APS, cotasInfo.ApresentacaoID, cotasInfo.ApresentacaoSetorID);

                    cotasInfo.QuantidadeApresentacao = qtds[0];
                    cotasInfo.QuantidadeApresentacaoSetor = qtds[1];
                    cotasInfo.QuantidadeCota = qtds[2];
                    cotasInfo.QuantidadeCotaAPS = qtds[3];

                    string retorno = cotasInfo.ValidaQuantidadeComMSG(qtdAP, qtdAPS, qtdCotaItemAP, qtdCotaItemAPS);
                    if (retorno.Length > 0)
                    {
                        msgCota[0] = "0";
                        msgCota[1] = retorno;
                        //msgCota[1] = "O preço " + oCarrinho.PrecoNome + "não pode mais ser vendido por excedeu o limite de venda, por favor escolha outro preço.";
                    }

                    if (!string.IsNullOrEmpty(msgCota[1]))
                        return msgCota;

                    msgCota = oCotaItem.ValidarCotaInformacoesNovo(cotasInfo, oCarrinho.PrecoNome, BIN, formaPagamentoID, somenteVIR, somenteCortesias);

                    if (!string.IsNullOrEmpty(msgCota[1]))
                        return msgCota;
                }
                return msgCota;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string VerificarComprasMesmaApresentacao(int clienteID)
        {
            try
            {
                if (!Convert.ToBoolean(ConfigurationManager.AppSettings["BloqueioMultiplasComprasApresentacao"]))
                    return string.Empty;

                VendaBilheteria oVendaBilheteria = new VendaBilheteria();
                DataTable dttBulk = new DataTable("bulk");
                dttBulk.Columns.Add("ID");
                DataRow dtr;

                foreach (int apresentacaoID in this.Select(c => c.ApresentacaoID).Where(c => c > 0).Distinct().ToList<int>())
                {
                    dtr = dttBulk.NewRow();
                    dtr["ID"] = apresentacaoID;
                    dttBulk.Rows.Add(dtr);
                }

                List<int> ids = oVendaBilheteria.VerificarComprasMesmaApresentacao(dttBulk, clienteID);

                if (ids.Count == 0)
                    return string.Empty;

                StringBuilder stb = new StringBuilder();

                stb.Append(new ConfigGerenciador().getMensagemApresentacoes());

                //if(stb.ToString().EndsWith("\n") || 

                for (int i = 0; i < ids.Count; i++)
                    stb.Append(string.Format("Evento: {0} - Horario: {1} ",
                        this.Where(c => c.ApresentacaoID == ids[i]).Select(c => c.Evento).FirstOrDefault().ToString(),
                        this.Where(c => c.ApresentacaoID == ids[i]).Select(c => c.ApresentacaoDataHora).FirstOrDefault().ToString()));

                stb.Append(". Por favor, dirija-se ao Ponto de Venda mais próximo para efetuar a compra.");

                return stb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<EstruturaCotasValidacao> CarregarCotas(string BIN)
        //{
        //    try
        //    {
        //        oCotaItem = new CotaItem();
        //        //EstruturaCotaItemReserva item = new EstruturaCotaItemReserva();

        //        int cotaItemIDaux = 0;

        //        List<int> lstItemaux = new List<int>();
        //        List<int> lstApresentacaoAux = new List<int>();
        //        EstruturaPrecoReservaSite preco;
        //        Ingresso oIngresso = new Ingresso();

        //        List<EstruturaCotasValidacao> listaValidacao = new List<EstruturaCotasValidacao>();
        //        EstruturaCotasValidacao item = new EstruturaCotasValidacao();

        //        StringBuilder stbHidden;
        //        foreach (Carrinho oCarrinho in this.OrderBy(c => c.ApresentacaoID).OrderBy(c => c.SetorID).OrderBy(c => c.CotaItemID).Where(c => c.CotaItemID != 0))
        //        {
        //            bool mudouItem = lstItemaux.Where(p => p == oCarrinho.CotaItemID).Count() == 0 || lstApresentacaoAux.Where(a => a == oCarrinho.ApresentacaoID).Count() == 0;
        //            if (mudouItem)
        //            {
        //                //Informações do Preço para que possa ser validada cota
        //                preco = new EstruturaPrecoReservaSite();
        //                preco.ID = oCarrinho.PrecoID;
        //                preco.Quantidade = this.Where(z => z.ApresentacaoID == oCarrinho.ApresentacaoID).Count();
        //                preco.CotaItemID = oCarrinho.CotaItemID;

        //                //Lista utilizada somente para validar se a apresentacao ja foi validada
        //                lstItemaux.Add(oCarrinho.CotaItemID);
        //                lstApresentacaoAux.Add(oCarrinho.ApresentacaoID);


        //                //Busca a cotaItem apartir do ID
        //                if (cotaItemIDaux != oCarrinho.CotaItemID)
        //                {
        //                    //Verfica as cotas por Preco
        //                    item = oCotaItem.getCotaItemSimples(oCarrinho.CotaItemID);
        //                    cotaItemIDaux = oCarrinho.CotaItemID;
        //                }
        //                item.PrecoNome = oCarrinho.PrecoNome;

        //                stbHidden = new StringBuilder();
        //                stbHidden.Append(oCarrinho.PrecoID);
        //                stbHidden.Append(",");
        //                stbHidden.Append(oCarrinho.PrecoNome);
        //                stbHidden.Append(",");
        //                stbHidden.Append(oCarrinho.ApresentacaoID);
        //                stbHidden.Append(",");
        //                stbHidden.Append(oCarrinho.SetorID);
        //                stbHidden.Append(",");
        //                stbHidden.Append(oCarrinho.CotaItemID);
        //                stbHidden.Append(",");
        //                stbHidden.Append(item.PrecoIniciaCom);
        //                stbHidden.Append(",");
        //                stbHidden.Append(item.ValidaBin.ToString());
        //                stbHidden.Append(",");
        //                stbHidden.Append(item.ParceiroID);
        //                stbHidden.Append(",");
        //                stbHidden.Append(oCarrinho.CotaItemID);
        //                item.hiddenInfo = stbHidden.ToString();

        //                item.IngressoID = oCarrinho.IngressoID;

        //                if (item.ValidaBin || item.ParceiroID == 0)
        //                    item.CodigoPromoVisivel = false;
        //                else
        //                    item.CodigoPromoVisivel = true;

        //                listaValidacao.Add(item);

        //            }
        //            else
        //            {
        //                listaValidacao.Add(item);
        //            }
        //        }
        //        return listaValidacao;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public List<EstruturaCotasInfo> CarregarCotas()
        {
            try
            {
                oCotaItem = new IRLib.CotaItem();
                List<EstruturaCotasInfo> lstCotasInfo = new List<EstruturaCotasInfo>();
                EstruturaCotasInfo cotaInfo;
                List<EstruturaCotaItemReserva> lstItem = new List<EstruturaCotaItemReserva>();
                EstruturaCotaItemReserva itemPesquisar = new EstruturaCotaItemReserva();
                foreach (Carrinho oCarrinho in this.OrderBy(c => c.ApresentacaoID).OrderBy(c => c.Setor).OrderBy(c => c.CotaItemID).OrderBy(c => c.CotaItemIDAPS).Where(c => c.isCota.Length > 0))
                {
                    cotaInfo = new EstruturaCotasInfo();

                    #region CotaItem (Apresentacao)
                    if (lstItem.Where(c => c.ID == oCarrinho.CotaItemID).Count() == 0 && oCarrinho.CotaItemID > 0)
                        itemPesquisar = oCotaItem.getCotaItemPorID(oCarrinho.CotaItemID);
                    else if (oCarrinho.CotaItemID > 0)
                        itemPesquisar = lstItem.Where(c => c.ID == oCarrinho.CotaItemID).FirstOrDefault();


                    if (oCarrinho.CotaItemID > 0)
                    {
                        lstItem.Add(itemPesquisar);
                        cotaInfo.CotaItemID = oCarrinho.CotaItemID;
                        cotaInfo.Evento = oCarrinho.Evento;
                        cotaInfo.PrecoNome = oCarrinho.PrecoNome;
                        cotaInfo.IngressoID = oCarrinho.IngressoID;
                        cotaInfo.CodigoPromoVisivel = itemPesquisar.ParceiroID > 0 && !itemPesquisar.ValidaBin;
                        cotaInfo.ValidaBin = itemPesquisar.ValidaBin;
                        cotaInfo.ParceiroID = itemPesquisar.ParceiroID;
                        cotaInfo.CodigoPromo = itemPesquisar.TextoValidacao.Length > 0 ? itemPesquisar.TextoValidacao : "Código Promocional: ";
                        cotaInfo.CPFResponsavel = itemPesquisar.CPFResponsavel;
                        cotaInfo.TemTermo = itemPesquisar.TemTermo;
                    }

                    if (lstItem.Where(c => c.ID == oCarrinho.CotaItemIDAPS).Count() == 0 && oCarrinho.CotaItemIDAPS > 0)
                        itemPesquisar = oCotaItem.getCotaItemPorID(oCarrinho.CotaItemIDAPS);
                    else if (oCarrinho.CotaItemIDAPS > 0)
                        itemPesquisar = lstItem.Where(c => c.ID == oCarrinho.CotaItemIDAPS).FirstOrDefault();
                    #endregion

                    #region CotaItem (Setor)

                    if (oCarrinho.CotaItemIDAPS > 0)
                    {
                        lstItem.Add(itemPesquisar);
                        cotaInfo.CotaItemID_APS = oCarrinho.CotaItemIDAPS;
                        cotaInfo.Evento = oCarrinho.Evento;
                        cotaInfo.PrecoNome = oCarrinho.PrecoNome;
                        cotaInfo.IngressoID = oCarrinho.IngressoID;
                        cotaInfo.CodigoPromoVisivel = itemPesquisar.ParceiroID > 0 && !itemPesquisar.ValidaBin;
                        cotaInfo.ValidaBin = itemPesquisar.ValidaBin;
                        cotaInfo.ParceiroID = itemPesquisar.ParceiroID;
                        cotaInfo.CodigoPromo = itemPesquisar.TextoValidacao.Length > 0 ? itemPesquisar.TextoValidacao : "Código Promocional: ";
                        cotaInfo.CPFResponsavel = itemPesquisar.CPFResponsavel;
                        cotaInfo.TemTermo = itemPesquisar.TemTermo;
                    }

                    #endregion

                    cotaInfo.ApresentacaoID = oCarrinho.ApresentacaoID;
                    cotaInfo.SetorID = oCarrinho.SetorID;


                    lstCotasInfo.Add(cotaInfo);
                }
                return lstCotasInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InserirVIR(int quantidade, int ValeIngressoTipoID)
        {
            try
            {
                Bilheteria oBilheteria = new Bilheteria();
                BindingList<EstruturaRetornoReservaValeIngresso> oRetorno;

                EstruturaReservaInternet estruturaVendaInternet = MontarEstruturaReserva();

                oRetorno = oBilheteria.ReservarValeIngresso(ValeIngressoTipoID, quantidade, this[0].ClienteID, estruturaVendaInternet.CanalID, estruturaVendaInternet.LojaID,
                    estruturaVendaInternet.UsuarioID, this[0].SessionID);
                int quantidadeReservadaCarrinho = 0;

                try
                {
                    for (int i = 0; i < oRetorno.Count; i++)
                    {
                        StringBuilder stbSQL = new StringBuilder();
                        stbSQL.Append("INSERT INTO Carrinho ");
                        stbSQL.Append("(ClienteID, SessionID, TimeStamp, PrecoValor, TaxaConveniencia, ValeIngressoTipoID, ");
                        stbSQL.Append("Status, ValeIngressoID) ");
                        stbSQL.Append("VALUES ");
                        stbSQL.Append("( " + this[0].ClienteID + ", '" + this[0].SessionID + "', ");
                        stbSQL.Append("'" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', " + oRetorno[i].ValorPagamento.ToString().Replace(",", ".") + ", 0, " + ValeIngressoTipoID + ", ");
                        stbSQL.Append("'R', " + oRetorno[i].ID + ")");
                        oDAL.Execute(stbSQL.ToString(), null);
                        quantidadeReservadaCarrinho++;
                    }
                }
                catch (Exception) { }
                return quantidadeReservadaCarrinho;
            }
            catch (Exception)
            {
                throw new Exception("Erro na reserva");
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public CarrinhoLista CarregarRelatOrigem(string clausula)
        {
            string strSql = string.Empty;

            strSql = "SELECT TagOrigem, SUM(Valor) Total FROM vwVendaOrigem " + clausula + " GROUP BY TagOrigem";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oCarrinho = new Carrinho();
                        oCarrinho.TagOrigem = dr["TagOrigem"].ToString();
                        oCarrinho.Total = (decimal)dr["Total"];

                        this.Add(oCarrinho);
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return this;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public CarrinhoLista VerificaLugares(int clienteID, string sessionID)
        {
            string strSql = "";
            int apresentacaoSetorIDAnterior = 0, apresentacaoSetorIDAtual = 0;
            int grupoAnterior = 0, grupoAtual = 0;
            int classificacaoAnterior = 0, classificacaoAtual = 0;
            string codigoAnterior = "";

            #region verifica Cadeiras

            strSql = "SELECT ApresentacaoID, SetorID, Grupo, Classificacao, LugarID, Codigo, ApresentacaoDataHora, Evento, Setor " +
                    "FROM Carrinho " +
                    "WHERE Status = 'R' AND ClienteID = " + clienteID + "AND SessionID = '" + sessionID + "' " +
                    "AND TipoLugar = 'C' AND (PacoteGrupo IS NULL OR PacoteGrupo = '') " +
                    "ORDER BY ApresentacaoID, SetorID, Grupo, Classificacao";

            using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
            {
                while (dr.Read())
                {
                    apresentacaoSetorIDAtual = int.Parse(dr["ApresentacaoID"].ToString() + dr["SetorID"].ToString());
                    grupoAtual = (int)dr["Grupo"];
                    classificacaoAtual = (int)dr["Classificacao"];

                    if (apresentacaoSetorIDAtual != apresentacaoSetorIDAnterior)
                    {
                        //nova apresentacao, zera os campos
                        grupoAnterior = 0;
                        classificacaoAnterior = 0;
                        codigoAnterior = "";
                    }

                    if ((grupoAnterior != 0 && grupoAtual != grupoAnterior) || (classificacaoAnterior != 0 && classificacaoAtual != classificacaoAnterior + 1))
                    {
                        // nao é sequencia
                        //lugaresSeparados += "Evento: " + dr["Evento"].ToString() + " - Apresentação: " + DateTime.ParseExact(dr["ApresentacaodataHora"].ToString(), "yyyyMMddHHmmss", null).ToString("dd/MM/yyy HH:mm") + " - Setor: " + dr["Setor"].ToString() + " - Lugar: (" + codigoAnterior + ", " + dr["Codigo"].ToString() + ")" + EOF;
                        oCarrinho = new Carrinho();
                        oCarrinho.Evento = dr["Evento"].ToString();
                        oCarrinho.ApresentacaoDataHora = DateTime.ParseExact(dr["ApresentacaoDataHora"].ToString(), "yyyyMMddHHmmss", null);
                        oCarrinho.Setor = dr["Setor"].ToString();
                        oCarrinho.Codigo = codigoAnterior + " - " + dr["Codigo"].ToString();
                        this.Add(oCarrinho);
                    }

                    codigoAnterior = dr["Codigo"].ToString();
                    grupoAnterior = (int)dr["Grupo"];
                    classificacaoAnterior = (int)dr["Classificacao"];
                    apresentacaoSetorIDAnterior = int.Parse(dr["ApresentacaoID"].ToString() + dr["SetorID"].ToString());
                }
            }

            #endregion

            #region Mesa Aberta

            //zerar 
            apresentacaoSetorIDAnterior = 0;
            apresentacaoSetorIDAtual = 0;
            grupoAnterior = 0;
            grupoAtual = 0;
            classificacaoAnterior = 0;
            classificacaoAtual = 0;
            codigoAnterior = "";


            strSql = "SELECT ApresentacaoID, SetorID, Grupo, Classificacao, LugarID, Codigo, ApresentacaodataHora, Evento, Setor " +
                     "FROM Carrinho " +
                     "WHERE Status = 'R' AND ClienteID = " + clienteID + "AND SessionID = '" + sessionID + "' " +
                     "AND TipoLugar = 'A' AND (PacoteGrupo IS NULL OR PacoteGrupo = '') " +
                     "ORDER BY ApresentacaoID, SetorID, Grupo, Classificacao";

            using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
            {
                while (dr.Read())
                {
                    apresentacaoSetorIDAtual = int.Parse(dr["ApresentacaoID"].ToString() + dr["SetorID"].ToString());
                    grupoAtual = (int)dr["Grupo"];
                    classificacaoAtual = (int)dr["Classificacao"];

                    if (apresentacaoSetorIDAtual != apresentacaoSetorIDAnterior)
                    {
                        //nova apresentacao, zera os campos
                        grupoAnterior = 0;
                        classificacaoAnterior = 0;
                        codigoAnterior = "";
                    }

                    if ((grupoAnterior != 0 && grupoAtual != grupoAnterior) || (classificacaoAnterior != 0 && classificacaoAtual != classificacaoAnterior))
                    {
                        // nao é sequencia
                        //lugaresSeparados += "Evento: " + dr["Evento"].ToString() + " - Apresentação: " + DateTime.ParseExact(dr["ApresentacaodataHora"].ToString(), "yyyyMMddHHmmss", null).ToString("dd/MM/yyy HH:mm") + " - Setor: " + dr["Setor"].ToString() + " - Lugar: (" + codigoAnterior + ", " + dr["Codigo"].ToString() + ")" + EOF;
                        oCarrinho = new Carrinho();
                        oCarrinho.Evento = dr["Evento"].ToString();
                        oCarrinho.ApresentacaoDataHora = DateTime.ParseExact(dr["ApresentacaoDataHora"].ToString(), "yyyyMMddHHmmss", null);
                        oCarrinho.Setor = dr["Setor"].ToString();
                        oCarrinho.Codigo = codigoAnterior + " - " + dr["Codigo"].ToString();
                        this.Add(oCarrinho);
                    }

                    codigoAnterior = dr["Codigo"].ToString();
                    grupoAnterior = (int)dr["Grupo"];
                    classificacaoAnterior = (int)dr["Classificacao"];
                    apresentacaoSetorIDAnterior = int.Parse(dr["ApresentacaoID"].ToString() + dr["SetorID"].ToString());
                }
            }

            #endregion

            #region Mesa Fechada

            //zerar 
            apresentacaoSetorIDAnterior = 0;
            apresentacaoSetorIDAtual = 0;
            grupoAnterior = 0;
            grupoAtual = 0;
            classificacaoAnterior = 0;
            classificacaoAtual = 0;
            codigoAnterior = "";

            strSql = "SELECT ApresentacaoID, SetorID, Grupo, classificacao, Evento, ApresentacaoDataHora, LugarID, substring(Setor,1,patindex('%(%',Setor)-1) AS Setor, substring(Codigo,1,patindex('%-%',Codigo)-1) AS Codigo " +
                    "FROM Carrinho " +
                    "WHERE Status = 'R' AND ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' " +
                    "AND TipoLugar = 'M' AND (PacoteGrupo IS NULL OR PacoteGrupo = '') " +
                    "GROUP BY " +
                    "ApresentacaoID, SetorID, Grupo, Classificacao, Evento, ApresentacaoDataHora, LugarID, Setor, substring(Codigo,1,patindex('%-%',codigo)-1) " +
                    "ORDER BY ApresentacaoID, SetorID, Grupo, Classificacao";

            using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
            {
                while (dr.Read())
                {
                    apresentacaoSetorIDAtual = int.Parse(dr["ApresentacaoID"].ToString() + dr["SetorID"].ToString());
                    grupoAtual = (int)dr["Grupo"];
                    classificacaoAtual = (int)dr["Classificacao"];


                    if (apresentacaoSetorIDAtual != apresentacaoSetorIDAnterior)
                    {
                        //nova apresentacao, zera os campos
                        grupoAnterior = 0;
                        classificacaoAnterior = 0;
                        codigoAnterior = "";
                    }

                    if ((grupoAnterior != 0 && grupoAtual != grupoAnterior) || (classificacaoAnterior != 0 && classificacaoAtual != classificacaoAnterior + 1))
                    {
                        // nao é sequencia
                        //lugaresSeparados += "Evento: " + dr["Evento"].ToString() + " - Apresentação: " + DateTime.ParseExact(dr["ApresentacaodataHora"].ToString(), "yyyyMMddHHmmss", null).ToString("dd/MM/yyy HH:mm") + " - Setor: " + dr["Setor"].ToString() + " - Lugar: (" + codigoAnterior + ", " + dr["Codigo"].ToString() + ")" + EOF;
                        oCarrinho = new Carrinho();
                        oCarrinho.Evento = dr["Evento"].ToString();
                        oCarrinho.ApresentacaoDataHora = DateTime.ParseExact(dr["ApresentacaoDataHora"].ToString(), "yyyyMMddHHmmss", null);
                        oCarrinho.Setor = dr["Setor"].ToString();
                        oCarrinho.Codigo = codigoAnterior + " - " + dr["Codigo"].ToString();
                        this.Add(oCarrinho);
                    }

                    codigoAnterior = dr["Codigo"].ToString();
                    grupoAnterior = (int)dr["Grupo"];
                    classificacaoAnterior = (int)dr["Classificacao"];
                    apresentacaoSetorIDAnterior = int.Parse(dr["ApresentacaoID"].ToString() + dr["SetorID"].ToString());
                }
            }
            #endregion

            return this;
        }

        public void PreencherInformacoesReserva(string tagOrigem, bool canalPOS = false)
        {
            Precos = new List<EstruturaPrecoReservaSite>();

            foreach (var item in this)
            {
                string sql;

                if (canalPOS)
                {
                    if (item.PrecoID > 0)
                    {
                        var strCon = ConfigurationManager.AppSettings["Conexao"];
                        var builder = new SqlConnectionStringBuilder(strCon);
                        var database = builder.InitialCatalog;

                        sql = string.Format(
                        @"SELECT
                          e.LocalID     AS LocalID,
                          l.Nome        AS Local,
                          e.IR_EventoID AS EventoID,
                          e.Nome        AS Evento,
                          ap.Horario,
                          s.Nome        AS Setor,
                          s.LugarMarcado,
                          tp.ID         AS IR_PrecoID,
                          tp.Nome        AS Preco,
                          tp.Valor,
                          TaxaMaximaEmpresa,
                          l.EmpresaID,
                          e.FilmeID,
                          ap.CodigoProgramacao,
                          s.NVendeLugar,
                          tp.CodigoCinema,
                          e.PossuiTaxaProcessamento,
                          l.Estado,
                          e.LimiteMaximoIngressosEvento,
                          e.LimiteMaximoIngressosEstado
                        FROM Evento e ( NOLOCK )
                          INNER JOIN Local l ( NOLOCK ) ON l.IR_LocalID = e.LocalID
                          INNER JOIN Apresentacao ap ( NOLOCK ) ON e.IR_EventoID = ap.EventoID
                          INNER JOIN Setor s ( NOLOCK ) ON s.ApresentacaoID = ap.IR_ApresentacaoID
                          INNER JOIN {0}..tApresentacaoSetor as aps ( NOLOCK ) ON aps.SetorID = s.IR_SetorID AND aps.ApresentacaoID = ap.IR_ApresentacaoID
                          RIGHT JOIN {0}..tPreco as tp ( NOLOCK ) ON tp.ApresentacaoSetorID = aps.ID
                        WHERE IR_ApresentacaoID = {1}
                        AND IR_SetorID = {2} AND tp.ID IN ({3}) ", database, item.ApresentacaoID, item.SetorID, item.PrecoID);
                    }
                    else if (item.SerieID > 0)
                    {
                        sql = string.Format(
                        @"SELECT DISTINCT  TOP 1
                          e.LocalID     AS LocalID,
                          l.Nome        AS Local,
                          e.IR_EventoID AS EventoID,
                          e.Nome        AS Evento,
                          ap.Horario,
                          s.Nome        AS Setor,
                          s.LugarMarcado,
                          p.IR_PrecoID,
                          p.Nome        AS Preco,
                          p.Valor,
                          TaxaMaximaEmpresa,
                          l.EmpresaID,
                          e.FilmeID,
                          ap.CodigoProgramacao,
                          s.NVendeLugar,
                          p.CodigoCinema,
                          e.PossuiTaxaProcessamento,
                          l.Estado,
                          e.LimiteMaximoIngressosEvento,
                          e.LimiteMaximoIngressosEstado
                        FROM SerieItem si ( NOLOCK )
                          INNER JOIN Evento e ( NOLOCK ) ON si.EventoID = e.IR_EventoID
                          INNER JOIN Local l ( NOLOCK ) ON l.IR_LocalID = e.LocalID
                          INNER JOIN Apresentacao ap ( NOLOCK ) ON e.IR_EventoID = ap.EventoID AND ap.IR_ApresentacaoID = si.ApresentacaoID
                          INNER JOIN Setor s ( NOLOCK ) ON s.IR_SetorID = si.SetorID
                          INNER JOIN Preco p ( NOLOCK ) ON p.IR_PrecoID = si.PrecoID
                        WHERE si.ApresentacaoID = {0} AND si.SetorID = {1} AND si.SerieID = {2}", item.ApresentacaoID, item.SetorID, item.SerieID);
                    }
                    else
                        throw new Exception("Nenhum preço informado, por favor, tente novamente.");
                }
                else
                {
                    if (item.PrecoID > 0)
                    {
                        sql = string.Format(@"SELECT e.LocalID AS LocalID, l.Nome AS Local, e.IR_EventoID AS EventoID, e.Nome AS Evento, ap.Horario, s.Nome AS Setor, s.LugarMarcado, p.IR_PrecoID, p.Nome AS Preco, p.Valor , TaxaMaximaEmpresa,l.EmpresaID,
                                   e.FilmeID, ap.CodigoProgramacao, s.NVendeLugar, p.CodigoCinema, e.PossuiTaxaProcessamento, l.Estado, e.LimiteMaximoIngressosEvento, e.LimiteMaximoIngressosEstado
								FROM Evento e (NOLOCK) 
								INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = e.LocalID 
								INNER JOIN Apresentacao ap (NOLOCK) ON e.IR_EventoID = ap.EventoID
								INNER JOIN Setor s (NOLOCK) ON s.ApresentacaoID = ap.IR_ApresentacaoID
								INNER JOIN Preco p (NOLOCK) ON p.SetorID = s.IR_SetorID AND p.ApresentacaoID = ap.IR_ApresentacaoID 
								WHERE IR_ApresentacaoID  = {0} 
								AND IR_SetorID = {1} AND IR_PrecoID IN ({2}) ", item.ApresentacaoID, item.SetorID, item.PrecoID);
                    }
                    else if (item.SerieID > 0)
                    {
                        sql = string.Format(@"SELECT DISTINCT  TOP 1 e.LocalID AS LocalID, l.Nome AS Local, e.IR_EventoID AS EventoID, e.Nome AS Evento, ap.Horario, s.Nome AS Setor, s.LugarMarcado, p.IR_PrecoID, p.Nome AS Preco, p.Valor , 
                    TaxaMaximaEmpresa,l.EmpresaID, e.FilmeID, ap.CodigoProgramacao, s.NVendeLugar, p.CodigoCinema, e.PossuiTaxaProcessamento, l.Estado, e.LimiteMaximoIngressosEvento, e.LimiteMaximoIngressosEstado                         
								FROM SerieItem si (NOLOCK)
								INNER JOIN Evento e (NOLOCK) ON si.EventoID = e.IR_EventoID
								INNER JOIN Local l (NOLOCK) ON l.IR_LocalID = e.LocalID 
								INNER JOIN Apresentacao ap (NOLOCK) ON e.IR_EventoID = ap.EventoID AND ap.IR_ApresentacaoID = si.ApresentacaoID
								INNER JOIN Setor s (NOLOCK) ON s.IR_SetorID = si.SetorID
								INNER JOIN Preco p (NOLOCK) ON p.IR_PrecoID = si.PrecoID
					WHERE si.ApresentacaoID  = {0}  AND si.SetorID = {1} AND si.SerieID = {2}", item.ApresentacaoID, item.SetorID, item.SerieID);
                    }
                    else
                        throw new Exception("Nenhum preço informado, por favor, tente novamente.");
                }

                using (var dr = oDAL.SelectToIDataReader(sql))
                {
                    while (dr.Read())
                    {
                        item.LocalID = Convert.ToInt32(dr["LocalID"]);
                        item.Local = dr["Local"].ToString();
                        item.Estado = dr["Estado"].ToString();
                        item.EventoID = dr["EventoID"].ToInt32();
                        item.Evento = dr["Evento"].ToString();
                        item.ApresentacaoDataHora = DateTime.ParseExact(dr["Horario"].ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao);
                        item.Setor = dr["Setor"].ToString();
                        item.TipoLugar = dr["LugarMarcado"].ToString();
                        item.TagOrigem = tagOrigem ?? string.Empty;
                        item.TaxaMaximaEmpresa = Convert.ToDecimal(dr["TaxaMaximaEmpresa"]);
                        item.EmpresaID = Convert.ToInt32(dr["EmpresaID"]);
                        item.FilmeID = dr["FilmeID"].ToInt32();
                        item.CodigoProgramacao = dr["CodigoProgramacao"].ToString();
                        item.NVendeLugar = dr["NVendeLugar"].ToBoolean();
                        item.PossuiTaxaProcessamento = dr["PossuiTaxaProcessamento"].ToBoolean();
                        item.LimiteMaximoIngressosEvento = dr["LimiteMaximoIngressosEvento"].ToInt32();
                        item.LimiteMaximoIngressosEstado = dr["LimiteMaximoIngressosEstado"].ToInt32();

                        if (Precos.Count(c => c.ID == dr["IR_PrecoID"].ToInt32()) == 0)
                        {
                            Precos.Add(new EstruturaPrecoReservaSite
                            {
                                ID = dr["IR_PrecoID"].ToInt32(),
                                Quantidade = item.Quantidade,
                                Valor = Convert.ToDecimal(dr["Valor"]),
                                PrecoNome = dr["Preco"].ToString(),
                                QuantidadeMapa = item.QuantidadeMapa,
                                CodigoProgramacao = dr["CodigoProgramacao"].ToString(),
                                CodigoCinema = dr["CodigoCinema"].ToString(),
                                PossuiTaxaProcessamento = dr["PossuiTaxaProcessamento"].ToBoolean(),
                            });
                        }
                    }
                }
            }
        }

        public void PreencherInformacaoLugar(int LugarID, int ApresentacaoID, int quantidade, IRLib.Setor.enumLugarMarcado setorTipo, string TagOrigem, int precoID = 0)
        {
            BD bd = new BD();
            try
            {
                string consulta = string.Format(@"SELECT {0} i.ID AS IngressoID, p.ID as PrecoID, p.Nome AS PrecoNome, p.Valor AS PrecoValor, s.LugarMarcado, ap.ID AS ApresentacaoID, 
                                ap.Horario, e.Nome AS Evento, e.ID AS EventoID, s.ID AS SetorID,  s.Nome AS SetorNome,
                                i.Status AS Status, l.ID AS LocalID, l.Nome AS LocalNome, i.LugarID, i.Codigo, ClienteID, SessionID,  TaxaMaximaEmpresa,m.ID AS EmpresaID, 
                                l.Estado, e.LimiteMaximoIngressosEvento, es.LimiteMaximoIngressosEstado, es.PossuiTaxaProcessamento, i.PrecoID AS PrecoAtualID, i.CotaItemID as CotaItemID
                            FROM tIngresso i (NOLOCK)
                            INNER JOIN tApresentacao ap (NOLOCK) ON i.ApresentacaoID = ap.ID 
                            INNER JOIN tSetor s (NOLOCK) ON i.SetorID = s.ID 
                            INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = ap.ID AND s.ID = aps.SetorID
                            INNER JOIN tPreco p (NOLOCK) ON p.ID = " + (precoID == 0 ? "(select top 1 ID from PrecosDisponiveisPorApresentacaoSetor(aps.ID) order by nome asc) " : precoID.ToString()) +
                          @"INNER JOIN tEvento e (NOLOCK) ON i.EventoID = e.ID AND ap.EventoID = e.ID
                            INNER JOIN tLocal l (NOLOCK) ON l.ID = e.LocalID
                            INNER JOIN tEstado es (NOLOCK) ON es.Sigla COLLATE Latin1_General_CI_AI = l.Estado
                            INNER JOIN tEmpresa m (NOLOCK) ON m.ID = l.EmpresaID
                                WHERE i.LugarID = {1} AND ap.ID = {2} {3}",
                quantidade > 0 ? "TOP " + quantidade : string.Empty, LugarID, ApresentacaoID, (setorTipo == IRLib.Setor.enumLugarMarcado.MesaAberta) ? "AND Status = 'D'" : string.Empty);

                bd.Consulta(consulta);

                while (bd.Consulta().Read())
                {
                    this.Add(new Carrinho
                    {
                        PrecoID = bd.LerInt("PrecoID"),
                        PrecoNome = bd.LerString("PrecoNome"),
                        PrecoValor = bd.LerDecimal("PrecoValor"),
                        TipoLugar = bd.LerString("LugarMarcado"),
                        Status = bd.LerString("Status"),
                        ApresentacaoID = bd.LerInt("ApresentacaoID"),
                        ApresentacaoDataHora = bd.LerDateTime("Horario"),
                        Evento = bd.LerString("Evento"),
                        EventoID = bd.LerInt("EventoID"),
                        SetorID = bd.LerInt("SetorID"),
                        Setor = bd.LerString("SetorNome"),
                        Local = bd.LerString("LocalNome"),
                        LocalID = bd.LerInt("LocalID"),
                        LugarID = bd.LerInt("LugarID"),
                        Codigo = bd.LerString("Codigo"),
                        IngressoID = bd.LerInt("IngressoID"),
                        ClienteID = bd.LerInt("ClienteID"),
                        SessionID = bd.LerString("SessionID"),
                        TaxaMaximaEmpresa = bd.LerDecimal("TaxaMaximaEmpresa"),
                        EmpresaID = bd.LerInt("EmpresaID"),
                        QuantidadeMapa = (setorTipo == IRLib.Setor.enumLugarMarcado.MesaFechada && quantidade > 0 ? quantidade : 1),
                        TagOrigem = TagOrigem ?? string.Empty,
                        Estado = bd.LerString("Estado"),
                        PossuiTaxaProcessamento = bd.LerBoolean("PossuiTaxaProcessamento"),
                        LimiteMaximoIngressosEstado = bd.LerInt("LimiteMaximoIngressosEstado"),
                        LimiteMaximoIngressosEvento = bd.LerInt("LimiteMaximoIngressosEvento"),
                        PrecoAtualID = bd.LerInt("PrecoAtualID"),
                        CotaItemID = bd.LerInt("CotaItemID")
                    });
                }

                if (this.Count == 0)
                    throw new Exception("O lugar selecionado não possui ingressos disponíveis.");
                else if (this[0].Status != "D" && quantidade > 0)
                    throw new IngressoException("Este Ingresso não está mais disponível.");
                else if (this.Count != quantidade && quantidade > 0)
                    throw new SetorException("Esta mesa não possui " + quantidade + " lugares disponíveis");
                else if (quantidade == 0)
                    this[0].QuantidadeMapa = this.Count;
            }
            finally
            {
                bd.Fechar();
            }
        }

        //Mesa Aberta e Cadeira
        public CarrinhoLista MudarPrecoMobile(int precoID, int quantidade, EstruturaReservaInternet estruturaReservaInternet)
        {
            try
            {
                List<Ingresso> lstIngresso = new List<Ingresso>();
                Ingresso oIngresso = new Ingresso();
                CotaItemControle cotaItemControle = new CotaItemControle();

                this.oCarrinho = this[0];

                oIngresso.Control.ID = oCarrinho.IngressoID;
                oIngresso.SessionID.Valor = oCarrinho.SessionID;
                oIngresso.ClienteID.Valor = oCarrinho.ClienteID;
                oIngresso.ApresentacaoID.Valor = oCarrinho.ApresentacaoID;
                oIngresso.SetorID.Valor = oCarrinho.SetorID;
                oIngresso.EventoID.Valor = oCarrinho.EventoID;
                oIngresso.LugarID.Valor = oCarrinho.LugarID;
                oIngresso.Control.ID = oCarrinho.IngressoID;
                oIngresso.EmpresaID.Valor = oCarrinho.EmpresaID;
                oIngresso.TxConv = oCarrinho.TaxaConveniencia;
                oIngresso.CotaItemID = oCarrinho.CotaItemID;
                oIngresso.SerieID.Valor = oCarrinho.SerieID;

                lstIngresso.Add(oIngresso);

                decimal auxiliarTaxaMaximaTotal = 0, valorAux = 0, taxaMaximaEmpresa = 0;

                if (lstIngresso.Count > 0)
                {
                    auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(lstIngresso[0].EmpresaID.Valor, this[0].ClienteID, this[0].SessionID) - lstIngresso[0].TxConv;
                    taxaMaximaEmpresa = GetValorTaxaMaximaEmpresa(oCarrinho.EmpresaID);
                }

                EstruturaPrecoReservaSite preco;
                using (IDataReader dr = oDAL.SelectToIDataReader("SELECT Nome, Valor FROM Preco WHERE IR_PrecoID = " + precoID))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar o preço selecionado.");

                    preco = new EstruturaPrecoReservaSite()
                    {
                        ID = precoID,
                        Quantidade = 1,
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        PrecoNome = dr["Nome"].ToString(),
                        QuantidadeMapa = quantidade,
                    };
                };


                if (!new Bilheteria().MudarPreco(ref lstIngresso, preco, estruturaReservaInternet, oCarrinho.SerieID))
                    throw new Exception("Não foi possível alterar o preço do ingresso selecionado.");

                foreach (Ingresso ingresso in lstIngresso)
                {
                    //Calcula a taxa máxima por empresa
                    if (taxaMaximaEmpresa > 0)
                    {
                        valorAux = taxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                        if (valorAux > 0)
                        {
                            if (valorAux < ingresso.TxConv)
                                ingresso.TxConv = valorAux;
                        }
                        else
                            ingresso.TxConv = 0;

                        auxiliarTaxaMaximaTotal += ingresso.TxConv;
                    }
                }

                oDAL.Execute(string.Format(
                    @"UPDATE Carrinho SET PrecoID = {0}, PrecoNome = '{1}', PrecoValor = {2}, TaxaConveniencia = {3},
                    CotaItemID = {4}, CotaItemIDAPS = {5}, DonoID = 0, DonoCPF = '', CodigoPromocional = '', CotaVerificada = 0 WHERE ID = {6} ",
                    precoID, preco.PrecoNome, preco.Valor.ToString().Replace(',', '.'),
                    lstIngresso[0].TxConv.ToString().Replace(',', '.'), lstIngresso[0].CotaItemID, lstIngresso[0].CotaItemIDAPS, this[0].ID));

                if (oCarrinho.ApresentacaoID > 0 && oCarrinho.CotaItemID > 0)
                {
                    cotaItemControle.DecrementarControladorApresentacao(oCarrinho.ApresentacaoID, oCarrinho.CotaItemID);

                    var ingresso = new IRLib.Ingresso();
                    ingresso.AdicionarCotaItem(oCarrinho.IngressoID, null);
                }

                this.oCarrinho.PrecoID = preco.ID;
                this.oCarrinho.PrecoNome = preco.PrecoNome;
                this.oCarrinho.PrecoValor = preco.Valor;
                this.oCarrinho.TaxaConveniencia = lstIngresso[0].TxConv;
                this.oCarrinho.Total = preco.Valor + lstIngresso[0].TxConv;
                this.oCarrinho.CotaItemIDAPS = lstIngresso[0].CotaItemIDAPS;
                this.oCarrinho.CotaItemID = lstIngresso[0].CotaItemID;
                this.oCarrinho.CotaItem = new CotaItem(lstIngresso[0].Control.ID).GetByID(lstIngresso[0].CotaItemIDAPS > 0 ? lstIngresso[0].CotaItemIDAPS : lstIngresso[0].CotaItemID);
                this.oCarrinho.Precos = (this.PreencherPrecosPorReserva(this.oCarrinho.ApresentacaoID, this.oCarrinho.SetorID, this.oCarrinho.SerieID));

                return this;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        //Mesa Fechada
        public CarrinhoLista MudarPrecoMobile(int precoID, EstruturaReservaInternet estruturaReservaInternet)
        {
            try
            {
                List<Ingresso> lstIngresso = new List<Ingresso>();
                Ingresso oIngresso = new Ingresso();
                CotaItemControle cotaItemControle = new CotaItemControle();

                foreach (Carrinho carrinho in this)
                {
                    oIngresso = new Ingresso();
                    oIngresso.Control.ID = carrinho.IngressoID;
                    oIngresso.SessionID.Valor = carrinho.SessionID;
                    oIngresso.ClienteID.Valor = carrinho.ClienteID;
                    oIngresso.ApresentacaoID.Valor = carrinho.ApresentacaoID;
                    oIngresso.SetorID.Valor = carrinho.SetorID;
                    oIngresso.EventoID.Valor = carrinho.EventoID;
                    oIngresso.LugarID.Valor = carrinho.LugarID;
                    oIngresso.Control.ID = carrinho.IngressoID;
                    oIngresso.EmpresaID.Valor = carrinho.EmpresaID;
                    oIngresso.CotaItemID = carrinho.CotaItemID;
                    oIngresso.TxConv = GetValorConvUtilizadoCarrinho(carrinho.SessionID, carrinho.ClienteID, carrinho.IngressoID);
                    lstIngresso.Add(oIngresso);
                }

                EstruturaPrecoReservaSite preco;

                decimal auxiliarTaxaMaximaTotal = 0, valorAux = 0;

                if (lstIngresso.Count > 0)
                    auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(lstIngresso[0].EmpresaID.Valor, this[0].ClienteID, this[0].SessionID) - lstIngresso.Sum(c => c.TxConv);

                using (IDataReader dr = oDAL.SelectToIDataReader("SELECT Nome, Valor FROM Preco WHERE IR_PrecoID = " + precoID))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar o preço selecionado.");

                    preco = new EstruturaPrecoReservaSite()
                    {
                        ID = precoID,
                        Quantidade = 1,
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        PrecoNome = dr["Nome"].ToString(),
                        QuantidadeMapa = this[0].QuantidadeMapa,
                    };
                };

                if (!new Bilheteria().MudarPreco(ref lstIngresso, preco, estruturaReservaInternet, 0))
                    throw new Exception("Não foi possível alterar o preço do ingresso selecionado.");

                foreach (Ingresso ingresso in lstIngresso)
                {
                    //Calcula a taxa máxima por empresa
                    if (this[0].TaxaMaximaEmpresa > 0)
                    {
                        valorAux = this[0].TaxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                        if (valorAux > 0)
                        {
                            if (valorAux < ingresso.TxConv)
                                ingresso.TxConv = valorAux;
                        }
                        else
                            ingresso.TxConv = 0;

                        auxiliarTaxaMaximaTotal += ingresso.TxConv;
                    }
                }

                var sql = string.Format(
                    @"UPDATE Carrinho SET PrecoID = {0}, PrecoNome = '{1}', PrecoValor = {2},
                            TaxaConveniencia = {3}, CotaItemID = {4}, CotaItemIDAPS = {5}, DonoID = 0,
                            DonoCPF = '', CodigoPromocional = '', CotaVerificada = 0  WHERE LugarID = {6} AND
                            ApresentacaoID = {7} AND ClienteID = {8} AND SessionID = '{9}' ",
                        precoID, preco.PrecoNome,
                        preco.Valor.ToString().Replace(',', '.'),
                        lstIngresso[0].TxConv.ToString().Replace(',', '.'),
                        lstIngresso[0].CotaItemID,
                        lstIngresso[0].CotaItemIDAPS,
                        this[0].LugarID,
                        this[0].ApresentacaoID,
                        this[0].ClienteID,
                        this[0].SessionID
                );

                var linhas = oDAL.Execute(sql);

                if (linhas == 0)
                    throw new Exception("Não foi possível encontrar a mesa selecionada.");

                var Precos = this.PreencherPrecosPorReserva(this[0].ApresentacaoID, this[0].SetorID, this[0].SerieID);
                foreach (Carrinho carrinho in this)
                {
                    if (carrinho.ApresentacaoID > 0 && carrinho.CotaItemID > 0)
                    {
                        cotaItemControle.DecrementarControladorApresentacao(carrinho.ApresentacaoID, carrinho.CotaItemID);

                        var ingresso = new IRLib.Ingresso();
                        ingresso.AdicionarCotaItem(carrinho.IngressoID, null);
                    }

                    carrinho.PrecoID = preco.ID;
                    carrinho.PrecoNome = preco.PrecoNome;
                    carrinho.PrecoValor = preco.Valor;
                    carrinho.TaxaConveniencia = lstIngresso[0].TxConv;
                    carrinho.Total = preco.Valor + lstIngresso[0].TxConv;
                    carrinho.CotaItemIDAPS = lstIngresso[0].CotaItemIDAPS;
                    carrinho.CotaItemID = lstIngresso[0].CotaItemID;
                    carrinho.CotaItem = new CotaItem(carrinho.IngressoID).GetByID(lstIngresso[0].CotaItemIDAPS > 0 ? lstIngresso[0].CotaItemIDAPS : lstIngresso[0].CotaItemID);
                    carrinho.Precos = Precos;
                }

                return this;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        /// <summary>
        /// Mudar Preço
        /// </summary>
        /// <param name="precoID"></param>
        /// <param name="quantidade"></param>
        /// <param name="CanalID"></param>
        /// <param name="CaixaErrado"></param>
        /// <param name="CanalVerificado"></param>
        /// <param name="LojaID"></param>
        /// <param name="CaixaID"></param>
        /// <param name="ClienteID"></param>
        /// <param name="SessionID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        public CarrinhoLista MudarPreco(int precoID, int quantidade, int CanalID, bool CaixaErrado, bool CanalVerificado, int LojaID, int CaixaID, int ClienteID, string SessionID, int UsuarioID)
        {
            try
            {
                List<Ingresso> lstIngresso = new List<Ingresso>();
                Ingresso oIngresso = new Ingresso();
                CotaItemControle cotaItemControle = new CotaItemControle();

                this.oCarrinho = this[0];

                oIngresso.Control.ID = oCarrinho.IngressoID;
                oIngresso.SessionID.Valor = oCarrinho.SessionID;
                oIngresso.ClienteID.Valor = oCarrinho.ClienteID;
                oIngresso.ApresentacaoID.Valor = oCarrinho.ApresentacaoID;
                oIngresso.SetorID.Valor = oCarrinho.SetorID;
                oIngresso.EventoID.Valor = oCarrinho.EventoID;
                oIngresso.LugarID.Valor = oCarrinho.LugarID;
                oIngresso.Control.ID = oCarrinho.IngressoID;
                oIngresso.EmpresaID.Valor = oCarrinho.EmpresaID;
                oIngresso.TxConv = oCarrinho.TaxaConveniencia;
                oIngresso.CotaItemID = oCarrinho.CotaItemID;
                oIngresso.SerieID.Valor = oCarrinho.SerieID;

                lstIngresso.Add(oIngresso);

                decimal auxiliarTaxaMaximaTotal = 0, valorAux = 0, taxaMaximaEmpresa = 0;

                if (lstIngresso.Count > 0)
                {
                    auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(lstIngresso[0].EmpresaID.Valor, this[0].ClienteID, this[0].SessionID) - lstIngresso[0].TxConv;
                    taxaMaximaEmpresa = GetValorTaxaMaximaEmpresa(oCarrinho.EmpresaID);
                }

                EstruturaPrecoReservaSite preco;
                using (IDataReader dr = oDAL.SelectToIDataReader("SELECT Nome, Valor FROM Preco WHERE IR_PrecoID = " + precoID))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar o preço selecionado.");

                    preco = new EstruturaPrecoReservaSite()
                    {
                        ID = precoID,
                        Quantidade = 1,
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        PrecoNome = dr["Nome"].ToString(),
                        QuantidadeMapa = quantidade,
                    };
                };

                EstruturaReservaInternet estruturaReservaInternet = MontarEstruturaReserva(CanalID, CaixaErrado, CanalVerificado, LojaID, CaixaID, ClienteID, SessionID, UsuarioID);

                if (!new Bilheteria().MudarPreco(ref lstIngresso, preco, estruturaReservaInternet, oCarrinho.SerieID))
                    throw new Exception("Não foi possível alterar o preço do ingresso selecionado.");

                foreach (Ingresso ingresso in lstIngresso)
                {
                    //Calcula a taxa máxima por empresa
                    if (taxaMaximaEmpresa > 0)
                    {
                        valorAux = taxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                        if (valorAux > 0)
                        {
                            if (valorAux < ingresso.TxConv)
                                ingresso.TxConv = valorAux;
                        }
                        else
                            ingresso.TxConv = 0;

                        auxiliarTaxaMaximaTotal += ingresso.TxConv;
                    }
                }

                oDAL.Execute(string.Format(
                    @"UPDATE Carrinho SET PrecoID = {0}, PrecoNome = '{1}', PrecoValor = {2}, TaxaConveniencia = {3},
                    CotaItemID = {4}, CotaItemIDAPS = {5}, DonoID = 0, DonoCPF = '', CodigoPromocional = '', CotaVerificada = 0 WHERE ID = {6} ",
                    precoID, preco.PrecoNome, preco.Valor.ToString().Replace(',', '.'),
                    lstIngresso[0].TxConv.ToString().Replace(',', '.'), lstIngresso[0].CotaItemID, lstIngresso[0].CotaItemIDAPS, this[0].ID));

                if (oCarrinho.ApresentacaoID > 0 && oCarrinho.CotaItemID > 0)
                {
                    cotaItemControle.DecrementarControladorApresentacao(oCarrinho.ApresentacaoID, oCarrinho.CotaItemID);

                    var ingresso = new IRLib.Ingresso();
                    ingresso.AdicionarCotaItem(oCarrinho.IngressoID, null);
                }

                this.oCarrinho.PrecoID = preco.ID;
                this.oCarrinho.PrecoNome = preco.PrecoNome;
                this.oCarrinho.PrecoValor = preco.Valor;
                this.oCarrinho.TaxaConveniencia = lstIngresso[0].TxConv;
                this.oCarrinho.Total = preco.Valor + lstIngresso[0].TxConv;
                this.oCarrinho.CotaItemIDAPS = lstIngresso[0].CotaItemIDAPS;
                this.oCarrinho.CotaItemID = lstIngresso[0].CotaItemID;
                this.oCarrinho.CotaItem = new CotaItem(lstIngresso[0].Control.ID).GetByID(lstIngresso[0].CotaItemIDAPS > 0 ? lstIngresso[0].CotaItemIDAPS : lstIngresso[0].CotaItemID);
                this.oCarrinho.Precos = (this.PreencherPrecosPorReserva(this.oCarrinho.ApresentacaoID, this.oCarrinho.SetorID, this.oCarrinho.SerieID));

                return this;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        //Mesa Fechada
        public CarrinhoLista MudarPreco(int precoID)
        {
            try
            {
                List<Ingresso> lstIngresso = new List<Ingresso>();
                Ingresso oIngresso = new Ingresso();
                CotaItemControle cotaItemControle = new CotaItemControle();

                foreach (Carrinho carrinho in this)
                {
                    oIngresso = new Ingresso();
                    oIngresso.Control.ID = carrinho.IngressoID;
                    oIngresso.SessionID.Valor = carrinho.SessionID;
                    oIngresso.ClienteID.Valor = carrinho.ClienteID;
                    oIngresso.ApresentacaoID.Valor = carrinho.ApresentacaoID;
                    oIngresso.SetorID.Valor = carrinho.SetorID;
                    oIngresso.EventoID.Valor = carrinho.EventoID;
                    oIngresso.LugarID.Valor = carrinho.LugarID;
                    oIngresso.Control.ID = carrinho.IngressoID;
                    oIngresso.EmpresaID.Valor = carrinho.EmpresaID;
                    oIngresso.CotaItemID = carrinho.CotaItemID;
                    oIngresso.TxConv = GetValorConvUtilizadoCarrinho(carrinho.SessionID, carrinho.ClienteID, carrinho.IngressoID);
                    lstIngresso.Add(oIngresso);
                }
                EstruturaPrecoReservaSite preco;

                decimal auxiliarTaxaMaximaTotal = 0, valorAux = 0;

                if (lstIngresso.Count > 0)
                    auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(lstIngresso[0].EmpresaID.Valor, this[0].ClienteID, this[0].SessionID) - lstIngresso.Sum(c => c.TxConv);

                using (IDataReader dr = oDAL.SelectToIDataReader("SELECT Nome, Valor FROM Preco WHERE IR_PrecoID = " + precoID))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar o preço selecionado.");

                    preco = new EstruturaPrecoReservaSite()
                    {
                        ID = precoID,
                        Quantidade = 1,
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        PrecoNome = dr["Nome"].ToString(),
                        QuantidadeMapa = this[0].QuantidadeMapa,
                    };
                };

                EstruturaReservaInternet estruturaReservaInternet = MontarEstruturaReserva();

                if (!new Bilheteria().MudarPreco(ref lstIngresso, preco, estruturaReservaInternet, 0))
                    throw new Exception("Não foi possível alterar o preço do ingresso selecionado.");

                CotaItem cotaItem = new CotaItem().GetByID(lstIngresso[0].CotaItemIDAPS > 0 ? lstIngresso[0].CotaItemIDAPS : lstIngresso[0].CotaItemID);

                foreach (Ingresso ingresso in lstIngresso)
                {
                    //Calcula a taxa máxima por empresa
                    if (this[0].TaxaMaximaEmpresa > 0)
                    {
                        valorAux = this[0].TaxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                        if (valorAux > 0)
                        {
                            if (valorAux < ingresso.TxConv)
                                ingresso.TxConv = valorAux;

                        }
                        else
                            ingresso.TxConv = 0;

                        auxiliarTaxaMaximaTotal += ingresso.TxConv;
                    }
                }

                if (oDAL.Execute(string.Format(
                    @"UPDATE Carrinho SET PrecoID = {0}, PrecoNome = '{1}', PrecoValor = {2},
                    TaxaConveniencia = {3}, CotaItemID = {4}, CotaItemIDAPS = {5}, DonoID = 0, DonoCPF = '', CodigoPromocional = '', CotaVerificada = 0  WHERE LugarID = {6} AND ApresentacaoID = {7} AND ClienteID = {8} AND SessionID = '{9}' ",
                    precoID, preco.PrecoNome, preco.Valor.ToString().Replace(',', '.'),
                    lstIngresso[0].TxConv.ToString().Replace(',', '.'), lstIngresso[0].CotaItemID, lstIngresso[0].CotaItemIDAPS,
                    this[0].LugarID, this[0].ApresentacaoID, this[0].ClienteID, this[0].SessionID)) == 0)
                    throw new Exception("Não foi possível encontrar a mesa selecionada.");

                var Precos = this.PreencherPrecosPorReserva(this[0].ApresentacaoID, this[0].SetorID, this[0].SerieID);
                foreach (Carrinho carrinho in this)
                {
                    if (carrinho.ApresentacaoID > 0 && carrinho.CotaItemID > 0)
                    {
                        cotaItemControle.DecrementarControladorApresentacao(carrinho.ApresentacaoID, carrinho.CotaItemID);

                        var ingresso = new IRLib.Ingresso();
                        ingresso.AdicionarCotaItem(carrinho.IngressoID, null);
                    }

                    carrinho.PrecoID = preco.ID;
                    carrinho.PrecoNome = preco.PrecoNome;
                    carrinho.PrecoValor = preco.Valor;
                    carrinho.TaxaConveniencia = lstIngresso[0].TxConv;
                    carrinho.Total = preco.Valor + lstIngresso[0].TxConv;
                    carrinho.CotaItemIDAPS = lstIngresso[0].CotaItemIDAPS;
                    carrinho.CotaItemID = lstIngresso[0].CotaItemID;
                    carrinho.CotaItem = new CotaItem(carrinho.IngressoID).GetByID(lstIngresso[0].CotaItemIDAPS > 0 ? lstIngresso[0].CotaItemIDAPS : lstIngresso[0].CotaItemID);
                    carrinho.Precos = Precos;
                }

                return this;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        //Mesa Aberta e Cadeira
        public CarrinhoLista MudarPreco(int precoID, int quantidade)
        {
            try
            {
                List<Ingresso> lstIngresso = new List<Ingresso>();
                Ingresso oIngresso = new Ingresso();
                CotaItemControle cotaItemControle = new CotaItemControle();

                this.oCarrinho = this[0];

                oIngresso.Control.ID = oCarrinho.IngressoID;
                oIngresso.SessionID.Valor = oCarrinho.SessionID;
                oIngresso.ClienteID.Valor = oCarrinho.ClienteID;
                oIngresso.ApresentacaoID.Valor = oCarrinho.ApresentacaoID;
                oIngresso.SetorID.Valor = oCarrinho.SetorID;
                oIngresso.EventoID.Valor = oCarrinho.EventoID;
                oIngresso.LugarID.Valor = oCarrinho.LugarID;
                oIngresso.Control.ID = oCarrinho.IngressoID;
                oIngresso.EmpresaID.Valor = oCarrinho.EmpresaID;
                oIngresso.TxConv = oCarrinho.TaxaConveniencia;
                oIngresso.CotaItemID = oCarrinho.CotaItemID;
                oIngresso.SerieID.Valor = oCarrinho.SerieID;

                lstIngresso.Add(oIngresso);

                decimal auxiliarTaxaMaximaTotal = 0, valorAux = 0, taxaMaximaEmpresa = 0;

                if (lstIngresso.Count > 0)
                {
                    auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(lstIngresso[0].EmpresaID.Valor, this[0].ClienteID, this[0].SessionID) - lstIngresso[0].TxConv;
                    taxaMaximaEmpresa = GetValorTaxaMaximaEmpresa(oCarrinho.EmpresaID);
                }

                EstruturaPrecoReservaSite preco;
                using (IDataReader dr = oDAL.SelectToIDataReader("SELECT Nome, Valor FROM Preco WHERE IR_PrecoID = " + precoID))
                {
                    if (!dr.Read())
                        throw new Exception("Não foi possível encontrar o preço selecionado.");

                    preco = new EstruturaPrecoReservaSite()
                    {
                        ID = precoID,
                        Quantidade = 1,
                        Valor = Convert.ToDecimal(dr["Valor"]),
                        PrecoNome = dr["Nome"].ToString(),
                        QuantidadeMapa = quantidade,
                    };
                };

                EstruturaReservaInternet estruturaReservaInternet = MontarEstruturaReserva();

                if (!new Bilheteria().MudarPreco(ref lstIngresso, preco, estruturaReservaInternet, oCarrinho.SerieID))
                    throw new Exception("Não foi possível alterar o preço do ingresso selecionado.");

                foreach (Ingresso ingresso in lstIngresso)
                {
                    //Calcula a taxa máxima por empresa
                    if (taxaMaximaEmpresa > 0)
                    {
                        valorAux = taxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                        if (valorAux > 0)
                        {
                            if (valorAux < ingresso.TxConv)
                                ingresso.TxConv = valorAux;
                        }
                        else
                            ingresso.TxConv = 0;

                        auxiliarTaxaMaximaTotal += ingresso.TxConv;
                    }
                }

                oDAL.Execute(string.Format(
                    @"UPDATE Carrinho SET PrecoID = {0}, PrecoNome = '{1}', PrecoValor = {2}, TaxaConveniencia = {3},
                    CotaItemID = {4}, CotaItemIDAPS = {5}, DonoID = 0, DonoCPF = '', CodigoPromocional = '', CotaVerificada = 0 WHERE ID = {6} ",
                    precoID, preco.PrecoNome, preco.Valor.ToString().Replace(',', '.'),
                    lstIngresso[0].TxConv.ToString().Replace(',', '.'), lstIngresso[0].CotaItemID, lstIngresso[0].CotaItemIDAPS, this[0].ID));

                if (oCarrinho.ApresentacaoID > 0 && oCarrinho.CotaItemID > 0)
                {
                    cotaItemControle.DecrementarControladorApresentacao(oCarrinho.ApresentacaoID, oCarrinho.CotaItemID);

                    var ingresso = new IRLib.Ingresso();
                    ingresso.AdicionarCotaItem(oCarrinho.IngressoID, null);
                }

                this.oCarrinho.PrecoID = preco.ID;
                this.oCarrinho.PrecoNome = preco.PrecoNome;
                this.oCarrinho.PrecoValor = preco.Valor;
                this.oCarrinho.TaxaConveniencia = lstIngresso[0].TxConv;
                this.oCarrinho.Total = preco.Valor + lstIngresso[0].TxConv;
                this.oCarrinho.CotaItemIDAPS = lstIngresso[0].CotaItemIDAPS;
                this.oCarrinho.CotaItemID = lstIngresso[0].CotaItemID;
                this.oCarrinho.CotaItem = new CotaItem(lstIngresso[0].Control.ID).GetByID(lstIngresso[0].CotaItemIDAPS > 0 ? lstIngresso[0].CotaItemIDAPS : lstIngresso[0].CotaItemID);
                this.oCarrinho.Precos = (this.PreencherPrecosPorReserva(this.oCarrinho.ApresentacaoID, this.oCarrinho.SetorID, this.oCarrinho.SerieID));

                return this;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public void RecalcularTaxa()
        {
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>();

                decimal valorAux = 0, taxaMaximaEmpresa = 0, auxiliarTaxaMaximaTotal = 0;

                int empresaID = 0;

                foreach (Carrinho item in this)
                {
                    if (empresaID != item.EmpresaID)
                    {
                        auxiliarTaxaMaximaTotal = new Carrinho().TaxaMaximaReservadaPorEmpresa(item.EmpresaID, this[0].ClienteID, this[0].SessionID);
                        empresaID = item.EmpresaID;
                    }

                    taxaMaximaEmpresa = GetValorTaxaMaximaEmpresa(item.EmpresaID);

                    if (auxiliarTaxaMaximaTotal == taxaMaximaEmpresa)
                    {
                        if (this.Count > 1)
                            this.oCarrinho.TaxaConveniencia = 0;
                    }
                    else
                    {
                        //Calcula a taxa máxima por empresa
                        if (taxaMaximaEmpresa > 0)
                        {
                            valorAux = taxaMaximaEmpresa - auxiliarTaxaMaximaTotal;

                            if (valorAux > 0)
                            {
                                if (valorAux < item.TaxaConveniencia || item.TaxaConveniencia == 0)
                                    item.TaxaConveniencia = valorAux;
                            }
                            else
                                item.TaxaConveniencia = 0;

                            auxiliarTaxaMaximaTotal += item.TaxaConveniencia;

                            this.oCarrinho.TaxaConveniencia = item.TaxaConveniencia;

                            parametros.Clear();
                            parametros.Add(new SqlParameter("@ID", item.ID));
                            parametros.Add(new SqlParameter("@TaxaConveniencia", item.TaxaConveniencia));
                            oDAL.Scalar(@"Update Carrinho Set TaxaConveniencia = @TaxaConveniencia Where ID = @ID", parametros.ToArray());
                        }
                    }
                }
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public decimal GetValorTaxaMaximaEmpresa(int empresaID)
        {
            try
            {
                string sql = "SELECT TOP 1 TaxaMaximaEmpresa FROM Local (NOLOCK) WHERE EmpresaID = " + empresaID;

                return Convert.ToDecimal(oDAL.Scalar(sql));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public decimal GetValorConvUtilizadoCarrinho(string sessionID, int clienteID, int ingressoID)
        {
            try
            {
                string sql = "SELECT TOP 1 TaxaConveniencia FROM Carrinho (NOLOCK) WHERE SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " AND IngressoID = " + ingressoID + " AND Status= 'R'";

                return Convert.ToDecimal(oDAL.Scalar(sql));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string VerificarSerie(int SerieID)
        {
            try
            {
                //Serie serie = new Serie();
                //serie.CarregarSerie(SerieID, HttpContext.Current.Session["ClienteID"].ToInt32(),
                //    HttpContext.Current.Session.SessionID, this);

                //List<Carrinho> lstCarrinho = this.Where(c => c.SerieID == SerieID).ToList();
                //if (lstCarrinho.Count() == 0)
                //    return "Desculpe mas você não possui nenhum ingresso desta série em seu carrinho.";

                //d


                return string.Empty;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public string VerificarSeries()
        {
            int SerieID = 0;
            try
            {
                foreach (int serieID in this.Where(c => c.SerieID > 0).Select(c => c.SerieID).Distinct())
                {
                    SerieID = serieID;
                    var series = new Serie().MontarGrupo(serieID, true);
                    if (series.ContemErro())
                        throw new Exception();
                }

                return string.Empty;
            }
            catch (Exception)
            {
                HttpContext.Current.Session["ErroSerie"] = true;
                return "ID=" + SerieID.ToString();
            }
        }

        public List<EstruturaDonoIngresso> AtribuirDonoIngresso(List<EstruturaCotasInfo> lstCotasInfo, ref string[] msgRetorno)
        {
            try
            {
                CotaItemControle oCotaItemControle = new CotaItemControle();
                IngressoCliente oIngressoCliente = new IngressoCliente();
                List<EstruturaDonoIngresso> lstDonoIngresso = new List<EstruturaDonoIngresso>();
                EstruturaCotasInfo cotaInfo;
                EstruturaCotaItemReserva item = new EstruturaCotaItemReserva();
                List<EstruturaCotaValidacaoCliente> lstValidacaoCliente = new List<EstruturaCotaValidacaoCliente>();
                List<EstruturaCotaItemReserva> lstItens = new List<EstruturaCotaItemReserva>();

                oCotaItem = new IRLib.CotaItem();
                int qtdAP = 0;
                int qtdAPS = 0;
                int qtdCotaItemAP = 0;
                int qtdCotaItemAPS = 0;

                int qtdAPCliente = 0;
                int qtdAPSCliente = 0;
                int qtdClienteCotaItemAP = 0;
                int qtdClienteCotaItemAPS = 0;

                int qtdCodigoPromo = 0;
                for (int i = 0; i < lstCotasInfo.Count; i++)
                {
                    Carrinho oCarrinho = this.Where(c => c.IngressoID == lstCotasInfo[i].IngressoID).FirstOrDefault();

                    cotaInfo = new EstruturaCotasInfo();
                    cotaInfo.CotaItemID = lstCotasInfo[i].CotaItemID;
                    cotaInfo.CotaItemID_APS = lstCotasInfo[i].CotaItemID_APS;
                    cotaInfo.DonoID = lstCotasInfo[i].DonoID;
                    cotaInfo.ApresentacaoID = oCarrinho.ApresentacaoID;
                    cotaInfo.SetorID = oCarrinho.SetorID;
                    cotaInfo.IngressoID = lstCotasInfo[i].IngressoID;
                    cotaInfo.ParceiroID = lstCotasInfo[i].ParceiroIDAPS > 0 ? lstCotasInfo[i].ParceiroIDAPS : lstCotasInfo[i].ParceiroID;
                    cotaInfo.CodigoPromocional = lstCotasInfo[i].CodigoPromocional;

                    qtdAP = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.isCota.Length > 0).Count();
                    qtdCotaItemAP = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemID == oCarrinho.CotaItemID && c.CotaItemID > 0).Count();

                    qtdAPS = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.SetorID == oCarrinho.SetorID && c.isCota.Length > 0).Count();
                    qtdCotaItemAPS = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.SetorID == oCarrinho.SetorID && c.CotaItemIDAPS == oCarrinho.CotaItemIDAPS && c.CotaItemIDAPS > 0).Count();

                    qtdAPCliente = lstCotasInfo.Where(c => c.DonoID == lstCotasInfo[i].DonoID && c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemID == oCarrinho.CotaItemID && c.CotaItemID > 0).Count();
                    qtdAPSCliente = lstCotasInfo.Where(c => c.DonoID == lstCotasInfo[i].DonoID && c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemID_APS == oCarrinho.CotaItemIDAPS && c.CotaItemID_APS > 0).Count();
                    qtdClienteCotaItemAP = lstCotasInfo.Where(c => c.CPF == lstCotasInfo[i].CPF && c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemID == oCarrinho.CotaItemID && c.CotaItemID > 0).Count();
                    qtdClienteCotaItemAPS = lstCotasInfo.Where(c => c.CPF == lstCotasInfo[i].CPF && c.ApresentacaoID == oCarrinho.ApresentacaoID && c.SetorID == oCarrinho.SetorID && c.CotaItemID_APS == oCarrinho.CotaItemIDAPS && c.CotaItemID_APS > 0).Count();

                    #region Apresentacao
                    if (lstItens.Where(c => c.ID == cotaInfo.CotaItemID && c.ApresentacaoID == cotaInfo.ApresentacaoID).Count() == 0 && cotaInfo.CotaItemID > 0)
                    {
                        item = oCotaItem.getCotaItemPorID(cotaInfo.CotaItemID, cotaInfo.ApresentacaoID, cotaInfo.SetorID);
                        cotaInfo.ApresentacaoSetorID = item.ApresentacaoSetorID;
                        lstItens.Add(item);

                        cotaInfo.ParceiroID = item.ParceiroID;
                        cotaInfo.ValidaBin = item.ValidaBin;
                        cotaInfo.MaximaApresentacao = item.QuantidadeApresentacao;
                        cotaInfo.MaximaPorClienteApresentacao = item.QuantidadePorClienteApresentacao;
                        cotaInfo.MaximaCotaItem = item.Quantidade;
                        cotaInfo.MaximaPorClienteCotaItem = item.QuantidadePorCliente;
                    }
                    else if (cotaInfo.CotaItemID > 0)
                    {
                        EstruturaCotaItemReserva itemEncontrado = lstItens.Where(c => c.ID == cotaInfo.CotaItemID && c.isApresentacao).FirstOrDefault();
                        cotaInfo.ApresentacaoSetorID = itemEncontrado.ApresentacaoSetorID;
                        cotaInfo.ParceiroID = itemEncontrado.ParceiroID;
                        cotaInfo.ValidaBin = itemEncontrado.ValidaBin;
                        cotaInfo.MaximaApresentacao = itemEncontrado.QuantidadeApresentacao;
                        cotaInfo.MaximaPorClienteApresentacao = itemEncontrado.QuantidadePorClienteApresentacao;
                        cotaInfo.MaximaCotaItem = itemEncontrado.Quantidade;
                        cotaInfo.MaximaPorClienteCotaItem = itemEncontrado.QuantidadePorCliente;
                        cotaInfo.MaximaCodigo = itemEncontrado.QuantidadePorCodigo;
                    }
                    #endregion

                    #region Encontra os Itens da ApresentacaoSetor
                    if (lstItens.Where(c => c.ID == cotaInfo.CotaItemID_APS && c.ApresentacaoID == cotaInfo.ApresentacaoID && c.SetorID == cotaInfo.SetorID).Count() == 0 && cotaInfo.CotaItemID_APS > 0)
                    {
                        item = oCotaItem.getCotaItemPorID(cotaInfo.CotaItemID_APS, cotaInfo.ApresentacaoID, cotaInfo.SetorID, cotaInfo.ApresentacaoSetorID);
                        cotaInfo.ApresentacaoSetorID = item.ApresentacaoSetorID;
                        lstItens.Add(item);

                        cotaInfo.ParceiroID = item.ParceiroID;
                        cotaInfo.ValidaBin = item.ValidaBin;
                        cotaInfo.MaximaCotaItemAPS = item.Quantidade;
                        cotaInfo.MaximaPorClienteCotaItemAPS = item.QuantidadePorCliente;
                        cotaInfo.MaximaApresentacaoSetor = item.QuantidadeApresentacaoSetor;
                        cotaInfo.MaximaPorClienteApresentacaoSetor = item.QuantidadePorClienteApresentacaoSetor;
                        cotaInfo.MaximaCodigo = item.QuantidadePorCodigo;
                    }
                    else if (cotaInfo.CotaItemID_APS > 0)
                    {
                        EstruturaCotaItemReserva itemEncontrado = lstItens.Where(c => c.ID == cotaInfo.CotaItemID_APS && !c.isApresentacao).FirstOrDefault();

                        cotaInfo.ApresentacaoSetorID = itemEncontrado.ApresentacaoSetorID;
                        cotaInfo.ParceiroID = itemEncontrado.ParceiroID;
                        cotaInfo.ValidaBin = itemEncontrado.ValidaBin;
                        cotaInfo.MaximaCotaItemAPS = itemEncontrado.Quantidade;
                        cotaInfo.MaximaPorClienteCotaItemAPS = itemEncontrado.QuantidadePorCliente;
                        cotaInfo.MaximaApresentacaoSetor = itemEncontrado.QuantidadeApresentacaoSetor;
                        cotaInfo.MaximaPorClienteApresentacaoSetor = itemEncontrado.QuantidadePorClienteApresentacaoSetor;
                    }
                    #endregion


                    if (!cotaInfo.ValidaBin && cotaInfo.ParceiroID > 0)
                    {
                        qtdCodigoPromo = lstCotasInfo.Where(c => c.CodigoPromocional.Trim() == cotaInfo.CodigoPromocional.Trim() && c.ApresentacaoID == oCarrinho.ApresentacaoID).Count();

                        if (qtdCodigoPromo > 1)
                        {
                            msgRetorno[0] = "4";
                            msgRetorno[1] = "Não é permitida a utilização do mesmo código promocional diversas vezes para a mesma apresentação.";
                            msgRetorno[2] = cotaInfo.IngressoID.ToString();
                        }

                        string retorno = oCotaItem.ValidarCodigoPromoInternet(cotaInfo.ParceiroID, cotaInfo.CodigoPromocional, cotaInfo.ApresentacaoID);
                        if (retorno.Length > 0)
                        {
                            msgRetorno[0] = "4";
                            msgRetorno[1] = retorno;
                            msgRetorno[2] = cotaInfo.IngressoID.ToString();
                        }
                    }

                    int[] qtds = oCotaItemControle.getQuantidadeNovo(cotaInfo.CotaItemID, cotaInfo.CotaItemID_APS, cotaInfo.ApresentacaoID, cotaInfo.ApresentacaoSetorID);

                    cotaInfo.QuantidadeApresentacao = qtds[0];
                    cotaInfo.QuantidadeApresentacaoSetor = qtds[1];
                    cotaInfo.QuantidadeCota = qtds[2];
                    cotaInfo.QuantidadeCotaAPS = qtds[3];

                    if (cotaInfo.MaximaPorClienteApresentacao > 0 || cotaInfo.MaximaPorClienteApresentacaoSetor > 0 || cotaInfo.MaximaPorClienteCotaItem > 0 || cotaInfo.MaximaPorClienteCotaItemAPS > 0)
                    {
                        //Preenche o OBJ de IngressoCliente e retorna as quantidades ja compradas
                        oIngressoCliente.ApresentacaoID.Valor = cotaInfo.ApresentacaoID;
                        oIngressoCliente.ApresentacaoSetorID.Valor = cotaInfo.ApresentacaoSetorID;
                        oIngressoCliente.DonoID.Valor = cotaInfo.DonoID;
                        oIngressoCliente.CotaItemID.Valor = cotaInfo.CotaItemID;

                        //Busca na Proc e retorna o Array
                        qtds = oIngressoCliente.QuantidadeJaCompradaNovo(cotaInfo.CotaItemID, cotaInfo.CotaItemID_APS);

                        cotaInfo.QuantidadePorClienteApresentacao = qtds[0];
                        cotaInfo.QuantidadePorClienteApresentacaoSetor = qtds[1];
                        cotaInfo.QuantidadePorClienteCota = qtds[2];
                        cotaInfo.QuantidadePorClienteCotaAPS = qtds[3];
                    }
                    else
                    {
                        cotaInfo.QuantidadePorClienteApresentacao = 0;
                        cotaInfo.QuantidadePorClienteApresentacaoSetor = 0;
                        cotaInfo.QuantidadePorClienteCota = 0;
                        cotaInfo.QuantidadePorClienteCotaAPS = 0;
                    }

                    if (qtdClienteCotaItemAP > cotaInfo.MaximaPorClienteCotaItem && cotaInfo.MaximaPorClienteCotaItem > 0)
                    {
                        msgRetorno[0] = "2";
                        msgRetorno[1] = "O preço: \"" + oCarrinho.PrecoNome + "\" possui o limite de " + cotaInfo.MaximaPorClienteCotaItem + " ingresso(s) por cliente.";
                        msgRetorno[2] = cotaInfo.IngressoID.ToString();
                    }
                    else if (qtdClienteCotaItemAPS > cotaInfo.MaximaPorClienteCotaItemAPS && cotaInfo.MaximaPorClienteCotaItemAPS > 0)
                    {
                        msgRetorno[0] = "2";
                        msgRetorno[1] = "O preço: \"" + oCarrinho.PrecoNome + "\" possui o limite de " + cotaInfo.MaximaPorClienteCotaItemAPS + " ingresso(s) por cliente.";
                        msgRetorno[2] = cotaInfo.IngressoID.ToString();
                    }
                    else if (!cotaInfo.ValidaQuantidadeCliente(qtdAPCliente, qtdAPSCliente, qtdClienteCotaItemAP, qtdClienteCotaItemAPS))
                    {
                        msgRetorno[0] = "2";
                        msgRetorno[1] = "O preço: \"" + oCarrinho.PrecoNome + "\" não pode mais ser vendido para este cliente. O limite de venda foi excedido.";
                        msgRetorno[2] = cotaInfo.IngressoID.ToString();
                    }

                    if (!cotaInfo.ValidaQuantidade(qtdAP, qtdAPS, qtdCotaItemAP, qtdCotaItemAPS))
                    {
                        msgRetorno[0] = "3";
                        msgRetorno[1] = "O preço \"" + oCarrinho.PrecoNome + "\" não pode mais ser vendido por excedeu o limite de venda, por favor escolha outro preço.";
                        msgRetorno[2] = cotaInfo.IngressoID.ToString();
                    }

                    if (!string.IsNullOrEmpty(msgRetorno[0]))
                        return lstDonoIngresso;


                    lstDonoIngresso.Add(new EstruturaDonoIngresso
                    {
                        DonoID = lstCotasInfo[i].DonoID,
                        CodigoPromocional = lstCotasInfo[i].CodigoPromocional,
                        CotaItemID = lstCotasInfo[i].CotaItemID,
                        CotaItemIDAPS = lstCotasInfo[i].CotaItemID_APS,
                        IngressoID = lstCotasInfo[i].IngressoID,
                        Quantidade = cotaInfo.getQuantidadeMaximaVender(),
                        QuantidadeAPS = cotaInfo.getQuantidadeMaximaVenderAPS(),
                        CPF = lstCotasInfo[i].CPF,
                        UsarCPFResponsavel = lstCotasInfo[i].CPFResponsavel,
                    });
                }
                return lstDonoIngresso;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CotaItemValidar> VerificarCota(List<CotaItemValidar> listaCotaItemValidar)
        {
            oCotaItem = new IRLib.CotaItem();
            CotaItemControle oCotaItemControle = new CotaItemControle();
            IngressoCliente oIngressoCliente = new IngressoCliente();
            List<EstruturaCotaItemReserva> lstItens = new List<EstruturaCotaItemReserva>();
            EstruturaCotasInfo cotaInfo = new EstruturaCotasInfo();
            EstruturaCotaItemReserva item = new EstruturaCotaItemReserva();
            CodigoPromo oCodigoPromo = new CodigoPromo();

            Parceiro oParceiro = new Parceiro();

            int qtdAP = 0;
            int qtdAPS = 0;
            int qtdCotaItemAP = 0;
            int qtdCotaItemAPS = 0;

            int qtdAPCliente = 0;
            int qtdAPSCliente = 0;
            int qtdClienteCotaItemAP = 0;
            int qtdClienteCotaItemAPS = 0;
            int qtdCodigoPromo = 0;

            List<string> identificacoesDistintas = new List<string>();

            foreach (var cota in listaCotaItemValidar)
            {
                oCarrinho = this.Where(c => c.IngressoID == cota.IngressoID && c.Status == ((char)Carrinho.EnumStatusCarrinho.Resevado).ToString()).FirstOrDefault();

                if (oCarrinho == null)
                {
                    cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.Ingresso;
                    cota.Mensagem = "Não foi possível encontrar este item em seu carrinho, ele pode ter sido expirado.";
                    continue;
                }

                if (cota.Dono != null && (cota.Dono.Preenchido || oCarrinho.CotaItem.Nominal))
                {
                    if (cota.Dono.CamposValidos())
                    {
                        DonoIngresso oDonoIngresso = new DonoIngresso();
                        oDonoIngresso.Nome.Valor = cota.Dono.Nome;
                        oDonoIngresso.CPF.Valor = cota.Dono.CPF;
                        oDonoIngresso.RG.Valor = cota.Dono.RG;
                        oDonoIngresso.Email.Valor = cota.Dono.Email;
                        oDonoIngresso.Telefone.Valor = cota.Dono.Telefone;
                        oDonoIngresso.NomeResponsavel.Valor = cota.Dono.NomeResponsavel;
                        oDonoIngresso.CPFResponsavel.Valor = cota.Dono.CPFResponsavel;
                        oDonoIngresso.DataNascimento.Valor = string.IsNullOrEmpty(cota.Dono.DataNascimento) ? DateTime.MinValue : DateTime.ParseExact(cota.Dono.DataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        oDonoIngresso.Inserir();
                        cota.DonoID = Convert.ToInt32(oDonoIngresso.Control.ID);
                    }
                    else
                    {
                        cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.ClienteInvalido;
                        cota.Mensagem = cota.Dono.Mensagem;
                        continue;
                    }
                }

                cotaInfo.CotaItemID = oCarrinho.CotaItemID;
                cotaInfo.CotaItemID_APS = oCarrinho.CotaItemIDAPS;
                cotaInfo.DonoID = cota.DonoID;
                cotaInfo.ApresentacaoID = oCarrinho.ApresentacaoID;
                cotaInfo.SetorID = oCarrinho.SetorID;
                cotaInfo.IngressoID = cota.IngressoID;
                cotaInfo.ParceiroID = oCarrinho.CotaItem.ParceiroID;
                cotaInfo.CodigoPromocional = cota.Codigo;


                oCarrinho.DonoID = oCarrinho.CotaItem.DonoID = cota.DonoID;

                qtdAP = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.isCota.Length > 0).Count();

                qtdCotaItemAP = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemID == oCarrinho.CotaItemID && c.CotaItemID > 0).Count();

                qtdAPS = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.SetorID == oCarrinho.SetorID && c.isCota.Length > 0).Count();

                qtdCotaItemAPS = this.Where(c => c.ApresentacaoID == oCarrinho.ApresentacaoID && c.SetorID == oCarrinho.SetorID && c.CotaItemIDAPS == oCarrinho.CotaItemIDAPS && c.CotaItemIDAPS > 0).Count();

                qtdAPCliente = this.Where(c => c.CotaItem.DonoID == cota.DonoID && c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemID == oCarrinho.CotaItemID && c.CotaItemID > 0).Count();

                qtdAPSCliente = this.Where(c => c.CotaItem.DonoID == cota.DonoID && c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemIDAPS == oCarrinho.CotaItemIDAPS && c.CotaItemIDAPS > 0).Count();

                if (cota.Dono != null)
                {
                    oCarrinho.DonoCPF = oCarrinho.CotaItem.DonoCPF = cota.Dono.CPF;

                    qtdClienteCotaItemAP = this.Where(c => c.CotaItem.DonoCPF == cota.Dono.CPF && c.ApresentacaoID == oCarrinho.ApresentacaoID && c.CotaItemID == oCarrinho.CotaItemID && c.CotaItemID > 0).Count();
                    qtdClienteCotaItemAPS = this.Where(c => c.CotaItem.DonoCPF == cota.Dono.CPF && c.ApresentacaoID == oCarrinho.ApresentacaoID && c.SetorID == oCarrinho.SetorID && c.CotaItemIDAPS == oCarrinho.CotaItemIDAPS && c.CotaItemIDAPS > 0).Count();

                }


                #region Encontra os Itens da Apresentacao
                if (lstItens.Where(c => c.ID == cotaInfo.CotaItemID && c.ApresentacaoID == cotaInfo.ApresentacaoID).Count() == 0 && cotaInfo.CotaItemID > 0)
                {
                    item = oCotaItem.getCotaItemPorID(oCarrinho.CotaItem.ID, cotaInfo.ApresentacaoID, cotaInfo.SetorID);
                    cotaInfo.ApresentacaoSetorID = item.ApresentacaoSetorID;
                    lstItens.Add(item);

                    cotaInfo.ParceiroID = item.ParceiroID;
                    cotaInfo.ValidaBin = item.ValidaBin;
                    cotaInfo.MaximaApresentacao = item.QuantidadeApresentacao;
                    cotaInfo.MaximaPorClienteApresentacao = item.QuantidadePorClienteApresentacao;
                    cotaInfo.MaximaCotaItem = item.Quantidade;
                    cotaInfo.MaximaPorClienteCotaItem = item.QuantidadePorCliente;
                    cotaInfo.MaximaCodigo = item.QuantidadePorCodigo;
                    cotaInfo.Nominal = item.Nominal;
                }
                else if (cotaInfo.CotaItemID > 0)
                {
                    EstruturaCotaItemReserva itemEncontrado = lstItens.Where(c => c.ID == cotaInfo.CotaItemID && c.isApresentacao).FirstOrDefault();
                    cotaInfo.ApresentacaoSetorID = itemEncontrado.ApresentacaoSetorID;
                    cotaInfo.ParceiroID = itemEncontrado.ParceiroID;
                    cotaInfo.ValidaBin = itemEncontrado.ValidaBin;
                    cotaInfo.MaximaApresentacao = itemEncontrado.QuantidadeApresentacao;
                    cotaInfo.MaximaPorClienteApresentacao = itemEncontrado.QuantidadePorClienteApresentacao;
                    cotaInfo.MaximaCotaItem = itemEncontrado.Quantidade;
                    cotaInfo.MaximaPorClienteCotaItem = itemEncontrado.QuantidadePorCliente;
                    cotaInfo.MaximaCodigo = itemEncontrado.QuantidadePorCodigo;
                    cotaInfo.Nominal = itemEncontrado.Nominal;
                }
                #endregion

                #region Encontra os Itens da ApresentacaoSetor
                if (lstItens.Where(c => c.ID == cotaInfo.CotaItemID_APS && c.ApresentacaoID == cotaInfo.ApresentacaoID && c.SetorID == cotaInfo.SetorID).Count() == 0 && cotaInfo.CotaItemID_APS > 0)
                {
                    item = oCotaItem.getCotaItemPorID(cotaInfo.CotaItemID_APS, cotaInfo.ApresentacaoID, cotaInfo.SetorID, cotaInfo.ApresentacaoSetorID);
                    cotaInfo.ApresentacaoSetorID = item.ApresentacaoSetorID;
                    lstItens.Add(item);

                    cotaInfo.ParceiroID = item.ParceiroID;
                    cotaInfo.ValidaBin = item.ValidaBin;
                    cotaInfo.MaximaCotaItemAPS = item.Quantidade;
                    cotaInfo.MaximaPorClienteCotaItemAPS = item.QuantidadePorCliente;
                    cotaInfo.MaximaApresentacaoSetor = item.QuantidadeApresentacaoSetor;
                    cotaInfo.MaximaPorClienteApresentacaoSetor = item.QuantidadePorClienteApresentacaoSetor;
                    cotaInfo.MaximaCodigo = item.QuantidadePorCodigo;
                }
                else if (cotaInfo.CotaItemID_APS > 0)
                {
                    EstruturaCotaItemReserva itemEncontrado = lstItens.Where(c => c.ID == cotaInfo.CotaItemID_APS && !c.isApresentacao).FirstOrDefault();

                    cotaInfo.ApresentacaoSetorID = itemEncontrado.ApresentacaoSetorID;
                    cotaInfo.ParceiroID = itemEncontrado.ParceiroID;
                    cotaInfo.ValidaBin = itemEncontrado.ValidaBin;
                    cotaInfo.MaximaCotaItemAPS = itemEncontrado.Quantidade;
                    cotaInfo.MaximaPorClienteCotaItemAPS = itemEncontrado.QuantidadePorCliente;
                    cotaInfo.MaximaApresentacaoSetor = itemEncontrado.QuantidadeApresentacaoSetor;
                    cotaInfo.MaximaPorClienteApresentacaoSetor = itemEncontrado.QuantidadePorClienteApresentacaoSetor;
                    cotaInfo.MaximaCodigo = itemEncontrado.QuantidadePorCodigo;
                }
                #endregion

                if (!cotaInfo.ValidaBin && cotaInfo.ParceiroID > 0)
                {
                    qtdCodigoPromo = this.Where(c => !string.IsNullOrEmpty(c.CodigoPromocional) && string.Compare(c.CodigoPromocional.Trim(), cotaInfo.CodigoPromocional.Trim(), true) == 0 && c.ApresentacaoID == oCarrinho.ApresentacaoID).Count();

                    int qtdCotaApresentacao = oCotaItemControle.getQuantidadePromocionalPorCliente(cotaInfo.ApresentacaoID, cotaInfo.CodigoPromocional);

                    if (cotaInfo.MaximaCodigo > 0 && cotaInfo.MaximaCodigo <= (qtdCodigoPromo + qtdCotaApresentacao))
                    {
                        cota.Mensagem = "O código '" + cotaInfo.CodigoPromocional + "' execedeu o limite de utilização de " + cotaInfo.MaximaCodigo + " por apresentação.";
                        cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.Codigo;
                        continue;
                    }

                    oParceiro.Ler(cotaInfo.ParceiroID);

                    string retorno = oCodigoPromo.ValidarCodigoPromo(new List<EstruturaCodigoPromoValidacao>()
					{
						new EstruturaCodigoPromoValidacao()
						{
							ApresentacaoID = cotaInfo.ApresentacaoID,
							CodigoValidacao = cotaInfo.CodigoPromocional,
							CotaItemID = cotaInfo.CotaItemID,
							CotaItemIDAPS = cotaInfo.CotaItemID_APS,
							ParceiroID = cotaInfo.ParceiroID,
							QuantidadePorCodigo = cotaInfo.MaximaCodigo,
                            Tipo = (Enumerators.TipoParceiro)oParceiro.Tipo.Valor,
                            Url = oParceiro.Url.Valor,
					        Identificacao = cota.Identificacao
						},
					});

                    if (retorno.Length > 0)
                    {
                        cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.Codigo;
                        cota.Mensagem = retorno;
                        continue;
                    }
                }

                if (cotaInfo.Nominal && (cotaInfo.MaximaPorClienteApresentacao > 0 || cotaInfo.MaximaPorClienteApresentacaoSetor > 0 || cotaInfo.MaximaPorClienteCotaItem > 0 || cotaInfo.MaximaPorClienteCotaItemAPS > 0))
                {
                    //Preenche o OBJ de IngressoCliente e retorna as quantidades ja compradas
                    oIngressoCliente.ApresentacaoID.Valor = cotaInfo.ApresentacaoID;
                    oIngressoCliente.ApresentacaoSetorID.Valor = cotaInfo.ApresentacaoSetorID;
                    oIngressoCliente.DonoID.Valor = cotaInfo.DonoID;
                    oIngressoCliente.CPF.Valor = cota.Dono.CPF;
                    oIngressoCliente.CotaItemID.Valor = cotaInfo.CotaItemID;

                    //Busca na Proc e retorna o Array
                    int[] qtds = oIngressoCliente.QuantidadeJaCompradaNovo(cotaInfo.CotaItemID, cotaInfo.CotaItemID_APS);

                    cotaInfo.QuantidadePorClienteApresentacao = qtds[0];
                    cotaInfo.QuantidadePorClienteApresentacaoSetor = qtds[1];
                    cotaInfo.QuantidadePorClienteCota = qtds[2];
                    cotaInfo.QuantidadePorClienteCotaAPS = qtds[3];

                    if (cotaInfo.Nominal && qtdClienteCotaItemAP > cotaInfo.MaximaPorClienteCotaItem && cotaInfo.MaximaPorClienteCotaItem > 0)
                    {
                        cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.QuantidadeCliente;
                        cota.Mensagem = "O preço \"" + oCarrinho.PrecoNome + "\" possui o limite de " + cotaInfo.MaximaPorClienteCotaItem + " ingresso(s) por cliente.";
                        continue;
                    }
                    else if (cotaInfo.Nominal && qtdClienteCotaItemAPS > cotaInfo.MaximaPorClienteCotaItemAPS && cotaInfo.MaximaPorClienteCotaItemAPS > 0)
                    {
                        cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.QuantidadeCliente;
                        cota.Mensagem = "O preço \"" + oCarrinho.PrecoNome + "\" possui o limite de " + cotaInfo.MaximaPorClienteCotaItemAPS + " ingresso(s) por cliente.";
                        continue;
                    }
                    else if (!cotaInfo.ValidaQuantidadeCliente(qtdAPCliente, qtdAPSCliente, qtdClienteCotaItemAP, qtdClienteCotaItemAPS))
                    {
                        cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.QuantidadeCliente;
                        cota.Mensagem = "O preço \"" + oCarrinho.PrecoNome + "\" não pode mais ser vendido para este cliente. O limite de venda foi excedido.";
                        continue;
                    }
                }

                oDAL.Execute(
                    string.Format("UPDATE Carrinho SET DonoID = {0}, DonoCPF = '{1}', CodigoPromocional = '{2}', CotaVerificada = 1 WHERE ID = {3}",
                    cota.DonoID, oCarrinho.DonoCPF, cota.Codigo.Replace("'", ""), oCarrinho.ID));

                oCarrinho.CodigoPromocional = cota.Codigo;

            }

            return listaCotaItemValidar;
        }

        public List<CotaItemValidar> VerificarCotaPorQuantidade(List<CotaItemValidar> listaCotaItemValidar)
        {
            oCotaItem = new IRLib.CotaItem();
            List<EstruturaCotaItemReserva> lstItens = new List<EstruturaCotaItemReserva>();
            EstruturaCotasInfo cotaInfo = new EstruturaCotasInfo();
            EstruturaCotaItemReserva item = new EstruturaCotaItemReserva();

            foreach (var cota in listaCotaItemValidar)
            {
                oCarrinho = this.Where(c => c.IngressoID == cota.IngressoID && c.Status == ((char)Carrinho.EnumStatusCarrinho.Resevado).ToString()).FirstOrDefault();

                if (oCarrinho == null)
                {
                    cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.Ingresso;
                    cota.Mensagem = "Não foi possível encontrar este item em seu carrinho, ele pode ter sido expirado.";
                    continue;
                }

                cotaInfo.CotaItemID = oCarrinho.CotaItemID;
                cotaInfo.CotaItemID_APS = oCarrinho.CotaItemIDAPS;
                cotaInfo.DonoID = cota.DonoID;
                cotaInfo.ApresentacaoID = oCarrinho.ApresentacaoID;
                cotaInfo.SetorID = oCarrinho.SetorID;
                cotaInfo.IngressoID = cota.IngressoID;
                cotaInfo.ParceiroID = oCarrinho.CotaItem.ParceiroID;
                cotaInfo.CodigoPromocional = cota.Codigo;

                #region Encontra os Itens da Apresentacao
                if (lstItens.Where(c => c.ID == cotaInfo.CotaItemID && c.ApresentacaoID == cotaInfo.ApresentacaoID).Count() == 0 && cotaInfo.CotaItemID > 0)
                {
                    item = oCotaItem.getCotaItemPorID(oCarrinho.CotaItem.ID, cotaInfo.ApresentacaoID, cotaInfo.SetorID);
                    cotaInfo.ApresentacaoSetorID = item.ApresentacaoSetorID;
                    lstItens.Add(item);

                    cotaInfo.ParceiroID = item.ParceiroID;
                    cotaInfo.ValidaBin = item.ValidaBin;
                    cotaInfo.MaximaApresentacao = item.QuantidadeApresentacao;
                    cotaInfo.MaximaPorClienteApresentacao = item.QuantidadePorClienteApresentacao;
                    cotaInfo.MaximaCotaItem = item.Quantidade;
                    cotaInfo.MaximaPorClienteCotaItem = item.QuantidadePorCliente;
                    cotaInfo.MaximaCodigo = item.QuantidadePorCodigo;
                    cotaInfo.Nominal = item.Nominal;
                }
                else if (cotaInfo.CotaItemID > 0)
                {
                    EstruturaCotaItemReserva itemEncontrado = lstItens.Where(c => c.ID == cotaInfo.CotaItemID && c.isApresentacao).FirstOrDefault();
                    cotaInfo.ApresentacaoSetorID = itemEncontrado.ApresentacaoSetorID;
                    cotaInfo.ParceiroID = itemEncontrado.ParceiroID;
                    cotaInfo.ValidaBin = itemEncontrado.ValidaBin;
                    cotaInfo.MaximaApresentacao = itemEncontrado.QuantidadeApresentacao;
                    cotaInfo.MaximaPorClienteApresentacao = itemEncontrado.QuantidadePorClienteApresentacao;
                    cotaInfo.MaximaCotaItem = itemEncontrado.Quantidade;
                    cotaInfo.MaximaPorClienteCotaItem = itemEncontrado.QuantidadePorCliente;
                    cotaInfo.MaximaCodigo = itemEncontrado.QuantidadePorCodigo;
                    cotaInfo.Nominal = itemEncontrado.Nominal;
                }
                #endregion

                if (cotaInfo.CotaItemID > 0 && cota.Quantidade)
                {
                    var cotaItemControle = new CotaItemControle();
                    var linhas = cotaItemControle.IncrementarControladorApresentacao(cotaInfo.ApresentacaoID, cotaInfo.CotaItemID);
                    if (linhas == 0)
                    {
                        cota.Mensagem = string.Format("O preço \"{0}\" não pode mais ser vendido porque excedeu o limite de venda, por favor escolha outro preço.", oCarrinho.PrecoNome);
                        cota.TipoRetorno = (int)IRLib.CotaItem.EnumRetornoTipo.Quantidade;
                    }
                    else
                    {
                        cota.Quantidade = false;
                        var ingresso = new Ingresso();
                        ingresso.AdicionarCotaItem(cotaInfo.IngressoID, cotaInfo.CotaItemID);
                    }
                }

            }

            return listaCotaItemValidar;
        }

        public class CarrinhoContador
        {

            public decimal Conveniencia { get; set; }
            public string TipoLugar { get; set; }
            public string PacoteGrupo { get; set; }
            public int SerieID { get; set; }
            public int ApresentacaoID { get; set; }
            public decimal PrecoValor { get; set; }

            public int LugarID { get; set; }

            public int SerieItemID { get; set; }

            public bool ItemPromocional { get; set; }

            public int QuantidadePorCodigo { get; set; }

            public int QuantidadePorPromocional { get; set; }

            public int EventoID { get; set; }

            public string Estado { get; set; }

            public bool PossuiTaxaProcessamento { get; set; }

            public int PacoteID { get; set; }

            public int Quantidade { get; set; }

            public int LimiteMaximoIngressosEvento { get; set; }

            public int LimiteMaximoIngressosEstado { get; set; }
        }

        public List<int> CarregarEventosReservados(int clienteID, string sessionID)
        {
            try
            {
                List<int> eventos = new List<int>();
                using (IDataReader dr = oDAL.SelectToIDataReader(
                     string.Format(@"
                        SELECT DISTINCT ISNULL(EventoID, 0) as EventoID FROM Carrinho (NOLOCK) WHERE ClienteID = {0} AND SessionID = '{1}' AND Status = 'R'
                        ", clienteID, sessionID)))
                {
                    while (dr.Read())
                    {
                        int EventoID = dr["EventoID"].ToInt32();

                        if (EventoID > 0)
                            eventos.Add(EventoID);
                    }
                }
                return eventos;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        /// <summary>
        /// Metodo novo de monstar estrutura reserva
        /// Criado para recerber os parâmetros e não pegar da Seção
        /// </summary>
        /// <param name="CanalID"></param>
        /// <param name="CaixaErrado"></param>
        /// <param name="CanalVerificado"></param>
        /// <param name="LojaID"></param>
        /// <param name="CaixaID"></param>
        /// <param name="ClienteID"></param>
        /// <param name="SessionID"></param>
        /// <param name="UsuarioID"></param>
        /// <returns></returns>
        public static EstruturaReservaInternet MontarEstruturaReserva(int CanalID, bool CaixaErrado, bool CanalVerificado, int LojaID, int CaixaID, int ClienteID, string SessionID, int UsuarioID)
        {

            var canais = ConfigurationManager.AppSettings["CanaisInternet"];

            var canaisSplit = canais.Split(',');

            if (CaixaErrado)
                throw new Exception("Esse caixa não pode verder na internet. Abra outro caixa!");
            else if (!CanalVerificado)
            {
                if (!canaisSplit.Contains(CanalID.ToString()) || !new Loja().ValidarLojaCanal(LojaID, new List<string>() { CanalID.ToString() }))
                {
                    throw new Exception("Esse caixa não pode verder na internet. Abra outro caixa!");
                }
                CanalVerificado = true;
            }

            return new EstruturaReservaInternet()
            {
                CaixaID = CaixaID,
                LojaID = LojaID,
                UsuarioID = UsuarioID,
                CanalID = CanalID,
                ClienteID = ClienteID,
                SessionID = SessionID,
                GUID = Guid.NewGuid().ToString(),
            };
        }

        /// <summary>
        /// TODO: Metodo Antigo Verificar se está sendo usado
        /// </summary>
        /// <returns></returns>
        public static EstruturaReservaInternet MontarEstruturaReserva()
        {
            var canalID = HttpContext.Current.Session["CanalID"].ToInt32();
            var caixaErrado = HttpContext.Current.Session["CaixaErrado"].ToBoolean();
            var canalVerificado = HttpContext.Current.Session["CanalVerificado"].ToBoolean();
            var lojaID = HttpContext.Current.Session["LojaID"].ToInt32();

            var canaisSplit = ConfigurationManager.AppSettings["CanaisInternet"].Split(',');

            if (caixaErrado)
                throw new Exception("Esse caixa não pode verder na internet. Abra outro caixa!");
            else if (!canalVerificado)
            {
                if (!canaisSplit.Contains(canalID.ToString()) || !new Loja().ValidarLojaCanal(lojaID, new List<string>() { canalID.ToString() }))
                {
                    HttpContext.Current.Session["CaixaErrado"] = true;
                    throw new Exception("Esse caixa não pode verder na internet. Abra outro caixa!");
                }
                HttpContext.Current.Session["CanalVerificado"] = true;
            }

            return new EstruturaReservaInternet()
            {
                CaixaID = HttpContext.Current.Session["CaixaID"].ToInt32(),
                LojaID = lojaID,
                UsuarioID = HttpContext.Current.Session["UsuarioID"].ToInt32(),
                CanalID = canalID,
                ClienteID = HttpContext.Current.Session["ClienteID"].ToInt32(),
                SessionID = HttpContext.Current.Session.SessionID,
                GUID = Guid.NewGuid().ToString(),
            };
        }

        public static EstruturaReservaInternet MontarEstruturaReservaMobile(int ClienteID, string SessionID)
        {
            return new EstruturaReservaInternet()
            {
                LojaID = Loja.INTERNET_LOJA_ID,
                CaixaID = 0,
                CanalID = Canal.CANAL_INTERNET,
                UsuarioID = IRLib.Usuario.INTERNET_USUARIO_ID,
                ClienteID = ClienteID,
                SessionID = SessionID,
                GUID = Guid.NewGuid().ToString(),
            };
        }

        public void PreencherInformacoesReservaPacote()
        {
            try
            {
                var pacoteID = 0;

                var oPacote = new Pacote();
                var estrutura = new EstruturaTaxaProcessamentoPacote();

                foreach (var carrinho in this.Where(c => c.PacoteID > 0).Distinct().OrderBy(c => c.PacoteID))
                {
                    if (pacoteID != carrinho.PacoteID)
                        estrutura = oPacote.TaxaProcessamento(carrinho.PacoteID);

                    this.Where(c => c.PacoteID == carrinho.PacoteID).ToList().ForEach(delegate(Carrinho item)
                    {
                        item.PossuiTaxaProcessamento = estrutura.PossuiTaxaProcessamento;
                        item.LimiteMaximoIngressosEstado = estrutura.LimiteIngressosEstado;
                        item.LimiteMaximoIngressosEvento = estrutura.LimiteIngressosEvento;
                        item.Estado = estrutura.Estado;
                    });

                    pacoteID = carrinho.PacoteID;
                }
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public decimal TotalTaxaProcessamento()
        {
            if (this.Where(c => c.TaxaProcessamento > 0).Count() == 0)
                return 0;

            return this.Where(c => c.TaxaProcessamento > 0).Select(c => new { Estado = c.Estado, TaxaProcessamento = c.TaxaProcessamento }).Distinct().Sum(c => c.TaxaProcessamento);

        }

        public static decimal CalcularTaxaProcessamento(List<Carrinho> lista)
        {
            if (lista.Where(c => c.TaxaProcessamento > 0).Count() == 0)
                return 0;

            return lista.Where(c => c.TaxaProcessamento > 0).Select(c => new { Estado = c.Estado, TaxaProcessamento = c.TaxaProcessamento }).Distinct().Sum(c => c.TaxaProcessamento);

        }

        public int VerificarQuantidadeReservada(int EventoID, int ClienteID, string SessionID)
        {
            try
            {
                int quantidade = 0;

                using (IDataReader dr = oDAL.SelectToIDataReader(string.Format(@"SELECT ISNULL(Quantidade, 0) as Quantidade  FROM vwReservasVendaFlipReservado where SessionID = '{0}' AND ClienteID = {1} AND EventoID = {2}", SessionID, ClienteID, EventoID)))
                {
                    if (dr.Read())
                    {
                        quantidade = dr["Quantidade"].ToInt32();
                    }
                }

                return quantidade;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }
    }
}