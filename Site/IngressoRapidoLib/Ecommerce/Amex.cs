using CTLib;
using IngressoRapido.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;

namespace IngressoRapido.Ecommerce
{
    public class Amex
    {
        // constantes

        //Ingressorapido
        //private const string Merchant = "TEST9917588774"; //número de Afiliação
        //private const string AccessCode = "DEE970C5";
        //private const string SECURE_SECRET = "E003E46A9C3172C3CF8D1A3352000F16";
        //private const string ReturnURL = "http://localhost:9999/ingressorapido.com.br/ecommerce/Amex/retornoAmex.aspx";
        //private const string vpc_User = "captura";
        //private const string vpc_Password = "123mudar";

        private static string Merchant = ConfigurationManager.AppSettings["AmexFiliacao"].ToString();
        private static string AccessCode = ConfigurationManager.AppSettings["AmexAccessCode"].ToString();
        private static string SECURE_SECRET = ConfigurationManager.AppSettings["AmexSECURE_SECRET"].ToString();

        private static string ReturnURL = ConfigurationManager.AppSettings["RetornoAmex"].ToString();
        private static string vpc_User = ConfigurationManager.AppSettings["AmexCapturaUser"].ToString();
        private static string vpc_Password = ConfigurationManager.AppSettings["AmexCapturaPassword"].ToString();


        public static string Processar(int clienteID, int parcelas, Decimal totalCompra, string sessionID, int taxaEntregaID, int formaPagamentoID, string bin)
        {
            SortedList transactionData = new SortedList(new VPCStringComparer());
            string queryString = "https://vpos.amxvpos.com/vpcpay";

            int clientePedidoID = Controle.GetOrderID(Controle.FP_AMEX, clienteID);
            string tid = Controle.GeraTID(Controle.FP_AMEX, clienteID, clientePedidoID);
            //string tid = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");

            transactionData.Add("vpc_Version", "1");
            transactionData.Add("vpc_Command", "pay");
            transactionData.Add("vpc_AccessCode", AccessCode);
            transactionData.Add("vpc_MerchTxnRef", tid); // TID validar se funciona bem
            transactionData.Add("vpc_OrderInfo", sessionID);  //SessionID
            transactionData.Add("vpc_Merchant", Merchant);
            transactionData.Add("vpc_Locale", "pt_BR");
            transactionData.Add("vpc_ReturnURL", ReturnURL);
            transactionData.Add("vpc_Amount", Util.FormataValorRedecard(totalCompra));  //preco no formato 15000 = 150,00 

            // campos variáveis
            if (parcelas > 1)
            {
                transactionData.Add("vpc_PaymentPlan", "PlanN");
                transactionData.Add("vpc_numPayments", parcelas);    //parcelas TODO: verificar com AMEX n esta funcionando parece           
            }

            string rawHashData = SECURE_SECRET;
            string seperator = "?";

            foreach (System.Collections.DictionaryEntry item in transactionData)
            {
                // build the query string, URL Encoding all keys and their values
                queryString += seperator + System.Web.HttpUtility.UrlEncode(item.Key.ToString()) + "=" + System.Web.HttpUtility.UrlEncode(item.Value.ToString());
                seperator = "&";

                // Collect the data required for the MD5 sugnature if required
                if (SECURE_SECRET.Length > 0)
                {
                    rawHashData += item.Value.ToString();
                }
            }

            // Create the MD5 signature if required
            string signature = "";
            if (SECURE_SECRET.Length > 0)
            {
                // create the signature and add it to the query string
                signature = CreateMD5Signature(rawHashData);
                queryString += seperator + "vpc_SecureHash=" + signature;
            }


            // Transmit the DO to the Payment Server via a browser redirect
            //Page.Response.Redirect(queryString);

            if (InserirTransacao(clienteID, tid, clientePedidoID, parcelas, totalCompra, sessionID, taxaEntregaID, formaPagamentoID, bin) != 0)
            {
                return queryString;
            }
            else
            {
                return string.Empty;
            }

            #region querystring pronta

            //queryString = "https://vpos.amxvpos.com/vpcpay?" +
            //"&vpc_Version=1" +
            //"&vpc_Command=pay" +
            //"&vpc_AccessCode=DEE970C5" +
            //"&vpc_MerchTxnRef=" + DateTime.Now.ToString("yyyyMMddHHmmssff") +
            //"&vpc_Merchant=TEST9917588774" +
            //"&vpc_OrderInfo=campo livre" +
            //"&vpc_Amount=100" +
            //"&vpc_ReturnURL=http://localhost:8888/Amex/DR.aspx" +
            //"&vpc_Locale=pt_BR";

            #endregion
        }

        public static int InserirTransacao(int clienteID, string tid, int clientePedidoID, int parcelas, Decimal totalCompra, string sessionID, int taxaEntregaID, int formaPagamentoID, string bin)
        {
            DAL oDAL = new DAL();
            int retorno = 0;

            int lugaresSeparados = 0;

            if (HttpContext.Current.Session["LugaresSeparados"] != null && (bool)HttpContext.Current.Session["LugaresSeparados"] != false)
                lugaresSeparados = 1;

            string strSql = "INSERT INTO AMEX (ClienteID, TID, ClientePedidoID, Parcelas, Total, SessionID, TaxaEntregaID, FormaPagamentoID, BIN, TimeStamp, LugaresSeparados) VALUES (" + clienteID + ",'" + tid + "', " + clientePedidoID + ", " + parcelas + ", " + totalCompra.ToString("f").Replace(",", ".") + ", '" + sessionID + "', " + taxaEntregaID + ", " + formaPagamentoID + ", '" + bin + "', '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', " + lugaresSeparados + ")";

            try
            {
                retorno = oDAL.Execute(strSql);
                oDAL.ConnClose();
                return retorno;
            }
            catch
            {
                oDAL.ConnClose();
                return 0;
            }
        }

        public static int AtualizarRetorno(Dictionary<string, string> queryString)
        {
            DAL oDAL = new DAL();
            StringBuilder strSql = new StringBuilder();
            int retorno = 0;
            if (queryString.ContainsKey("vpc_OrderInfo") && queryString["vpc_OrderInfo"] != null && queryString["vpc_OrderInfo"] != string.Empty)
            {
                strSql.AppendLine("UPDATE Amex SET ");

                if (queryString.ContainsKey("vpc_TxnResponseCode") && queryString["vpc_TxnResponseCode"] != null)
                    strSql.Append("vpc_TxnResponseCode = '" + queryString["vpc_TxnResponseCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_OrderInfo") && queryString["vpc_OrderInfo"] != null)
                    strSql.Append("vpc_OrderInfo = '" + queryString["vpc_OrderInfo"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Version") && queryString["vpc_Version"] != null)
                    strSql.Append("vpc_Version = '" + queryString["vpc_Version"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Command") && queryString["vpc_Command"] != null)
                    strSql.Append("vpc_Command = '" + queryString["vpc_Command"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Merchant") && queryString["vpc_Merchant"] != null)
                    strSql.Append("vpc_Merchant = '" + queryString["vpc_Merchant"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_BatchNo") && queryString["vpc_BatchNo"] != null)
                    strSql.Append("vpc_BatchNo = '" + queryString["vpc_BatchNo"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Card") && queryString["vpc_Card"] != null)
                    strSql.Append("vpc_Card = '" + queryString["vpc_Card"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_ReceiptNo") && queryString["vpc_ReceiptNo"] != null)
                    strSql.Append("vpc_ReceiptNo = '" + queryString["vpc_ReceiptNo"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AuthorizeId") && queryString["vpc_AuthorizeId"] != null)
                    strSql.Append("vpc_AuthorizeID = '" + queryString["vpc_AuthorizeId"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_MerchTxnRef") && queryString["vpc_MerchTxnRef"] != null)
                    strSql.Append("vpc_MerchTxnRef = '" + queryString["vpc_MerchTxnRef"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AcqResponseCode") && queryString["vpc_AcqResponseCode"] != null)
                    strSql.Append("vpc_AcqResponseCode = '" + queryString["vpc_AcqResponseCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AcqCSCRespCode") && queryString["vpc_AcqCSCRespCode"] != null)
                    strSql.Append("vpc_AcqCSCRespCode = '" + queryString["vpc_AcqCSCRespCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AcqAVSRespCode") && queryString["vpc_AcqAVSRespCode"] != null)
                    strSql.Append("vpc_AcqAVSRespCode = '" + queryString["vpc_AcqAVSRespCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_CSCRequestCode") && queryString["vpc_CSCRequestCode"] != null)
                    strSql.Append("vpc_CSCRequestCode = '" + queryString["vpc_CSCRequestCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_CSCResultCode") && queryString["vpc_CSCResultCode"] != null)
                    strSql.Append("vpc_CSCResultCode = '" + queryString["vpc_CSCResultCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_TransactionNo") && queryString["vpc_TransactionNo"] != null)
                    strSql.Append("vpc_TransactionNo = '" + queryString["vpc_TransactionNo"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Message") && queryString["vpc_Message"] != null)
                    strSql.Append("vpc_Message = '" + queryString["vpc_Message"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_SecureHash") && queryString["vpc_SecureHash"] != null)
                    strSql.Append("vpc_SecureHash = '" + queryString["vpc_SecureHash"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AVSRequestCode") && queryString["vpc_AVSRequestCode"] != null)
                    strSql.Append("AVSRequestCode = '" + queryString["vpc_AVSRequestCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AVSResultCode") && queryString["vpc_AVSResultCode"] != null)
                    strSql.Append("AVSResultCode = '" + queryString["vpc_AVSResultCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AVS_Street01") && queryString["vpc_AVS_Street01"] != null)
                    strSql.Append("AVS_Street01 = '" + queryString["vpc_AVS_Street01"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AVS_City") && queryString["vpc_AVS_City"] != null)
                    strSql.Append("AVS_City = '" + queryString["vpc_AVS_City"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AVS_StateProv") && queryString["vpc_AVS_StateProv"] != null)
                    strSql.Append("AVS_StateProv = '" + queryString["vpc_AVS_StateProv"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AVS_PostCode") && queryString["vpc_AVS_PostCode"] != null)
                    strSql.Append("AVS_PostCode = '" + queryString["vpc_AVS_PostCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_AVS_Country") && queryString["vpc_AVS_Country"] != null)
                    strSql.Append("AVS_Country = '" + queryString["vpc_AVS_Country"].Trim() + "' ");

                if (strSql.ToString().EndsWith(", "))
                    strSql.Remove(strSql.Length - 2, 1);

                strSql.AppendLine(" WHERE (SessionID = '" + queryString["vpc_OrderInfo"].Trim() + "' AND TID = '" + queryString["vpc_MerchTxnRef"].Trim() + "')");

                try
                {
                    retorno = oDAL.Execute(strSql.ToString());
                    oDAL.ConnClose();
                    return retorno;
                }
                catch
                {
                    oDAL.ConnClose();
                    return 0;
                }
            }
            else
                return 0;
        }

        public static bool Capturar(string tid)
        {
            DAL oDAL = new DAL();
            IDataReader reader = null;
            string vpc_TransactionNo = string.Empty;
            string vpc_Amount = string.Empty;

            try
            {
                reader = oDAL.SelectToIDataReader("SELECT TOP 1 vpc_TransactionNo, total FROM Amex WHERE TID = '" + tid + "' ORDER BY ID DESC");

                if (!reader.Read())
                    throw new ApplicationException("Informações do pagamento não encontradas.");
                else
                {
                    vpc_TransactionNo = reader["vpc_TransactionNo"].ToString();
                    vpc_Amount = reader["total"].ToString().Replace(",", "").Replace(".", "");
                }
                oDAL.ConnClose();

                string queryStringPost = "vpc_Version=1" +
                    "&vpc_Command=capture" +
                    "&vpc_AccessCode=" + AccessCode +
                    "&vpc_Merchant=" + Merchant +
                    "&vpc_User=" + vpc_User +
                    "&vpc_Password=" + vpc_Password +
                    // vars
                    "&vpc_MerchTxnRef=" + tid +
                    "&vpc_TransNo=" + vpc_TransactionNo +
                    "&vpc_Amount=" + vpc_Amount;

                // retorno da captura
                string[] retorno = HttpUtility.UrlDecode(Util.HTTPPostPage("https://vpos.amxvpos.com/vpcdps", queryStringPost)).Split('&');


                Dictionary<string, string> queryString = new Dictionary<string, string>();

                foreach (string v in retorno)
                {
                    string[] item = v.Split('=');
                    queryString.Add(item[0], item[1]);
                }

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();

                strSql.AppendLine("UPDATE Amex SET ");

                if (queryString.ContainsKey("vpc_AcqResponseCode") && queryString["vpc_AcqResponseCode"] != null)
                    strSql.AppendLine("cap_AcqResponseCode = '" + queryString["vpc_AcqResponseCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Amount") && queryString["vpc_Amount"] != null)
                    strSql.AppendLine("cap_Amount='" + Util.AmountToDecimalBD(queryString["vpc_Amount"].Trim()) + "', ");

                if (queryString.ContainsKey("vpc_AuthorisedAmount") && queryString["vpc_AuthorisedAmount"] != null)
                    strSql.AppendLine("cap_AuthorisedAmount='" + Util.AmountToDecimalBD(queryString["vpc_AuthorisedAmount"].Trim()) + "', ");

                if (queryString.ContainsKey("vpc_AuthorizeId") && queryString["vpc_AuthorizeId"] != null)
                    strSql.AppendLine("cap_AuthorizeID='" + queryString["vpc_AuthorizeId"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_BatchNo") && queryString["vpc_BatchNo"] != null)
                    strSql.AppendLine("cap_BatchNo='" + queryString["vpc_BatchNo"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_CapturedAmount") && queryString["vpc_CapturedAmount"] != null)
                    strSql.AppendLine("cap_CapturedAmount='" + Util.AmountToDecimalBD(queryString["vpc_CapturedAmount"].Trim()) + "', ");

                if (queryString.ContainsKey("vpc_Card") && queryString["vpc_Card"] != null)
                    strSql.AppendLine("cap_Card='" + queryString["vpc_Card"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Command") && queryString["vpc_Command"] != null)
                    strSql.AppendLine("cap_Command='" + queryString["vpc_Command"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_MerchTxnRef") && queryString["vpc_MerchTxnRef"] != null)
                    strSql.AppendLine("cap_MerchTxnRef='" + queryString["vpc_MerchTxnRef"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Merchant") && queryString["vpc_Merchant"] != null)
                    strSql.AppendLine("cap_Merchant='" + queryString["vpc_Merchant"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_Message") && queryString["vpc_Message"] != null)
                    strSql.AppendLine("cap_Message='" + queryString["vpc_Message"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_ReceiptNo") && queryString["vpc_ReceiptNo"] != null)
                    strSql.AppendLine("cap_ReceiptNo='" + queryString["vpc_ReceiptNo"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_RefundedAmount") && queryString["vpc_RefundedAmount"] != null)
                    strSql.AppendLine("cap_RefundedAmount='" + queryString["vpc_RefundedAmount"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_ShopTransactionNo") && queryString["vpc_ShopTransactionNo"] != null)
                    strSql.AppendLine("cap_ShopTransactionNo='" + queryString["vpc_ShopTransactionNo"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_TransactionNo") && queryString["vpc_TransactionNo"] != null)
                    strSql.AppendLine("cap_TransactionNo='" + queryString["vpc_TransactionNo"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_TxnResponseCode") && queryString["vpc_TxnResponseCode"] != null)
                    strSql.AppendLine("cap_TxnResponseCode='" + queryString["vpc_TxnResponseCode"].Trim() + "', ");

                if (queryString.ContainsKey("vpc_terminalID") && queryString["vpc_terminalID"] != null)
                    strSql.AppendLine("cap_terminalID='" + queryString["vpc_terminalID"].Trim() + "', ");

                strSql.AppendLine("TimeStamp_captura = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' ");
                strSql.AppendLine("WHERE (TID = '" + queryString["vpc_MerchTxnRef"].Trim() + "')");

                oDAL = new DAL();
                //grava retorno da captura no banco
                if (oDAL.Execute(strSql.ToString()) == 1)
                {
                    if (queryString["vpc_TxnResponseCode"].Trim() != "0")
                        throw new Exception("Ocorreu um erro na captura");
                }
                else
                    throw new ApplicationException("erro");

                oDAL.ConnClose();
                return true;
            }
            catch (Exception ex)
            {
                oDAL.ConnClose();
                throw new Exception("Capturar Amex: " + ex.Message);
            }
        }

        #region Amex Methods

        private class VPCStringComparer : IComparer
        {
            /*
             <summary>Customised Compare Class</summary>
             <remarks>
             <para>
             The Virtual Payment Client need to use an Ordinal comparison to Sort on 
             the field names to create the MD5 Signature for validation of the message. 
             This class provides a Compare method that is used to allow the sorted list 
             to be ordered using an Ordinal comparison.
             </para>
             </remarks>
             */

            public int Compare(Object a, Object b)
            {
                /*
                 <summary>Compare method using Ordinal comparison</summary>
                 <param name="a">The first string in the comparison.</param>
                 <param name="b">The second string in the comparison.</param>
                 <returns>An int containing the result of the comparison.</returns>
                 */

                // Return if we are comparing the same object or one of the 
                // objects is null, since we don't need to go any further.
                if (a == b) return 0;
                if (a == null) return -1;
                if (b == null) return 1;

                // Ensure we have string to compare
                string sa = a as string;
                string sb = b as string;

                // Get the CompareInfo object to use for comparing
                System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
                if (sa != null && sb != null)
                {
                    // Compare using an Ordinal Comparison.
                    return myComparer.Compare(sa, sb, System.Globalization.CompareOptions.Ordinal);
                }
                throw new ArgumentException("a and b should be strings.");
            }
        }

        private static string CreateMD5Signature(string RawData)
        {
            /*
             <summary>Creates a MD5 Signature</summary>
             <param name="RawData">The string used to create the MD5 signautre.</param>
             <returns>A string containing the MD5 signature.</returns>
             */

            System.Security.Cryptography.MD5 hasher = System.Security.Cryptography.MD5CryptoServiceProvider.Create();
            byte[] HashValue = hasher.ComputeHash(System.Text.Encoding.ASCII.GetBytes(RawData));

            string strHex = "";
            foreach (byte b in HashValue)
            {
                strHex += b.ToString("x2");
            }
            return strHex.ToUpper();
        }

        private string getResponseDescription(string vResponseCode)
        {
            /* 
             <summary>Maps the vpc_TxnResponseCode to a relevant description</summary>
             <param name="vResponseCode">The vpc_TxnResponseCode returned by the transaction.</param>
             <returns>The corresponding description for the vpc_TxnResponseCode.</returns>
             */
            string result = "Indefinido";

            if (vResponseCode.Length > 0)
            {
                switch (vResponseCode)
                {
                    case "0": result = "Transação finalizada com Sucesso"; break;
                    case "1": result = "Erro desconhecido"; break;
                    case "2": result = "Transação recusada pelo Banco"; break;
                    case "3": result = "Nenhuma resposta do banco"; break;
                    case "4": result = "Cartão Expirado"; break;
                    case "5": result = "Saldo Insuficientes"; break;
                    case "6": result = "Erro ao comunicar-se com o banco, tente mais tarde"; break;
                    case "7": result = "Payment Server System Error"; break;
                    case "8": result = "Tipo Da Transação não suportado"; break;
                    case "9": result = "Transação recusado pelo banco (não contate o banco)"; break;
                    case "A": result = "Transação Abortada"; break;
                    case "C": result = "Transação Cancelada"; break;
                    case "D": result = "A transação aguardando processamento pelo Banco"; break;
                    case "F": result = "O VISA 3D Secure negou a autorização"; break;
                    case "I": result = "A verificação do código de segurança do cartão falhou"; break;
                    case "L": result = "Shopping Transaction Locked (Please try the transaction again later)"; break;
                    case "N": result = "Seu cartão não é registrado no VISA 3D Secure"; break;
                    case "P": result = "A transação foi recebida pelo gerenciador do pagamento e está sendo processada"; break;
                    case "R": result = "A transação não foi processada - limite de tentativas excedido"; break;
                    case "T": result = "A Verificação do Endereço Falhou"; break;
                    case "U": result = "O Código de Segurança do Cartão Falhou"; break;
                    case "V": result = "O código da verificação do endereço e de segurança do cartão falhou"; break;
                    case "?": result = "O status da transação é desconhecido"; break;
                    default: result = "Indefinido"; break;

                    #region Original

                    //case "0": result = "Transaction Successful"; break;
                    //case "1": result = "Transaction Declined"; break;
                    //case "2": result = "Bank Declined Transaction"; break;
                    //case "3": result = "No Reply from Bank"; break;
                    //case "4": result = "Expired Card"; break;
                    //case "5": result = "Insufficient Funds"; break;
                    //case "6": result = "Error Communicating with Bank"; break;
                    //case "7": result = "Payment Server detected an error"; break;
                    //case "8": result = "Transaction Type Not Supported"; break;
                    //case "9": result = "Bank declined transaction (Do not contact Bank)"; break;
                    //case "A": result = "Transaction Aborted"; break;
                    //case "B": result = "Transaction Declined - Contact the Bank"; break;
                    //case "C": result = "Transaction Cancelled"; break;
                    //case "D": result = "Deferred transaction has been received and is awaiting processing"; break;
                    //case "F": result = "3-D Secure Authentication failed"; break;
                    //case "I": result = "Card Security Code verification failed"; break;
                    //case "L": result = "Shopping Transaction Locked (Please try the transaction again later)"; break;
                    //case "N": result = "Cardholder is not enrolled in Authentication scheme"; break;
                    //case "P": result = "Transaction has been received by the Payment Adaptor and is being processed"; break;
                    //case "R": result = "Transaction was not processed - Reached limit of retry attempts allowed"; break;
                    //case "S": result = "Duplicate SessionID"; break;
                    //case "T": result = "Address Verification Failed"; break;
                    //case "U": result = "Card Security Code Failed"; break;
                    //case "V": result = "Address Verification and Card Security Code Failed"; break;
                    //default: result = "Unable to be determined"; break; 

                    #endregion
                }
            }
            return result;
        }

        private string displayAVSResponse(string vAVSResultCode)
        {
            /*
             <summary>Maps the vpc_AVSResultCode to a relevant description</summary>
             <param name="vAVSResultCode">The vpc_AVSResultCode returned by the transaction.</param>
             <returns>The corresponding description for the vpc_AVSResultCode.</returns>
             */
            string result = "Unknown";

            if (vAVSResultCode.Length > 0)
            {
                if (vAVSResultCode.Equals("Unsupported"))
                {
                    result = "AVS not supported or there was no AVS data provided";
                }
                else
                {
                    switch (vAVSResultCode)
                    {
                        case "X": result = "Exact match - address and 9 digit ZIP/postal code"; break;
                        case "Y": result = "Exact match - address and 5 digit ZIP/postal code"; break;
                        case "S": result = "Service not supported or address not verified (international transaction)"; break;
                        case "G": result = "Issuer does not participate in AVS (international transaction)"; break;
                        case "A": result = "Address match only"; break;
                        case "W": result = "9 digit ZIP/postal code matched, Address not Matched"; break;
                        case "Z": result = "5 digit ZIP/postal code matched, Address not Matched"; break;
                        case "R": result = "Issuer system is unavailable"; break;
                        case "U": result = "Address unavailable or not verified"; break;
                        case "E": result = "Address and ZIP/postal code not provided"; break;
                        case "N": result = "Address and ZIP/postal code not matched"; break;
                        case "0": result = "AVS not requested"; break;
                        default: result = "Unable to be determined"; break;
                    }
                }
            }
            return result;
        }

        private string displayCSCResponse(string vCSCResultCode)
        {
            /*
             <summary>Maps the vpc_CSCResultCode to a relevant description</summary>
             <param name="vCSCResultCode">The vpc_CSCResultCode returned by the transaction.</param>
             <returns>The corresponding description for the vpc_CSCResultCode.</returns>
             */
            string result = "Unknown";
            if (vCSCResultCode.Length > 0)
            {
                if (vCSCResultCode.Equals("Unsupported"))
                {
                    result = "CSC not supported or there was no CSC data provided";
                }
                else
                {

                    switch (vCSCResultCode)
                    {
                        case "M": result = "Exact code match"; break;
                        case "S": result = "Merchant has indicated that CSC is not present on the card (MOTO situation)"; break;
                        case "P": result = "Code not processed"; break;
                        case "U": result = "Card issuer is not registered and/or certified"; break;
                        case "N": result = "Code invalid or not matched"; break;
                        default: result = "Unable to be determined"; break;
                    }
                }
            }
            return result;
        }

        private System.Collections.Hashtable splitResponse(string rawData)
        {
            /*
             * <summary>This function parses the content of the VPC response
             * <para>This function parses the content of the VPC response to extract the
             * individual parameter names and values. These names and values are then
             * returned as a Hashtable.</para>
             *
             * <para>The content returned by the VPC is a HTTP POST, so the content will
             * be in the format "parameter1=value&parameter2=value&parameter3=value".
             * i.e. key/value pairs separated by ampersands "&".</para>
             *
             * <param name="RawData"> data string containing the raw VPC response content
             * <returns> responseData - Hashtable containing the response data
             */
            System.Collections.Hashtable responseData = new System.Collections.Hashtable();
            try
            {
                // Check if there was a response containing parameters
                if (rawData.IndexOf("=") > 0)
                {
                    // Extract the key/value pairs for each parameter
                    foreach (string pair in rawData.Split('&'))
                    {
                        int equalsIndex = pair.IndexOf("=");
                        if (equalsIndex > 1 && pair.Length > equalsIndex)
                        {
                            string paramKey = System.Web.HttpUtility.UrlDecode(pair.Substring(0, equalsIndex));
                            string paramValue = System.Web.HttpUtility.UrlDecode(pair.Substring(equalsIndex + 1));
                            responseData.Add(paramKey, paramValue);
                        }
                    }
                }
                else
                {
                    // There were no parameters so create an error
                    responseData.Add("vpc_Message", "The data contained in the response was not a valid receipt.<br/>\nThe data was: <pre>" + rawData + "</pre><br/>\n");
                }
                return responseData;
            }
            catch (Exception ex)
            {
                // There was an exception so create an error
                responseData.Add("vpc_Message", "\nThe was an exception parsing the response data.<br/>\nThe data was: <pre>" + rawData + "</pre><br/>\n<br/>\nException: " + ex.ToString() + "<br/>\n");
                return responseData;
            }
        }

        #endregion

    }
}
