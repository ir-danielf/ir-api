using CTLib;
using IRLib.ClientObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text;
using System.Web;



namespace IngressoRapido.Lib
{
    /// <summary>
    /// Summary description for Carrinho.
    /// </summary>
    /// 
    [DataContract()]
    public class Carrinho
    {

        public enum EnumStatusCarrinho
        {
            Expirado = 'E',
            Resevado = 'R'
        }

        private string URL = ConfigurationManager.AppSettings["DiretorioImagensEventos"].ToString();
        private string URLValeIngresso = ConfigurationManager.AppSettings["DiretorioImagensValeIngresso"].ToString();
        private DAL oDAL = new DAL();

        #region Fields
        public string Status { get; set; }

        public string StatusDetalhado
        {
            get
            {
                if (Status == "E")
                    return "Expirado";
                else
                    return string.Empty;
            }
            set
            {
                if (value != null)
                    Status = value.Trim();
            }
        }

        public int EmpresaID { get; set; }

        [DataMember]
        public decimal TaxaMaximaEmpresa { get; set; }

        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public int EventoID { get; set; }

        [DataMember]
        public decimal TaxaConveniencia { get; set; }

        [DataMember]
        public int Quantidade { get; set; }

        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public int ClienteID { get; set; }

        private int precoexclusivocodigoID;
        public int PrecoExclusivoCodigoID
        {
            get { return precoexclusivocodigoID; }
            set { precoexclusivocodigoID = value; }
        }

        public DateTime Timestamp { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public int LugarID { get; set; }

        [DataMember]
        public int IngressoID { get; set; }

        [DataMember]
        public string TipoLugar { get; set; }

        [DataMember]
        public int ApresentacaoID { get; set; }

        [DataMember]
        public int SetorID { get; set; }

        [DataMember]
        public int PrecoID { get; set; }

        [DataMember]
        public int PrecoAtualID { get; set; }

        [DataMember]
        public int GerenciamentoIngressosID { get; set; }

        public int LocalID { get; set; }

        public string Local { get; set; }

        public string Evento { get; set; }

        public DateTime ApresentacaoDataHora { get; set; }

        public string Setor { get; set; }

        public string SetorExibicao
        {
            get
            {
                if (Setor != "-")
                    return "Setor: " + Setor;
                else
                    return string.Empty;
            }
        }

        public decimal PrecoValor { get; set; }

        public string PrecoNome { get; set; }

        public string PrecoExibicao
        {
            get
            {
                if (PrecoNome != "-")
                    return "Preço: " + PrecoNome;
                else
                    return string.Empty;
            }
        }

        public string TagOrigem { get; set; }
        public decimal Total { get; set; }

        [DataMember]
        public int PacoteID { get; set; }

        public string PacoteGrupo { get; set; }

        public string PacoteNome { get; set; }

        public int Grupo { get; set; }

        public int Classificacao { get; set; }

        public decimal ValorTotalIngresso
        {
            get { return (PrecoValor + TaxaConveniencia); }
        }

        public string ImagemEvento
        {
            get { return (URL + this.EventoID.ToString("eI000000") + "thumb.jpg"); }
        }

        public string ImagemLugar
        {
            get
            {
                //if (this.tipoLugar == "C")
                return "Images/cad_mapa.png";
                //else if (this.tipoLugar == "M")
                //    return "Images/MesaFechada_mapa.png";
                //else if (this.tipoLugar == "A")
                //    return "Images/MesaAberta_mapa.png";
                //else if (this.tipoLugar == "P")
                //    return "Images/Pista_mapa.png";
                //else
                //    return string.Empty;
            }
        }

        public bool DesconsideraLimite { get; set; }

        public int SpecialEvent { get; set; }

        public string IngressoLugarApresentacaoIDCarrinhoID
        {
            get { return (this.IngressoID.ToString() + ";" + this.LugarID.ToString() + ";" + this.ApresentacaoID + ";" + this.PacoteGrupo + ";" + this.ID + ";" + this.PrecoID); }
        }

        public string EventoIDPacoteGrupoLugarIDCodigo
        {
            get { return (EventoID + ";" + PacoteGrupo + ";" + LugarID + ";" + Codigo); }
        }

        private string localImagemNome;
        public string LocalImagemNome
        {
            get
            {
                if (localImagemNome == string.Empty || localImagemNome == null || localImagemNome == ConfigurationManager.AppSettings["DiretorioImagensMapas"].ToString())
                    return ("Images/mapa_ind.jpg");
                else
                    return (ConfigurationManager.AppSettings["DiretorioImagensMapas"].ToString() + localImagemNome);
            }
            set { localImagemNome = value; }

        }

        public int CotaItemID { get; set; }
        public int CotaItemIDAPS { get; set; }

        public string isCota
        {
            get
            {
                if (this.CotaItemID > 0 || this.CotaItemIDAPS > 0)
                    return "*";
                else return string.Empty;
            }
        }

        public int QuantidadeMapa { get; set; }

        [DataMember]
        public int SerieID { get; set; }

        public List<EstruturaIDNome> Precos { get; set; }

        public CotaItem CotaItem { get; set; }
        public List<CotaItem> ListaCotaItem { get; set; }
        #region ValeIngresso

        public string VirThumb
        {
            get { return URLValeIngresso + ValeIngressoTipoID.ToString("ivir000000") + "thumb.jpg"; }
        }
        public int ValeIngressoID { get; set; }
        public int ValeIngressoTipoID { get; set; }
        public string ValidadeData { get; set; }
        public string ValeIngressoNome { get; set; }
        public string Acumulativo { get; set; }
        public int QuantidadeVIR { get; set; }
        public char ValorTipo { get; set; }
        public decimal ValorTroca { get; set; }
        public string NomePresenteado { get; set; }
        public bool TrocaEntrega { get; set; }
        public bool TrocaConveniencia { get; set; }
        public bool TrocaIngresso { get; set; }
        public bool CarregarSerieItem { get; set; }
        private int serieItemID { get; set; }
        public int SerieItemID
        {
            get
            {
                if (serieItemID == 0 && CarregarSerieItem && SerieID > 0)
                {
                    SerieItem serieItem = new SerieItem();
                    serieItem.GetByPrecoSerie(SerieID, PrecoID);

                    this.ItemPromocional = serieItem.Promocional;
                    this.QuantidadePorPromocional = serieItem.QuantidadePorPromocional;
                    this.SerieItemID = serieItem.ID;
                    return serieItem.ID;
                }
                else
                    return 0;
            }
            set
            {
                this.serieItemID = value;
            }
        }

        public int DonoID { get; set; }
        public string DonoCPF { get; set; }
        public string CodigoPromocional { get; set; }
        public bool ItemPromocional { get; set; }
        public int QuantidadePorPromocional { get; set; }

        #endregion


        public int FilmeID { get; set; }

        public string CodigoProgramacao { get; set; }

        public bool NVendeLugar { get; set; }

        public string Estado { get; set; }

        private bool possuiTaxaProcessamento { get; set; }
        public bool PossuiTaxaProcessamento
        {
            get
            {
                return possuiTaxaProcessamento || TaxaProcessamento > 0;
            }
            set { possuiTaxaProcessamento = value; }
        }

        public decimal TaxaProcessamento { get; set; }

        public int LimiteMaximoIngressosEvento { get; set; }

        public int LimiteMaximoIngressosEstado { get; set; }

        [DataMember]
        public bool IngressoBem { get; set; }

        public List<string> KitCamisas { get; set; }

        public bool SeguroMondial { get; set; }

        public List<EstruturaPrecoQuantidade> ListQuantidadePreco { get; set; }

        #endregion

        public enum TipoReserva
        {
            ValeIngresso,
            Ingresso
        }

        public Carrinho()
        {

        }

        public Carrinho(int id)
        {
            this.ID = id;
        }

        public Carrinho(int id, int ingressoID)
        {
            this.ID = id;
            this.IngressoID = ingressoID;
        }
        public Carrinho(int id, int ingressoID, int lugarID, int apresentacaoID, string sessionID)
        {
            this.ID = id;
            this.IngressoID = ingressoID;
            this.LugarID = lugarID;
            this.ApresentacaoID = apresentacaoID;
            this.SessionID = sessionID;
        }

        /// <summary>
        /// se o id for zero a função irá tentar buscar automaticamente por lugarID, SessionID e ApresentaçãoID (utilizado em mesa fechada)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Carrinho GetByID(int id)
        {
            string strSql = string.Empty;
            if (id > 0)
                strSql = "SELECT TOP 1 * FROM Carrinho (NOLOCK) WHERE ID = " + id;
            else
                strSql = string.Format(@"SELECT TOP 1 * FROM Carrinho (NOLOCK)
                            WHERE ApresentacaoID = {0} AND LugarID = {1} AND ClienteID = {2} AND SessionID ='{3}'",
                            this.ApresentacaoID, this.LugarID, this.ClienteID, this.SessionID);

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        this.ID = Convert.ToInt32(dr["ID"].ToString());
                        this.ClienteID = Convert.ToInt32(dr["ClienteID"].ToString());
                        this.Codigo = dr["Codigo"].ToString();
                        this.LugarID = Convert.ToInt32(dr["LugarID"].ToString());
                        this.IngressoID = Convert.ToInt32(dr["IngressoID"].ToString());
                        this.TipoLugar = dr["TipoLugar"].ToString();
                        this.ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"].ToString());
                        this.SetorID = Convert.ToInt32(dr["SetorID"].ToString());
                        this.PrecoID = Convert.ToInt32(dr["PrecoID"].ToString());
                        this.LocalID = (int)dr["LocalID"];
                        this.Local = dr["Local"].ToString();
                        this.EventoID = (int)dr["EventoID"];
                        this.Evento = dr["Evento"].ToString();
                        this.ApresentacaoDataHora = DateTime.ParseExact(dr["ApresentacaoDataHora"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        this.Setor = dr["Setor"].ToString();
                        this.PrecoNome = dr["PrecoNome"].ToString();
                        this.PrecoValor = (decimal)dr["PrecoValor"];
                        this.Timestamp = DateTime.ParseExact(dr["TimeStamp"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        this.SpecialEvent = DBNull.Value.Equals(dr["IsSpecial"]) ? 0 : Convert.ToInt32(dr["IsSpecial"]);
                        this.SessionID = dr["SessionID"].ToString();
                        this.Status = dr["Status"].ToString();
                        this.TaxaConveniencia = Convert.ToDecimal(dr["TaxaConveniencia"]);
                        this.EmpresaID = Convert.ToInt32(dr["EmpresaID"]);
                        this.SerieID = dr["SerieID"].ToInt32();
                    }
                }

                oDAL.ConnClose();   // Fecha conexão da classe DataAccess
                return this;

            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public Carrinho GetByIngresso(int ingressoID, int clienteID, string sessionID)
        {
            if (ID == 0)
                throw new Exception("Ingresso Inválido.");

            string strSql = string.Format(@"
                    SELECT * FROM Carrinho (NOLOCK)
                    WHERE IngressoID = {0} AND ClienteID = {1} AND SessionID = {2}",
                    ingressoID, clienteID, sessionID);

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        this.ID = Convert.ToInt32(dr["ID"].ToString());
                        this.ClienteID = Convert.ToInt32(dr["ClienteID"].ToString());
                        this.Codigo = dr["Codigo"].ToString();
                        this.LugarID = Convert.ToInt32(dr["LugarID"].ToString());
                        this.IngressoID = Convert.ToInt32(dr["IngressoID"].ToString());
                        this.TipoLugar = dr["TipoLugar"].ToString();
                        this.ApresentacaoID = Convert.ToInt32(dr["ApresentacaoID"].ToString());
                        this.SetorID = Convert.ToInt32(dr["SetorID"].ToString());
                        this.PrecoID = Convert.ToInt32(dr["PrecoID"].ToString());
                        this.LocalID = (int)dr["LocalID"];
                        this.Local = dr["Local"].ToString();
                        this.EventoID = (int)dr["EventoID"];
                        this.Evento = dr["Evento"].ToString();
                        this.ApresentacaoDataHora = DateTime.ParseExact(dr["ApresentacaoDataHora"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        this.Setor = dr["Setor"].ToString();
                        this.PrecoNome = dr["PrecoNome"].ToString();
                        this.PrecoValor = (decimal)dr["PrecoValor"];
                        this.Timestamp = DateTime.ParseExact(dr["TimeStamp"].ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                        this.SpecialEvent = DBNull.Value.Equals(dr["IsSpecial"]) ? 0 : Convert.ToInt32(dr["IsSpecial"]);
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

        public void GetByPrecoID(int precoID, int clienteID, string sessionID)
        {
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(
                    string.Format(
                        @"SELECT TOP 1
                                ID, IngressoID
                          FROM Carrinho (NOLOCK) 
                            WHERE PrecoID = {0} AND ClienteID = {1} AND SessionID = '{2}' AND Status = 'R'",
                         precoID, clienteID, sessionID)))
                {
                    if (!dr.Read())
                        throw new Exception("Ingressos não localizados.");

                    this.ID = dr["ID"].ToInt32();
                    this.IngressoID = dr["IngressoID"].ToInt32();
                    dr.Close();
                }
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public Carrinho PreencherInformacaoIngresso(int ingressoID, int quantidadeMapa, int precoID, int serieID, string TagOrigem)
        {
            BD bd = new BD();
            try
            {
                string consulta = string.Empty;

                if (serieID == 0)
                    consulta = string.Format(@"SELECT TOP 1 p.ID AS PrecoID, p.Nome AS PrecoNome, p.Valor AS PrecoValor, s.LugarMarcado, ap.ID AS ApresentacaoID, 
                                ap.Horario, e.Nome AS Evento, e.ID AS EventoID, s.ID AS SetorID,  s.Nome AS SetorNome,
                                i.Status AS Status, l.ID AS LocalID, l.Nome AS LocalNome, i.LugarID, i.Codigo , TaxaMaximaEmpresa,m.ID AS EmpresaID, l.Estado, 
                                e.LimiteMaximoIngressosEvento, es.LimiteMaximoIngressosEstado, es.PossuiTaxaProcessamento
                            FROM tIngresso i (NOLOCK)
                            INNER JOIN tApresentacao ap (NOLOCK) ON i.ApresentacaoID = ap.ID 
                            INNER JOIN tSetor s (NOLOCK) ON i.SetorID = s.ID 
                            INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = ap.ID AND s.ID = aps.SetorID
                            INNER JOIN tPreco p (NOLOCK) ON p.ID = {0}
                            INNER JOIN tEvento e (NOLOCK) ON i.EventoID = e.ID AND ap.EventoID = e.ID
                            INNER JOIN tLocal l (NOLOCK) ON l.ID = e.LocalID 
                            INNER JOIN tEstado es (NOLOCK) ON es.Sigla COLLATE Latin1_General_CI_AI = l.Estado
                            INNER JOIN tEmpresa m (NOLOCK) ON m.ID = l.EmpresaID
                                WHERE i.ID = {1}",
                            precoID > 0 ? precoID.ToString() : "(select top 1 ID from PrecosDisponiveisPorApresentacaoSetor(aps.ID) order by nome asc)",
                            ingressoID);
                else
                    consulta = string.Format(@"SELECT TOP 1 p.ID AS PrecoID, p.Nome AS PrecoNome, p.Valor AS PrecoValor, s.LugarMarcado, ap.ID AS ApresentacaoID, 
                                ap.Horario, e.Nome AS Evento, e.ID AS EventoID, s.ID AS SetorID,  s.Nome AS SetorNome,
                                i.Status AS Status, l.ID AS LocalID, l.Nome AS LocalNome, i.LugarID, i.Codigo , TaxaMaximaEmpresa,m.ID AS EmpresaID, l.Estado, 
                                e.LimiteMaximoIngressosEvento, es.LimiteMaximoIngressosEstado, es.PossuiTaxaProcessamento
                            FROM tIngresso i (NOLOCK)
                            INNER JOIN tApresentacao ap (NOLOCK) ON i.ApresentacaoID = ap.ID 
                            INNER JOIN tSetor s (NOLOCK) ON i.SetorID = s.ID 
                            INNER JOIN tApresentacaoSetor aps (NOLOCK) ON aps.ApresentacaoID = ap.ID AND s.ID = aps.SetorID
                            INNER JOIN tSerieItem si (NOLOCK) ON si.ApresentacaoID = i.ApresentacaoID AND si.SetorID = i.SetorID AND si.SerieID = {0}
                            INNER JOIN tPreco p (NOLOCK) ON p.ID = si.PrecoID
                            INNER JOIN tEvento e (NOLOCK) ON i.EventoID = e.ID AND ap.EventoID = e.ID
                            INNER JOIN tLocal l (NOLOCK) ON l.ID = e.LocalID 
                            INNER JOIN tEstado es (NOLOCK) ON es.Sigla COLLATE Latin1_General_CI_AI = l.Estado
                            INNER JOIN tEmpresa m (NOLOCK) ON m.ID = l.EmpresaID
                                WHERE i.ID = {1} {2}",
                            serieID,
                            ingressoID, precoID > 0 ? "AND p.ID = " + precoID : string.Empty);

                bd.Consulta(consulta);

                if (bd.Consulta().Read())
                {
                    this.PrecoID = precoID > 0 && serieID == 0 ? precoID : bd.LerInt("PrecoID");
                    this.PrecoNome = bd.LerString("PrecoNome");
                    this.PrecoValor = bd.LerDecimal("PrecoValor");
                    this.TipoLugar = bd.LerString("LugarMarcado");
                    this.Status = bd.LerString("Status");
                    this.ApresentacaoID = bd.LerInt("ApresentacaoID");
                    this.ApresentacaoDataHora = bd.LerDateTime("Horario");
                    this.Evento = bd.LerString("Evento");
                    this.EventoID = bd.LerInt("EventoID");
                    this.SetorID = bd.LerInt("SetorID");
                    this.Setor = bd.LerString("SetorNome");
                    this.Local = bd.LerString("LocalNome");
                    this.LocalID = bd.LerInt("LocalID");
                    this.LugarID = bd.LerInt("LugarID");
                    this.Codigo = bd.LerString("Codigo");
                    this.IngressoID = ingressoID;
                    this.QuantidadeMapa = quantidadeMapa;
                    this.EmpresaID = bd.LerInt("EmpresaID");
                    this.TaxaMaximaEmpresa = bd.LerDecimal("TaxaMaximaEmpresa");
                    this.TagOrigem = TagOrigem ?? string.Empty;
                    this.Estado = bd.LerString("Estado");
                    this.PossuiTaxaProcessamento = bd.LerBoolean("PossuiTaxaProcessamento");
                    this.LimiteMaximoIngressosEstado = bd.LerInt("LimiteMaximoIngressosEstado");
                    this.LimiteMaximoIngressosEvento = bd.LerInt("LimiteMaximoIngressosEvento");

                }

                if (this.Status != "D")
                    throw new IRLib.IngressoException("Este Ingresso não está mais disponível.");

                return this;
            }
            finally
            {
                bd.Fechar();
            }
        }

        public int NumItensCarrinhoCliente(TipoReserva tipo)
        {
            int retorno = 0;
            int clienteID = Convert.ToInt32(HttpContext.Current.Session["ClienteID"]);

            string strSQL = string.Empty;
            if (tipo == TipoReserva.Ingresso)
            {
                strSQL = "sp_getQuantidadeReserva '" + HttpContext.Current.Session.SessionID.ToString() + "', " + clienteID;
            }
            else
                strSQL = "SELECT Count(ValeIngressoID) FROM Carrinho (NOLOCK) WHERE SessionID = '" + HttpContext.Current.Session.SessionID + "' AND ClienteID = " + clienteID +
                    " AND Status = 'R'";
            try
            {
                retorno = Convert.ToInt32(oDAL.Scalar(strSQL));
                return retorno;
            }
            catch
            {
                return 0;
            }
        }
        public decimal TaxaMaximaReservadaPorEmpresa(int empresaID, int clienteID, string sessionID)
        {
            try
            {
                var strSQL = string.Format(@"SELECT SUM(TaxaConveniencia) FROM Carrinho c(NOLOCK)
                                WHERE SessionID = '{0}' AND Status = 'R' AND ClienteID = {1} AND EmpresaID = {2} GROUP BY EmpresaID", sessionID, clienteID, empresaID);

                return Convert.ToDecimal(oDAL.Scalar(strSQL));
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro na consulta ao banco de dados!", ex);
            }
        }

        public decimal ValorTotalCarrinhoCliente(string SessionID, int ClienteID)
        {

            string strSql =
                @"SELECT 
		            IsNull(
			            SUM(TaxaConveniencia) + 
			            SUM(PrecoValor), 0) + ValorTaxaProcessamento AS Valor
			            FROM Carrinho (NOLOCK) 
			            LEFT JOIN Local l ON l.IR_LocalID = Carrinho.LocalID
                WHERE (SessionID = '" + SessionID + "' AND ClienteID = " + ClienteID + ") AND (Status = 'R') GROUP BY Estado, ValorTaxaProcessamento";

            try
            {
                decimal total = 0;
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                    while (dr.Read()) total += dr["Valor"].ToDecimal();
                ;


                return total;
            }
            catch
            {
                return 0;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        /// <summary>
        /// Metodo que verifica se existem Ingressos Ou VIR no carrinho do cliente
        /// Se for Reservar VIR, devera verificar se já existem Ingressos e vice-versa, se existirem não permitir,
        /// </summary>
        /// <param name="Reserva"></param>
        /// <returns></returns>
        public int VerificarSeExisteVIRouIngresso(TipoReserva Reserva)
        {
            try
            {
                var tipoIngresso = Reserva == TipoReserva.Ingresso ? "IngressoID IS NULL" : "ValeIngressoTipoID IS NULL";

                var sql = "SELECT Count(ID) FROM Carrinho ( NOLOCK ) WHERE SessionID = '@sessionID' AND (ClienteID = @clienteID OR ClienteID = 0) AND @tipoIngresso AND STATUS = 'R'";
                sql = sql.Replace("@tipoIngresso", tipoIngresso);
                sql = sql.Replace("@sessionID", SessionID);
                sql = sql.Replace("@clienteID", ClienteID.ToString());

                var size = Convert.ToInt32(oDAL.Scalar(sql));

                return size;
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

        public int VerificarSeExisteVIRouIngressoComExpirado(TipoReserva Reserva)
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT Count(ID) FROM Carrinho (NOLOCK) WHERE ");
                stbSQL.Append("SessionID = '" + this.SessionID + "' AND ");
                stbSQL.Append("ClienteID = " + this.ClienteID + " AND ");
                if (Reserva == TipoReserva.ValeIngresso)
                    stbSQL.Append("ValeIngressoTipoID IS NULL ");
                else
                    stbSQL.Append("IngressoID IS NULL ");
                stbSQL.Append("AND (Status = 'R' OR Status = 'E') ");

                int quantidade = Convert.ToInt32(oDAL.Scalar(stbSQL.ToString()));

                return quantidade;
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

        public void MostrarMapaPopup()
        {
            try
            {
                string strSQL = "SELECT top 1 Evento.LocalImagemNome, Codigo From Evento " +
                                "INNER JOIN Carrinho ON Evento.IR_EventoID = Carrinho.EventoID " +
                                "WHERE SessionID = '" + Util.StringToBD(SessionID) + "' AND ClienteID = " + Convert.ToInt32(ClienteID) +
                                " AND Evento.IR_EventoID =" + Convert.ToInt32(this.EventoID) + " AND LugarID = " + Convert.ToInt32(this.LugarID);

                using (IDataReader dr = oDAL.SelectToIDataReader(strSQL))
                {
                    if (dr.Read())
                    {
                        this.localImagemNome = Convert.ToString(dr["LocalImagemNome"]);
                        this.Codigo = Convert.ToString(dr["Codigo"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetStatusCarrinho(int clienteID, string sessionID, string status)
        {
            if (status == "X")
            {
                IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();

                bilheteria.CancelarTodasReservasInternet(clienteID, sessionID);

                oDAL.Execute("UPDATE Carrinho SET Status = 'X' WHERE ClienteID = @ClienteID AND SessionID = @SessionID AND Status = 'R'",
                new System.Data.SqlClient.SqlParameter[] { 
                new System.Data.SqlClient.SqlParameter("@ClienteID", clienteID), 
                new System.Data.SqlClient.SqlParameter("@SessionID", sessionID) });

                return;
            }

            if (status == "VV")
            {
                oDAL.Execute("UPDATE Carrinho SET Status = 'VV' WHERE ClienteID = @ClienteID AND SessionID = @SessionID AND Status = 'R'",
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@ClienteID", clienteID), 
                    new System.Data.SqlClient.SqlParameter("@SessionID", sessionID) });

                return;
            }
            if (status == "V")
            {
                oDAL.Execute("UPDATE Carrinho SET Status = 'V' WHERE ClienteID = @ClienteID AND SessionID = @SessionID AND Status = 'VV'",
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@ClienteID", clienteID), 
                    new System.Data.SqlClient.SqlParameter("@SessionID", sessionID) });

                return;
            }
            if (status == "E") // Expirar Reservas
            {
                oDAL.Execute("UPDATE Carrinho SET Status = 'E' WHERE ClienteID = @ClienteID AND SessionID = @SessionID AND Status = 'R'",
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@ClienteID", clienteID), 
                    new System.Data.SqlClient.SqlParameter("@SessionID", sessionID) });

                return;
            }
            else
            {
                oDAL.Execute("UPDATE Carrinho SET Status = '" + status + "' WHERE ClienteID = @ClienteID AND SessionID = @SessionID AND Status = 'R'",
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@ClienteID", clienteID), 
                    new System.Data.SqlClient.SqlParameter("@SessionID", sessionID) });

                return;
            }
        }

        public void SetStatusCarrinhoVV(int clienteID, string sessionID, string status, long vendaBilheteriaID)
        {
            if (status == "VV")
            {
                oDAL.Execute("UPDATE Carrinho SET Status = 'VV', VendaBilheteriaID = " + vendaBilheteriaID + " WHERE ClienteID = @ClienteID AND SessionID = @SessionID AND Status = 'R'",
                new System.Data.SqlClient.SqlParameter[] { 
                    new System.Data.SqlClient.SqlParameter("@ClienteID", clienteID), 
                    new System.Data.SqlClient.SqlParameter("@SessionID", sessionID) });

                return;
            }
        }

        public bool Delete(int id, int UsuarioID)
        {
            try
            {
                IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();

                bilheteria.CancelarReservasInternet(this.IngressoID, UsuarioID);

                string strSql = string.Empty;
                if (id > 0)
                    strSql = "UPDATE Carrinho SET Status = 'D' WHERE ID = " + id;
                else
                    strSql = string.Format(@"UPDATE Carrinho SET Status = 'D' WHERE IngressoID = {0} AND ClienteID = {1} AND SessionID = '{2}'",
                        this.IngressoID, this.ClienteID, this.SessionID);

                new Pacote().ExcluirPacoteCamisasPorIngresso(this.IngressoID, this.SessionID);

                return oDAL.Execute(strSql) >= 1;
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

        public bool DeletePacote(string pacoteGrupo, string sessionID, int clienteID)
        {
            try
            {
                IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
                string strSQL = "SELECT IngressoID FROM Carrinho (NOLOCK) WHERE PacoteGrupo = '" + pacoteGrupo + "' AND SessionID = '" + sessionID + "' AND ClienteID = " + clienteID;


                List<int> ingressos = new List<int>();
                string ids = string.Empty;
                using (IDataReader reader = oDAL.SelectToIDataReader(strSQL))
                    while (reader.Read())
                    {
                        ids += reader.GetInt32(0) + ",";
                        ingressos.Add(reader.GetInt32(0));
                    }

                oDAL.ConnClose();

                new Pacote().ExcluirPacoteCamisas(pacoteGrupo, sessionID);

                bilheteria.CancelarReservasInternet(ingressos.ToArray(), HttpContext.Current.Session["UsuarioID"].ToInt32());

                if (ids.Length > 0)
                    ids = ids.Substring(0, ids.Length - 1);

                string strSql = "UPDATE Carrinho SET Status = 'D' WHERE ingressoID IN(" + ids + ") AND PacoteGrupo = '" + pacoteGrupo + "' AND ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' AND (Status = '" + (char)IRLib.Ingresso.StatusIngresso.RESERVADO + "' OR Status = 'E')";
                return (oDAL.Execute(strSql) > 0);
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

        public bool DeleteMesa(int lugarID, int apresentacaoID, string sessionID, int clienteID, int UsuarioID)
        {
            try
            {
                //MELHORIA: Para exclusão de reservas deverá ser utilizado apenas um método.
                //Hoje é necessário, pois ao reservar uma Mesa, apenas um registro deveria ser inserido na tabela Carrinho.

                IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
                string strSQL = string.Format(
                            @"SELECT IngressoID FROM Carrinho (NOLOCK)
                            WHERE LugarID = {0} AND ApresentacaoID = {1} AND ClienteID = {2} AND SessionID = '{3}' AND Status = '{4}'",
                    lugarID, apresentacaoID, clienteID, sessionID, (char)IRLib.Ingresso.StatusIngresso.RESERVADO);


                List<int> ingressos = new List<int>();
                string ids = string.Empty;

                using (IDataReader reader = oDAL.SelectToIDataReader(strSQL))
                    while (reader.Read())
                    {
                        ids += reader.GetInt32(0) + ",";
                        ingressos.Add(reader.GetInt32(0));
                    }
                oDAL.ConnClose();

                if (ingressos.Count == 0)
                    throw new Exception("Os ingressos não foram encontrados.");

                if (bilheteria.CancelarReservasInternet(ingressos.ToArray(), UsuarioID) != ingressos.ToArray().Length)
                    return false;

                ids = ids.Substring(0, ids.Length - 1);

                string strSql = string.Format(@"UPDATE Carrinho SET Status = 'D'
                        WHERE IngressoID IN({0}) AND ClienteID = {1} AND SessionID = '{2}'", ids, clienteID, sessionID);
                return (oDAL.Execute(strSql) > 0);
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

        public bool DeleteMesa(int lugarID, string sessionID, int clienteID, int UsuarioID)
        {
            try
            {
                //MELHORIA: Para exclusão de reservas deverá ser utilizado apenas um método.
                //Hoje é necessário, pois ao reservar uma Mesa, apenas um registro deveria ser inserido na tabela Carrinho.

                IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
                string strSQL = "SELECT IngressoID FROM Carrinho (NOLOCK) WHERE LugarID = " + lugarID + " AND SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " AND (Status = '" + (char)IRLib.Ingresso.StatusIngresso.RESERVADO + "')";


                List<int> ingressos = new List<int>();
                string ids = string.Empty;

                using (IDataReader reader = oDAL.SelectToIDataReader(strSQL))
                    while (reader.Read())
                    {
                        ids += reader.GetInt32(0) + ",";
                        ingressos.Add(reader.GetInt32(0));
                    }

                oDAL.ConnClose();


                if (bilheteria.CancelarReservasInternet(ingressos.ToArray(), UsuarioID) == ingressos.ToArray().Length)
                {
                    if (ids.Length > 0)
                        ids = ids.Substring(0, ids.Length - 1);

                    string strSql = "UPDATE Carrinho SET Status = 'D' WHERE ingressoID IN(" + ids + ")";
                    oDAL.Execute(strSql);

                    return true;
                }
                else
                    return false;
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

        public bool Delete(int UsuarioID)
        {
            try
            {
                if (PacoteGrupo != null && PacoteGrupo.Length > 0)
                    return this.DeletePacote(PacoteGrupo, SessionID, ClienteID);
                else
                {
                    string setorTipo = (string)oDAL.Scalar("SELECT TOP 1 TipoLugar FROM Carrinho (NOLOCK) WHERE LugarID = @LugarID AND ApresentacaoID = " + this.ApresentacaoID + " AND SessionID = '" + this.SessionID + "' AND ClienteID = " + this.ClienteID, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@LugarID", this.LugarID) });

                    if (setorTipo == "M") // Se for mesa                    
                        // Exclue toda a mesa.
                        return this.DeleteMesa(this.LugarID, this.SessionID, this.ClienteID, UsuarioID);
                    else
                        return this.Delete(this.ID, UsuarioID); // Exclue apenas uma reserva
                }
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public bool DeleteVIR(int[] valeIngressoID)
        {
            try
            {
                IRLib.ValeIngresso valeIngresso = new IRLib.ValeIngresso();
                valeIngresso.CancelarReservas(valeIngressoID);

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("UPDATE Carrinho SET Status = 'D' WHERE ValeIngressoID = @ValeIngressoID AND ");
                stbSQL.Append("ClienteID = @ClienteID AND SessionID = @SessionID");

                int x = oDAL.Execute(stbSQL.ToString(), new SqlParameter[]
                {
                    new SqlParameter("@ValeIngressoID", valeIngressoID[0]),
                    new SqlParameter("@ClienteID", this.ClienteID),
                    new SqlParameter("@SessionID", this.SessionID)
                });

                if (x > 0)
                    return true;
                else
                    return false;



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

        public bool ExpirarTodasReservas(int clienteID, string sessionID)
        {
            IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
            // cancelar todas reservas no sistema
            if (bilheteria.CancelarTodasReservasInternet(clienteID, sessionID) == 1)
            {
                SetStatusCarrinho(clienteID, sessionID, "E");
                return true;
            }
            else
                return false;
        }

        public bool ExpirarTodasReservasCanais(int clienteID, string sessionID)
        {
            IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
            // cancelar todas reservas no sistema
            if (bilheteria.CancelarTodasReservasInternet(clienteID, sessionID) == 1)
            {
                SetStatusCarrinho(clienteID, sessionID, "X");
                return true;
            }
            else
                return false;
        }

        public bool CancelarTodasReservasVIRporStatus(string Status)
        {
            try
            {
                IRLib.ValeIngresso valeIngresso = new IRLib.ValeIngresso();
                bool ok = valeIngresso.CancelarReservasInternet(this.ClienteID, this.SessionID);
                if (ok)
                {
                    this.SetStatusCarrinho(this.ClienteID, this.SessionID, Status);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal VerificarValorTotal()
        {
            try
            {
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT SUM(PrecoValor) AS ValorTotal FROM Carrinho (NOLOCK) ");
                stbSQL.Append("WHERE ClienteID = @ClienteID AND ");
                stbSQL.Append("SessionID = @SessionID AND ");
                stbSQL.Append("Status = 'R'");

                IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString(), new SqlParameter[]
                
                {
                    new SqlParameter("@ClienteID", this.ClienteID),
                    new SqlParameter("@SessionID", this.SessionID)
                });

                return Convert.ToDecimal(dr["ValorTotal"]);
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

        public DateTime DataMaisProxima(int clienteID, string sessionID)
        {
            object red = oDAL.Scalar("SELECT MIN(ApresentacaoDataHora) FROM Carrinho (NOLOCK) WHERE ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' AND Status = 'R'");//,
            //new System.Data.SqlClient.SqlParameter[] { 
            //  new System.Data.SqlClient.SqlParameter("@ClienteID", clienteID)
            //, new System.Data.SqlClient.SqlParameter("@SessionID", sessionID) });

            DateTime ret;
            try
            {
                ret = DateTime.ParseExact(red.ToString(), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ApplicationException("Data inválida!");
            }
            return ret;
        }

        public static decimal TotalCompra(string sessionID, int clienteID)
        {
            DAL oDal = new DAL();
            string strSql =
                @"SELECT 
		            IsNull(
			            SUM(TaxaConveniencia) + 
			            SUM(PrecoValor), 0) + ValorTaxaProcessamento AS Valor
			            FROM Carrinho (NOLOCK) 
			            LEFT JOIN Local l ON l.IR_LocalID = Carrinho.LocalID
                WHERE (SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + ") AND (Status = 'R') GROUP BY Estado, ValorTaxaProcessamento";

            try
            {
                decimal total = 0;
                using (IDataReader dr = oDal.SelectToIDataReader(strSql))
                    while (dr.Read()) total += dr["Valor"].ToDecimal();
                ;


                return total;
            }
            catch
            {
                return 0;
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public static decimal TotalIngressos(string sessionID, int clienteID)
        {
            DAL oDal = new DAL();
            string strSql =
                @"SELECT 
		            IsNull(SUM(PrecoValor), 0) AS Valor
			        FROM Carrinho (NOLOCK)			            
                WHERE (SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + ") AND (Status = 'R')";

            try
            {
                decimal total = 0;
                using (IDataReader dr = oDal.SelectToIDataReader(strSql))
                    while (dr.Read()) total += dr["Valor"].ToDecimal();
                ;


                return total;
            }
            catch
            {
                return 0;
            }
            finally
            {
                oDal.ConnClose();
            }
        }

        public static decimal TotalConveniencia(string sessionID, int clienteID)
        {
            DAL oDal = new DAL();
            string strSql =
                @"SELECT 
		            IsNull(SUM(TaxaConveniencia), 0) AS Valor
			        FROM Carrinho (NOLOCK)			            
                WHERE (SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + ") AND (Status = 'R')";

            try
            {
                decimal total = 0;
                using (IDataReader dr = oDal.SelectToIDataReader(strSql))
                    while (dr.Read()) total += dr["Valor"].ToDecimal();
                ;


                return total;
            }
            catch
            {
                return 0;
            }
            finally
            {
                oDal.ConnClose();
            }
        }


        public static decimal TotalCompraVIR(string sessionID, int clienteID)
        {
            try
            {
                DAL oDal = new DAL();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT IsNull(SUM(ValorPagamento),0) AS Valor FROM ValeIngressoTipo (NOLOCK) ");
                stbSQL.Append("INNER JOIN Carrinho (NOLOCK) ON ValeIngressoTipo.IR_ValeIngressoTipoID = Carrinho.ValeIngressoTipoID ");
                stbSQL.Append("WHERE Status = 'R' AND ClienteID = @ClienteID AND SessionID = @SessionID");

                decimal total = Convert.ToDecimal(oDal.Scalar(stbSQL.ToString(), new SqlParameter[]
                {
                    new SqlParameter("@ClienteID", clienteID),
                    new SqlParameter("@SessionID", sessionID)
                }));

                return total;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int MaxParcelas(int clienteID, string sessionID)
        {
            DAL oDal = new DAL();
            object x = oDal.Scalar("SELECT MIN(Parcelas) FROM vwCarrinhoEvento WHERE ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' AND Parcelas != 0  ", new System.Data.SqlClient.SqlParameter[] { });
            return x != null ? Convert.ToInt32(x) : 0;
        }

        public void AtualizarPrecoCarrinho(int precoID, int id, string valor, string taxaConv)
        {
            string strSQL = "";

            try
            {
                strSQL = "UPDATE Carrinho SET PrecoID=" + precoID + ", PrecoNome='ClienteVivo', PrecoValor=" + valor.Replace(",", ".") + ", TaxaConveniencia=" + taxaConv.Replace(",", ".") +
                         " WHERE ID=" + id;

                oDAL.Execute(strSQL);

                oDAL.ConnClose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, decimal> IngressosTaxasCarrinho(int clienteID, string sessionID)
        {
            Dictionary<int, decimal> retorno = new Dictionary<int, decimal>();
            //Busca somente os ingressos de pista.
            String strSql = "SELECT IngressoID,TaxaConveniencia " +
                            "FROM Carrinho (NOLOCK) " +
                            "WHERE " +
                            "SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " AND Status = 'R' ORDER BY IngressoID";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        retorno.Add(Convert.ToInt32(dr["IngressoID"]), Convert.ToDecimal(dr["TaxaConveniencia"]));
                    }
                }

                oDAL.ConnClose();
                return retorno;
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

        public List<int> getValeIngressoID(int clienteID, string sessionID)
        {
            try
            {
                List<int> lstVirID = new List<int>();
                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("SELECT ValeIngressoID FROM Carrinho (NOLOCK) ");
                stbSQL.Append("WHERE ClienteID = @ClienteID AND ");
                stbSQL.Append("SessionID = @SessionID AND Status = 'R' ORDER BY ValeIngressoID");

                IDataReader dr = oDAL.SelectToIDataReader(stbSQL.ToString(), new SqlParameter[]
                {
                    new SqlParameter("@ClienteID", clienteID),
                    new SqlParameter("@SessionID", sessionID)
                });

                while (dr.Read())
                    lstVirID.Add(Convert.ToInt32(dr["ValeIngressoID"]));

                return lstVirID;
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

        /// <summary>
        /// Criado por: Caio Maganha Rosa
        /// Data: 05/10/2009
        /// Utilização: Transfere os ingressos reservados após o Login
        /// </summary>
        /// <param name="ClienteID"></param>
        /// <param name="SessionID"></param>
        /// <param name="IngressoID"></param>
        public void TransferirIngressos(EstruturaReservaInternet estrutura)
        {
            try
            {
                int ok = 0;
                IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
                IngressoTransferenciaLista oTransferencia = new IngressoTransferenciaLista();

                oTransferencia.SessionID = this.SessionID;
                oTransferencia.ClienteID = this.ClienteID;

                bool transferir = oTransferencia.VerificarIngressos(true, estrutura);

                if (transferir)
                {
                    //Update na tIngresso
                    ok = bilheteria.TransferirTodasReservasInternet(ClienteID, SessionID);
                    if (ok == 1) //Transferecia feita com sucesso
                        //Update na carrinho
                        this.TransferirCarrinho();
                }
            }
            catch (Exception ex)
            {
                this.SetStatusCarrinho(this.ClienteID, this.SessionID, "X");
                this.SetStatusCarrinho(0, this.SessionID, "X");
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public void TransferirIngressosPaypalExpress(EstruturaReservaInternet estrutura)
        {
            try
            {
                int ok = 0;
                IRLib.Bilheteria bilheteria = new IRLib.Bilheteria();
                IngressoTransferenciaLista oTransferencia = new IngressoTransferenciaLista();

                oTransferencia.SessionID = this.SessionID;
                oTransferencia.ClienteID = this.ClienteID;

                bool transferir = oTransferencia.VerificarIngressos(true, estrutura);

                if (transferir)
                {
                    //Update na tIngresso
                    ok = bilheteria.TransferirTodasReservasPaypalExpress(ClienteID, SessionID);
                    if (ok == 1) //Transferecia feita com sucesso
                        //Update na carrinho
                        this.TransferirCarrinho();
                }
            }
            catch (Exception ex)
            {
                this.SetStatusCarrinho(this.ClienteID, this.SessionID, "X");
                this.SetStatusCarrinho(0, this.SessionID, "X");
                throw ex;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public void TransferirValeIngressos()
        {
            try
            {
                IRLib.Bilheteria Bilheteria = new IRLib.Bilheteria();
                bool ok = Bilheteria.TransferirTodosVIRInternet(this.ClienteID, this.SessionID);

                if (ok)
                    //Update na Carrinho
                    this.TransferirCarrinho();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool TransferirCarrinho()
        {
            try
            {
                System.Data.SqlClient.SqlParameter[] Parametros = new System.Data.SqlClient.SqlParameter[2];
                Parametros[0] = new System.Data.SqlClient.SqlParameter("@ClienteID", this.ClienteID);
                Parametros[1] = new System.Data.SqlClient.SqlParameter("@SessionID", this.SessionID);

                StringBuilder stbSQL = new StringBuilder();
                stbSQL.Append("UPDATE Carrinho ");
                stbSQL.Append("SET ClienteID = @ClienteID ");
                stbSQL.Append("WHERE ClienteID = 0 ");
                stbSQL.Append("AND SessionID = @SessionID ");
                stbSQL.Append("AND Status = 'R' ");

                int ok = oDAL.Execute(stbSQL.ToString(), Parametros);
                if (ok > 0)
                    return true;
                else
                    return false;
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

        public int TempoReserva(int clienteID, string sessionID)
        {
            int segundos = 0;

            DateTime timeStamp = new DateTime();

            DateTime tempoReserva = new DateTime();

            String strSql = "SELECT ISNULL(max(TimeStamp),0) AS TimeStamp  " +
                            "FROM Carrinho (NOLOCK) " +
                            "WHERE " +
                            "SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " AND Status = 'R'";
            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        if (Convert.ToDouble(dr["TimeStamp"]) > 0)
                        {
                            timeStamp = DateTime.ParseExact(dr["TimeStamp"].ToString(), "yyyyMMddHHmmss", Config.CulturaPadrao); ;
                        }
                    }
                }
                if (timeStamp > DateTime.MinValue)
                {
                    tempoReserva = timeStamp.AddMinutes(int.Parse(ConfigurationManager.AppSettings["TempoCarrinho"]));

                    if (tempoReserva > DateTime.Now)
                    {
                        TimeSpan teste = tempoReserva - DateTime.Now;
                        segundos = (int)teste.TotalSeconds;
                    }
                }

                return segundos;
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

    }
}