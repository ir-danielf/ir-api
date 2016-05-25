using CTLib;
using IngressoRapido.Lib;
using System;
using System.Data;


namespace IngressoRapido.Ecommerce
{
    public class Controle
    {
        public Controle() { }

        DAL oDal = new DAL();

        public const int FP_VISANET = 2;
        public const int FP_REDECARD = 3;
        public const int FP_AMEX = 5;
        public const int FP_HSBC = 6;
        public const int FP_ITAU = 7;
        public const int FP_AURA = 8;
        public const int FP_HIPER = 9;
        public const int FP_PAYPAL = 10;
        public const int FP_VALECULTURA = 11;
        public const int FP_ELO = 12;
        public const int FP_ELOCULTURA = 13;
        public const int FP_SMILES = 14;

        public enum Status
        {
            Negada,
            Problemas,
            Autorizada
        }

        private int taxaEntregaID;
        public int TaxaEntregaID
        {
            get { return this.taxaEntregaID; }
            set { this.taxaEntregaID = value; }
        }

        private decimal entregaValor;
        public decimal EntregaValor
        {
            get { return this.entregaValor; }
            set { this.entregaValor = value; }
        }

        public int EnderecoID { get; set; }
        public int PDVSelecionado { get; set; }
        public string DataSelecionada { get; set; }

        private Status statusPagamento;
        public Status StatusPagamento
        {
            get { return statusPagamento; }
            set { statusPagamento = value; }
        }

        private string tid = string.Empty;
        public string TID
        {
            get { return tid; }
            set { tid = value; }
        }

        private string bin;
        public string BIN
        {
            get { return bin; }
            set { bin = value; }
        }

        private int codigoRetorno;
        public int CodigoRetorno
        {
            get { return codigoRetorno; }
            set { codigoRetorno = value; }
        }

        private int lr;
        public int LR
        {
            get { return lr; }
            set { lr = value; }
        }

        private string lrMsg;
        public string LRMsg
        {
            get { return lrMsg; }
            set { lrMsg = value; }
        }

        private bool autorizado = false;
        public bool Autorizado
        {
            get { return this.statusPagamento == Status.Autorizada; }
        }

        private decimal total = 0;

        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        /// <summary>
        /// Codigo Autorizacao
        /// Redecard:   NUMCV
        /// Visanet:    ARP
        /// </summary>
        private string codigoAutorizacao = String.Empty;
        public string CodigoAutorizacao
        {
            get { return codigoAutorizacao; }
            set { codigoAutorizacao = value; }
        }

        private int parcelas = 0;
        public int Parcelas
        {
            get { return parcelas; }
            set { parcelas = value; }
        }

        private string amexTransactionNo = string.Empty;
        public string AmexTransactionNo
        {
            get { return amexTransactionNo; }
            set { amexTransactionNo = value; }
        }

        public void InfoVendaVisaNet(int clienteID, string sessionID, bool entregaGratuita)
        {
            IDataReader reader = null;
            try
            {
                reader = oDal.SelectToIDataReader("SELECT TOP 1 LR, ARS, TID, TaxaEntregaID, EntregaValor, EnderecoID, PDVSelecionado, DataSelecionada, ARP, Price, Parcelas FROM VisaNet WHERE SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " ORDER BY ID DESC");

                if (!reader.Read())
                    throw new ApplicationException("Informações do pagamento não encontradas.");
                else
                {
                    this.LR = int.Parse(reader["LR"].ToString());
                    this.LRMsg = reader["ARS"].ToString();
                    this.TID = reader["TID"].ToString();
                    this.TaxaEntregaID = (int)reader["TaxaEntregaID"];

                    this.EntregaValor = (int)reader["EntregaValor"];
                    this.EnderecoID = (int)reader["EnderecoID"];
                    this.PDVSelecionado = (int)reader["PDVSelecionado"];
                    this.DataSelecionada = (string)reader["DataSelecionada"];


                    this.Parcelas = (int)reader["Parcelas"];
                    this.Total = (Decimal)reader["Price"];

                    switch (LR)
                    {
                        case 0:
                        case 11:
                            this.statusPagamento = Status.Autorizada;
                            this.CodigoAutorizacao = reader["ARP"].ToString();
                            break;

                        case 6:
                            this.statusPagamento = Status.Problemas;
                            break;

                        default:
                            this.statusPagamento = Status.Negada;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                oDal.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                oDal.ConnClose();
            }
        }

        public void InfoVendaVisaNet(int clienteID, string estado, string sessionID, bool entregaGratuita)
        {
            IDataReader reader = null;
            try
            {
                reader = oDal.SelectToIDataReader("SELECT TOP 1 LR, ARS, TID, TaxaEntregaID,EntregaValor, EnderecoID, PDVSelecionado, DataSelecionada, ARP, Price, Parcelas FROM VisaNet WHERE SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " ORDER BY ID DESC");

                if (!reader.Read())
                    throw new ApplicationException("Informações do pagamento não encontradas.");
                else
                {
                    this.LR = int.Parse(reader["LR"].ToString());
                    this.LRMsg = reader["ARS"].ToString();
                    this.TID = reader["TID"].ToString();
                    this.TaxaEntregaID = (int)reader["TaxaEntregaID"];
                    this.Parcelas = (int)reader["Parcelas"];
                    this.Total = (Decimal)reader["Price"];

                    this.EntregaValor = (int)reader["EntregaValor"];
                    this.EnderecoID = (int)reader["EnderecoID"];
                    this.PDVSelecionado = (int)reader["PDVSelecionado"];
                    this.DataSelecionada = (string)reader["DataSelecionada"];

                    switch (LR)
                    {
                        case 0:
                        case 11:
                            this.statusPagamento = Status.Autorizada;
                            this.CodigoAutorizacao = reader["ARP"].ToString();
                            break;

                        case 6:
                            this.statusPagamento = Status.Problemas;
                            break;

                        default:
                            this.statusPagamento = Status.Negada;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                oDal.ConnClose();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                oDal.ConnClose();
            }
        }

        public void InfoVendaRedeCard(int clienteID, string sessionID, string bin, bool entregaGratuita)
        {
            IDataReader reader = null;
            try
            {
                reader = oDal.SelectToIDataReader("SELECT TOP 1 CODRET, TID, TaxaEntregaID, NR_CARTAO, NUMCV, Total, Parcelas FROM Redecard WHERE SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " ORDER BY ID DESC");

                if (!reader.Read())
                    throw new ApplicationException("Informações do pagamento não encontradas.");
                else
                {
                    this.BIN = reader["NR_CARTAO"].ToString().Substring(0, 6);

                    if (bin != "" && bin != BIN)
                    {
                        this.statusPagamento = Status.Negada;
                        return;
                    }

                    if (reader["CODRET"] != null)
                        this.CodigoRetorno = int.Parse(reader["CODRET"].ToString());
                    else
                        throw new ApplicationException("Erro na Transação");

                    this.TID = reader["TID"].ToString();
                    this.Parcelas = (int)reader["Parcelas"];
                    this.Total = (Decimal)reader["Total"];

                    this.TaxaEntregaID = int.Parse(reader["TaxaEntregaID"].ToString());

                    TaxaEntrega oTaxaEntrega = new TaxaEntrega();
                    oTaxaEntrega.GetByID(this.TaxaEntregaID);
                    this.EntregaValor = oTaxaEntrega.Valor;

                    if (CodigoRetorno > -1 && CodigoRetorno < 50)
                    {
                        this.statusPagamento = Status.Autorizada;
                        this.CodigoAutorizacao = reader["NUMCV"].ToString();
                    }
                    else
                        this.statusPagamento = Status.Negada;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                oDal.ConnClose();
            }
        }

        public void InfoVendaRedeCard(int clienteID, string estado, string sessionID, string bin, bool entregaGratuita)
        {
            IDataReader reader = null;
            try
            {
                reader = oDal.SelectToIDataReader("SELECT TOP 1 CODRET, TID, TaxaEntregaID, NR_CARTAO, NUMCV, Total, Parcelas FROM Redecard WHERE SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " ORDER BY ID DESC");

                if (!reader.Read())
                    throw new ApplicationException("Informações do pagamento não encontradas.");
                else
                {
                    this.BIN = reader["NR_CARTAO"].ToString().Substring(0, 6);

                    if (bin != "" && bin != BIN)
                    {
                        this.statusPagamento = Status.Negada;
                        return;
                    }

                    if (reader["CODRET"] != null)
                        this.CodigoRetorno = int.Parse(reader["CODRET"].ToString());
                    else
                        throw new ApplicationException("Erro na Transação");

                    this.TID = reader["TID"].ToString();
                    this.Parcelas = (int)reader["Parcelas"];
                    this.Total = (Decimal)reader["Total"];

                    this.TaxaEntregaID = int.Parse(reader["TaxaEntregaID"].ToString());

                    TaxaEntrega oTaxaEntrega = new TaxaEntrega();
                    oTaxaEntrega.TaxaCompra(clienteID, estado.ToUpper(), sessionID, entregaGratuita, this.TaxaEntregaID);
                    this.EntregaValor = oTaxaEntrega.Valor;

                    if (CodigoRetorno > -1 && CodigoRetorno < 50)
                    {
                        this.statusPagamento = Status.Autorizada;
                        this.CodigoAutorizacao = reader["NUMCV"].ToString();
                    }
                    else
                        this.statusPagamento = Status.Negada;



                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                oDal.ConnClose();
            }
        }

        public void InfoVendaAmex(int clienteID, string sessionID, string bin)
        {
            IDataReader reader = null;
            try
            {
                reader = oDal.SelectToIDataReader("SELECT TOP 1 vpc_TxnResponseCode, TID, TaxaEntregaID, vpc_AuthorizeID, vpc_TransactionNo, Total, Parcelas, BIN FROM Amex WHERE SessionID = '" + sessionID + "' AND ClienteID = " + clienteID + " ORDER BY ID DESC");

                if (!reader.Read())
                    throw new ApplicationException("Informações do pagamento não encontradas.");
                else
                {
                    this.BIN = reader["BIN"].ToString();

                    if (bin != "" && bin != BIN)
                    {
                        this.statusPagamento = Status.Negada;
                        return;
                    }


                    if (reader["vpc_TxnResponseCode"] != null)
                        this.CodigoRetorno = int.Parse(reader["vpc_TxnResponseCode"].ToString());
                    else
                        throw new ApplicationException("Erro na Transação");

                    this.TID = reader["TID"].ToString();
                    this.Parcelas = (int)reader["Parcelas"];
                    this.Total = (Decimal)reader["Total"];
                    this.TaxaEntregaID = int.Parse(reader["TaxaEntregaID"].ToString());

                    TaxaEntrega oTaxaEntrega = new TaxaEntrega();
                    oTaxaEntrega.GetByID(this.TaxaEntregaID);
                    this.EntregaValor = oTaxaEntrega.Valor;


                    if (CodigoRetorno == 0)
                    {
                        this.statusPagamento = Status.Autorizada;
                        this.CodigoAutorizacao = reader["vpc_AuthorizeID"].ToString();
                        this.AmexTransactionNo = reader["vpc_TransactionNo"].ToString();
                    }
                    else
                        this.statusPagamento = Status.Negada;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                oDal.ConnClose();
            }

        }

        public void InserirSenha(int clienteID, string sessionID, string tid, int bandeiraCartao, string senha)
        {
            string bandeira = "";

            if (bandeiraCartao == FP_REDECARD)
                bandeira = "Redecard";
            else if (bandeiraCartao == FP_VISANET)
                bandeira = "Visanet";
            else if (bandeiraCartao == FP_AMEX)
                bandeira = "Amex";
            else if (bandeiraCartao == FP_HSBC)
                bandeira = "HSBC";

            string strSql = string.Empty;
            if (bandeiraCartao == FP_HSBC)
            {
                strSql = "UPDATE " + bandeira + " SET Senha = '" + senha.Replace("'", "''") + "'" +
                                " WHERE (ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' AND NumeroPedido = '" + tid + "')";
            }
            else
            {
                strSql = "UPDATE " + bandeira + " SET Senha = '" + senha.Replace("'", "''") + "'" +
                                " WHERE (ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' AND TID = '" + tid + "')";
            }
            //new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@ClienteID", clienteID), new System.Data.SqlClient.SqlParameter("@SessionID", sessionID) });           
            oDal.Execute(strSql);
        }

        public void InserirSenhaRetornoTEF(int clienteID, string sessionID, string senha, string retornoTefFinaliza)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public static int GetOrderID(int bandeiraID, int clienteID)
        {
            DAL oDAL = new DAL();

            string bandeira = string.Empty;

            if (bandeiraID == FP_REDECARD)
                bandeira = "Redecard";
            else if (bandeiraID == FP_VISANET)
                bandeira = "Visanet";
            else if (bandeiraID == FP_AMEX)
                bandeira = "Amex";

            object obj = oDAL.Scalar("SELECT MAX(ClientePedidoID) AS ClientePedidoID FROM " + bandeira + " WHERE ClienteID = " + clienteID, null);
            int clientePedidoID = (obj != null && obj.ToString() != string.Empty) ? Convert.ToInt32(obj) : 0;

            return ++clientePedidoID;
        }

        public static string GeraTID(int bandeiraID, int clienteID, int clientePedidoID)
        {
            return bandeiraID.ToString("00") + clienteID.ToString("00000000") + clientePedidoID.ToString("000000");
        }
    }
}
