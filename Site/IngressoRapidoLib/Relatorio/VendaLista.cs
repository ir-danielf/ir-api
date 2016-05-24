using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
namespace IngressoRapido.Relatorio
{
    public class VendaLista : List<Venda>
    {
        public static readonly string serverName = ConfigurationManager.AppSettings["serverName"];
        DAL oDAL = new DAL();

        Venda oVenda;

        public VendaLista()
        {
            this.Clear();
        }

        public VendaLista CarregarClienteVisanet(string clausula)
        {
            string strSql = string.Empty;

            strSql = "SELECT c.ID, c.Nome, c.Email, c.DDDTelefone, c.Telefone, c.DDDCelular, c.Celular, v.Senha, " +
                     "v.tid, v.TimeStamp, v.pan, v.ars,  v.ars_captura, v.lr_captura, v.price, v.lr, v.arp, v.Parcelas " +
                     "FROM Ingressos.dbo.tCliente c, Visanet v WHERE c.ID = v.ClienteID " + clausula + " ORDER BY v.ID Desc";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oVenda = new Venda(Convert.ToInt32(dr["ID"].ToString()));
                        oVenda.Nome = dr["Nome"].ToString();
                        oVenda.Email = dr["Email"].ToString();
                        if (dr["Telefone"].ToString() != "")
                            oVenda.Telefone = "(" + dr["DDDTelefone"].ToString() + ")" + dr["Telefone"].ToString();
                        else
                            oVenda.Telefone = "";
                        if (dr["Celular"].ToString() != "")
                            oVenda.TelefoneCel = "(" + dr["DDDCelular"].ToString() + ")" + dr["Celular"].ToString();
                        else
                            oVenda.TelefoneCel = "";
                        oVenda.Senha = dr["Senha"].ToString();
                        oVenda.TID = dr["tid"].ToString();
                        oVenda.TimeStamp = dr["TimeStamp"].ToString().Substring(6, 2) + "/" + dr["TimeStamp"].ToString().Substring(4, 2) + "/" + dr["TimeStamp"].ToString().Substring(0, 4) + " " + dr["TimeStamp"].ToString().Substring(8, 2) + ":" + dr["TimeStamp"].ToString().Substring(10, 2);
                        oVenda.NumeroCartao = dr["pan"].ToString();
                        
                        if (dr["price"] != null && dr["price"].ToString() != "")
                            oVenda.Total = Convert.ToDecimal(dr["price"].ToString());
                        if (dr["lr"] != null && dr["lr"].ToString() != "")
                            oVenda.Status = dr["lr"].ToString();

                        oVenda.MsgRetornoAutenticacao = dr["lr"].ToString() != "0" ? dr["ars"].ToString() : "";
                        oVenda.MsgRetornoCaptura = dr["ars_captura"].ToString();
                        oVenda.ARP = dr["arp"].ToString();
                        
                        if (dr["Parcelas"] != null && dr["Parcelas"].ToString() != "")
                            oVenda.Parcelas = int.Parse(dr["Parcelas"].ToString());

                        this.Add(oVenda);
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

        public VendaLista CarregarClienteRedecard(string clausula)
        {
            string strSql = string.Empty;

            strSql = "SELECT c.ID, c.Nome, c.Email, c.DDDTelefone, c.Telefone, c.DDDCelular, c.Celular, r.Senha, " + 
                     "r.tid, r.TimeStamp, r.NR_CARTAO, r.MSGRET, r.MSGRET_confirmacao, r.total, r.CODRET, r.NUMAUTOR, r.NUMCV, r.Parcelas " +
                     "FROM Ingressos.dbo.tCliente c, Redecard r WHERE c.ID = r.ClienteID" + clausula + " ORDER BY r.ID Desc";

            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oVenda = new Venda(Convert.ToInt32(dr["ID"].ToString()));
                        oVenda.Nome = dr["Nome"].ToString();
                        oVenda.Email = dr["Email"].ToString();
                        if (dr["Telefone"].ToString() != "")
                            oVenda.Telefone = "(" + dr["DDDTelefone"].ToString() + ")" + dr["Telefone"].ToString();
                        else
                            oVenda.Telefone = "";
                        if (dr["Celular"].ToString() != "")
                            oVenda.TelefoneCel = "(" + dr["DDDCelular"].ToString() + ")" + dr["Celular"].ToString();
                        else
                            oVenda.TelefoneCel = "";
                        oVenda.Senha = dr["Senha"].ToString();
                        oVenda.TID = dr["tid"].ToString();
                        oVenda.TimeStamp = dr["TimeStamp"].ToString().Substring(6, 2) + "/" + dr["TimeStamp"].ToString().Substring(4, 2) + "/" + dr["TimeStamp"].ToString().Substring(0, 4) + " " + dr["TimeStamp"].ToString().Substring(8, 2) + ":" + dr["TimeStamp"].ToString().Substring(10, 2);
                        oVenda.NumeroCartao = dr["NR_CARTAO"].ToString();
                        oVenda.MsgRetornoAutenticacao = dr["MSGRET"].ToString();
                        oVenda.MsgRetornoCaptura = dr["MSGRET_confirmacao"].ToString();
                        if (dr["total"] != null && dr["total"].ToString() != "")
                            oVenda.Total = Convert.ToDecimal(dr["total"].ToString());
                        if (dr["CODRET"] != null && dr["CODRET"].ToString() != "")
                            oVenda.Status =dr["CODRET"].ToString();

                        oVenda.NumAutorizacao = dr["NUMAUTOR"].ToString();
                        oVenda.CV = dr["NUMCV"].ToString();

                        if (dr["Parcelas"] != null && dr["Parcelas"].ToString() != "")
                            oVenda.Parcelas = int.Parse(dr["Parcelas"].ToString());

                        this.Add(oVenda);
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

        public VendaLista CarregarClienteAmex(string clausula)
        {
            string strSql = string.Empty;

            strSql = "SELECT c.ID, c.Nome, c.Email, c.DDDTelefone, c.Telefone, c.DDDCelular, c.Celular, op.Senha, " +
                     "op.tid, op.TimeStamp, op.Total, op.Parcelas, op.Bin, op.vpc_TxnResponseCode, cap_AuthorizeID, vpc_Message, op.cap_TxnResponseCode, cap_Message " +
                     "FROM Ingressos.dbo.tCliente c, Amex op " +
                     "WHERE c.ID = op.ClienteID " + clausula + " ORDER BY op.ID Desc ";
    


            try
            {
                using (IDataReader dr = oDAL.SelectToIDataReader(strSql))
                {
                    while (dr.Read())
                    {
                        oVenda = new Venda(Convert.ToInt32(dr["ID"].ToString()));
                        oVenda.Nome = dr["Nome"].ToString();
                        oVenda.Email = dr["Email"].ToString();
                        if (dr["Telefone"].ToString() != "")
                            oVenda.Telefone = "(" + dr["DDDTelefone"].ToString() + ")" + dr["Telefone"].ToString();
                        else
                            oVenda.Telefone = "";
                        if (dr["Celular"].ToString() != "")
                            oVenda.TelefoneCel = "(" + dr["DDDCelular"].ToString() + ")" + dr["Celular"].ToString();
                        else
                            oVenda.TelefoneCel = "";
                        
                        oVenda.Senha = dr["Senha"].ToString();
                        oVenda.TID = dr["tid"].ToString();
                        oVenda.TimeStamp = dr["TimeStamp"].ToString().Substring(6, 2) + "/" + dr["TimeStamp"].ToString().Substring(4, 2) + "/" + dr["TimeStamp"].ToString().Substring(0, 4) + " " + dr["TimeStamp"].ToString().Substring(8, 2) + ":" + dr["TimeStamp"].ToString().Substring(10, 2);
                        oVenda.NumeroCartao = dr["Bin"].ToString();

                        if (dr["Total"] != null && dr["Total"].ToString() != "")
                            oVenda.Total = Convert.ToDecimal(dr["Total"].ToString());
                        if (dr["vpc_TxnResponseCode"] != null && dr["vpc_TxnResponseCode"].ToString() != "")
                            oVenda.Status = dr["vpc_TxnResponseCode"].ToString();

                        oVenda.MsgRetornoAutenticacao = dr["vpc_Message"].ToString();
                        oVenda.MsgRetornoCaptura = dr["cap_Message"].ToString();                        

                        if (dr["Parcelas"] != null && dr["Parcelas"].ToString() != "")
                            oVenda.Parcelas = int.Parse(dr["Parcelas"].ToString());

                        oVenda.NumAutorizacao = dr["cap_AuthorizeID"].ToString();

                        this.Add(oVenda);
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

        public VendaLista CarregarClienteHSBC(string clausula)
        {
            StringBuilder stbConsulta = new StringBuilder();
            IDataReader dr = null;
            DAL oDAL = new DAL();
            try
            {
                stbConsulta.Append("SELECT c.ID, c.Nome, c.Email, c.DDDTelefone, c.Telefone, c.DDDCelular, c.Celular,  ");
                stbConsulta.Append("isNull(h.Senha,'----') AS Senha,h.StatusCompra, h.NumeroPedido, h.FormaPagamentoID, h.DataValidacao AS TimeStamp, h.Token, h.TokenTemporario, h.ValorTotal AS Total, h.HSBCMensagem2 ");
                stbConsulta.Append("FROM Ingressos.dbo.tCliente c, HSBC h WHERE c.ID = h.ClienteID " + clausula + " ORDER BY c.ID DESC");

                dr = oDAL.SelectToIDataReader(stbConsulta.ToString());

                while (dr.Read())
                {
                    oVenda = new Venda(Convert.ToInt32(dr["ID"].ToString()));
                    oVenda.Nome = dr["Nome"].ToString();
                    oVenda.Email = dr["Email"].ToString();
                    if (dr["Telefone"].ToString() != string.Empty)
                        oVenda.Telefone = "(" + dr["DDDTelefone"].ToString() + ")" + dr["Telefone"].ToString();
                    else
                        oVenda.Telefone = "";
                    if (dr["Celular"].ToString() != string.Empty)
                        oVenda.TelefoneCel = "(" + dr["DDDCelular"].ToString() + ")" + dr["Celular"].ToString();
                    else
                        oVenda.TelefoneCel = string.Empty;
                    oVenda.Senha = dr["Senha"].ToString();
                    oVenda.TimeStamp = dr["TimeStamp"].ToString().Substring(6, 2) + "/" + dr["TimeStamp"].ToString().Substring(4, 2) + "/" + dr["TimeStamp"].ToString().Substring(0, 4) + " " + dr["TimeStamp"].ToString().Substring(8, 2) + ":" + dr["TimeStamp"].ToString().Substring(10, 2);

                    if (dr["FormaPagamentoID"].ToString() == "112")
                        oVenda.Parcelas = 2;
                    else
                        oVenda.Parcelas = 1;
                    oVenda.Status = dr["StatusCompra"].ToString();
                    //oVenda.Token = dr["Token"].ToString();
                    //oVenda.TokenTemporario = dr["TokenTemporario"].ToString();
                    oVenda.Total = Convert.ToDecimal(dr["Total"].ToString());
                    //oVenda.HSBCNome = dr["HSBCNome"].ToString();
                    //oVenda.HSBCMensagem1 = dr["MsgRetornoAutenticacao"].ToString();
                    oVenda.MsgRetornoCaptura = dr["StatusCompra"].ToString();
                    oVenda.NumAutorizacao = dr["NumeroPedido"].ToString();

                    this.Add(oVenda);
                }
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
    }
}
