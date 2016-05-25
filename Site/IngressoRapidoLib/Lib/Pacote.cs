using CTLib;
using System;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace IngressoRapido.Lib
{
    [DataContract]
    public class Pacote
    {
        public enum TipoPacote
        {
            Assinatura = 'N',
            Pista = 'T',
        }

        public Pacote()
        {

        }

        public Pacote(int id)
        {
            this.id = id;
        }

        private DAL oDAL = new DAL();

        private int id;
        public int ID
        {
            get { return id; }
        }

        [DataMember]
        public string Nome { get; set; }

        public decimal Preco { get; set; }

        public int NomenclaturaPacoteID { get; set; }

        public decimal Valor
        {
            get
            {
                return this.PacoteItemLista.Sum(c => c.Valor);
            }
        }

        public string Tipo { get; set; }

        [DataMember]
        public string ValorFormatado
        {
            get
            {
                return this.PacoteItemLista.Sum(c => c.Valor).ToString("c");
            }
        }

        public int Quantidade
        {
            get
            {
                return this.pacoteItemLista.Count;
            }
        }

        private PacoteItemLista pacoteItemLista = new PacoteItemLista();

        [DataMember]
        public PacoteItemLista PacoteItemLista
        {
            get { return pacoteItemLista; }
            set { pacoteItemLista = value; }
        }

        public Pacote GetByID(int id)
        {
            //string strSql = "SELECT IR_PacoteID, Nome FROM Pacote " +
            //                "WHERE (IR_PacoteID = " + id + ")";

            string strSql = "SELECT P.IR_PacoteID,P.NomenclaturaPacoteID, P.Nome, SUM(Valor * Quantidade) AS Total FROM PacoteItem, Preco, Pacote P " +
                            "WHERE PacoteID = IR_PacoteID AND PrecoID = IR_PrecoID AND PacoteID = " + id + " " +
                            "GROUP BY P.IR_PacoteID, P.Nome";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    if (dr.Read())
                    {
                        this.id = Convert.ToInt32(dr["IR_PacoteID"].ToString());
                        this.Nome = Util.LimparTitulo(dr["Nome"].ToString()).Trim();
                        //this.Preco = Convert.ToDecimal(dr["Total"].ToString());
                        this.Preco = (decimal)dr["Total"];
                        this.NomenclaturaPacoteID = Convert.ToInt32(dr["NomenclaturaPacoteID"].ToString());
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

        public static string GerarOptions(int Quantidade)
        {
            StringBuilder stb = new StringBuilder();
            stb.AppendFormat("<option value='{0}' selected='selected'>{1}</option>", 0, 0);
            for (int i = 1; i < Quantidade; i++)
                stb.AppendFormat("<option value='{0}'>{1}</option>", i, i);

            return stb.ToString();
        }

        public string MontarHTMLCampo(bool incluirEnabled, string backGroundPath)
        {
            StringBuilder stb = new StringBuilder();

            switch (this.Tipo)
            {
                case "C":
                    //Procura se o setor tem imagem
                    System.Drawing.Image img = Setor.ProcurarImagemThumbNail(this.PacoteItemLista.FirstOrDefault().SetorID, backGroundPath);
                    bool temImagem = img != null;
                    stb.AppendFormat("<div id=\"infoAssinatura_{0}\" style=\"display: none;\">", this.ID);
                    stb.Append("{");
                    stb.AppendFormat("'PacoteID':{0}, ", this.ID);
                    stb.AppendFormat("'PacoteNome':'{0}', ", this.Nome);
                    stb.AppendFormat("'SetorID':{0}, ", this.PacoteItemLista.FirstOrDefault().SetorID);
                    stb.AppendFormat("'Background':'{0}', ", (temImagem ? this.PacoteItemLista.FirstOrDefault().SetorID.ToString("000000") : "0"));
                    stb.AppendFormat("'BackgroundWidth':{0}, ", (temImagem ? img.Width : 0));
                    stb.AppendFormat("'BackgroundHeight':{0} ", (temImagem ? img.Height : 0));
                    stb.Append("}</div>");

                    stb.Append("<li style=\"width: 100%; text-align: left; padding-left: 25px; padding-top: 5px;\">");
                    if (incluirEnabled)
                    {
                        stb.AppendFormat("<a id=\"assinatura_{0}\" class=\"ReservarAssinaturas\" style=\"cursor: pointer;\">", this.ID);
                        stb.AppendFormat("<img src='{0}' alt='{1}'/>", "../Images/Lugares/Cad_Livre.gif", "Escolher Lugar");
                        stb.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + this.Nome + " - R$ " + this.Valor);
                        stb.Append("</a>");
                    }
                    else
                        stb.Append(this.Nome + " - R$ " + this.Valor);
                    stb.Append("</li>");
                    break;
                case "P":
                    if (!incluirEnabled)
                        stb.AppendFormat("<li style=\"padding-left: 15px; padding-top: 5px; text-align: left\"> {0} - R$ {1} </li>",
                            this.Nome, this.Valor);
                    else
                        stb.AppendFormat("<li id=\"li_pacote{0}\" class=\"QuantidadePacote\" style=\"padding-left: 15px; text-align: left\"><select id=\"{0}\" class=\"SelectQuantidadePacote ColorAzulEscuro\" > {1} </select> {2} - R$ {3} ",
                            this.ID, Pacote.GerarOptions(5), this.Nome, this.Valor);
                    break;
            }
            return stb.ToString();
        }

        public IRLib.ClientObjects.EstruturaTaxaProcessamentoPacote TaxaProcessamento(int pacoteID)
        {
            try
            {
                var estrutura = new IRLib.ClientObjects.EstruturaTaxaProcessamentoPacote() { LimiteIngressosEstado = 999, LimiteIngressosEvento = 999 };

                string sql = @"SELECT
	                   DISTINCT e.PossuiTaxaProcessamento, e.LimiteMaximoIngressosEvento, e.LimiteMaximoIngressosEstado, l.Estado
	                FROM Pacote p (NOLOCK)
	                INNER JOIN PacoteItem pi (NOLOCK) ON pi.PacoteID = p.IR_PacoteID
	                INNER JOIN Evento e (NOLOCK) ON pi.EventoID = e.IR_EventoID
                    INNER JOIN Local l (NOLOCK) ON e.LocalID = l.IR_LocalID
                    WHERE p.IR_PacoteID = " + pacoteID;

                using (IDataReader dr = oDAL.SelectToIDataReader(sql))
                {
                    while (dr.Read())
                    {
                        estrutura.PossuiTaxaProcessamento |= dr["PossuiTaxaProcessamento"].ToBoolean();
                        estrutura.LimiteIngressosEvento = Math.Min(estrutura.LimiteIngressosEvento, dr["LimiteMaximoIngressosEvento"].ToInt32());
                        estrutura.LimiteIngressosEstado = Math.Min(estrutura.LimiteIngressosEstado, dr["LimiteMaximoIngressosEstado"].ToInt32());
                        estrutura.Estado = dr["Estado"].ToString();
                    }
                }

                return estrutura;
            }
            finally
            {
                oDAL.ConnClose();
            }
        }

        public bool InserirPacoteCamisas(string PacoteGrupo, int ClienteID, string SessionID, int QuantidadeP, int QuantidadeM, int QuantidadeG, int QuantidadeGG)
        {
            try
            {
                bool result = false;

                string sql = string.Format(@"INSERT INTO tPacoteTemporario
                            (PacoteGrupo, IngressoID, ClienteID, SessionID, Senha, QuantidadeP, QuantidadeM, QuantidadeG, QuantidadeGG, TimeStamp)
                            VALUES('{0}', {1}, {2}, '{3}', '{4}', {5}, {6}, {7}, {8}, {9})", PacoteGrupo, 0, ClienteID, SessionID, string.Empty, QuantidadeP, QuantidadeM, QuantidadeG, QuantidadeGG, DateTime.Now.ToString("yyyyMMddHHmmss"));

                int x = oDAL.Execute(sql.ToString());

                result = (x == 1);

                if (!result)
                    throw new Exception("Erro ao gravar lista de camisas.");

                return result;
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

        public bool InserirPacoteCamisas(string PacoteGrupo, int ingressoID, int ClienteID, string SessionID, int QuantidadeP, int QuantidadeM, int QuantidadeG, int QuantidadeGG)
        {
            try
            {
                bool result = false;

                string sql = string.Format(@"INSERT INTO tPacoteTemporario
                            (PacoteGrupo, IngressoID, ClienteID, SessionID, Senha, QuantidadeP, QuantidadeM, QuantidadeG, QuantidadeGG, TimeStamp)
                            VALUES('{0}', {1}, {2}, '{3}', '{4}', {5}, {6}, {7}, {8}, {9})", PacoteGrupo, ingressoID, ClienteID, SessionID, string.Empty, QuantidadeP, QuantidadeM, QuantidadeG, QuantidadeGG, DateTime.Now.ToString("yyyyMMddHHmmss"));

                int x = oDAL.Execute(sql.ToString());

                result = (x == 1);

                if (!result)
                    throw new Exception("Erro ao gravar lista de camisas.");

                return result;
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

        public void ExcluirPacoteCamisas(string PacoteGrupo, string SessionID)
        {
            try
            {
                string sqlSelect = string.Format("Select PacoteGrupo FROM tPacoteTemporario (NOLOCK) WHERE PacoteGrupo = '{0}' AND SessionID = '{1}'", PacoteGrupo, SessionID);
                int x = 0;

                using (IDataReader dr = oDAL.SelectToIDataReader(sqlSelect))
                    if (dr.Read())
                        x = 1;

                if (x == 1)
                {
                    x = 0;
                    string sqlDelete = string.Format(@"DELETE FROM tPacoteTemporario WHERE PacoteGrupo = '{0}' AND SessionID = '{1}'", PacoteGrupo, SessionID);

                    x = oDAL.Execute(sqlDelete);

                    if (x != 1)
                        throw new Exception("Erro ao excluir lista de camisas.");
                }
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

        public void ExcluirPacoteCamisasPorIngresso(int IngressoID, string SessionID)
        {
            try
            {
                string sqlSelect = string.Format("Select IngressoID FROM tPacoteTemporario (NOLOCK) WHERE SessionID = '{0}' AND IngressoID = {1}", SessionID, IngressoID);
                int x = 0;

                using (IDataReader dr = oDAL.SelectToIDataReader(sqlSelect))
                    if (dr.Read())
                        x = 1;

                if (x == 1)
                {
                    x = 0;
                    string sqlDelete = string.Format(@"DELETE FROM tPacoteTemporario WHERE IngressoID = {0} AND SessionID = '{1}'", IngressoID, SessionID);

                    x = oDAL.Execute(sqlDelete);

                    if (x != 1)
                        throw new Exception("Erro ao excluir lista de camisas.");
                }
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

        public void AtualizarPacoteCamisas(int ClienteID, string SessionID, string Senha)
        {
            try
            {
                string sqlUpdate = string.Format(@"UPDATE tPacoteTemporario Set Senha = '{0}', ClienteID = {1} Where SessionID = '{2}'", Senha, ClienteID, SessionID);

                oDAL.Execute(sqlUpdate);
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
    }
}
