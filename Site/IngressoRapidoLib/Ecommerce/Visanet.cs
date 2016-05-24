using CTLib;
using IngressoRapido.Lib;
using System;
using System.Configuration;
using System.Web;

namespace IngressoRapido.Ecommerce
{
    public class Visanet
    {
        private static string pagamento = "1001";  // a vista

        private static string numFiliacao = ConfigurationManager.AppSettings["VisanetFiliacao"].ToString();
        private static string merchid = ConfigurationManager.AppSettings["VisanetMerchID"].ToString();

        public Visanet() { }

        public static string GerarTid(string shopid, string pagamento)
        {
            if (shopid.Length != 10)
            {
                return "Tamanho do shopid deve ser 10 dígitos";
            }
            else
            {
                if (!Util.IsInt(shopid))
                {
                    return "Shopid deve ser numérico";
                }
            }

            shopid = shopid.Substring(4, 5);

            if (pagamento.Length != 4)
            {
                return "Tamanho do código de pagamento deve ser 4 dígitos.";
            }

            return shopid + DateTime.Now.Year.ToString().Substring(3) + DateTime.Now.DayOfYear.ToString("000") + DateTime.Now.ToString("HHmmssf") + pagamento;
        }

        public static string GerarTid()
        {
            return GerarTid(numFiliacao, pagamento);
        }

        public static string GeraComplementoTID(int parcelas, bool visaElectron)
        {
            string strProduto = "20"; // 20 = juros do lojista

            string strProdutoParcelas = string.Empty; //produto + frequencia         

            if (visaElectron)
            {
                strProdutoParcelas = "A001"; // Código VisaElectron débito a vista              
            }
            else
            {
                if (parcelas == 1)
                    strProdutoParcelas = "1001";// 10 = à vista
                else
                    strProdutoParcelas = strProduto + parcelas.ToString("00");
            }

            return strProdutoParcelas;
        }

        public static string Processar(decimal EntregaValor,int enderecoID,int pDVSelecionado,string dataSelecionada,int clienteID, int parcelas, Decimal totalCompra, string sessionID, int taxaEntregaID, int formaPagamentoID, string BIN, bool visaElectron)
        {
            try
            {
                int clientePedidoID = Controle.GetOrderID(Controle.FP_VISANET, clienteID);

                string tid = Controle.GeraTID(Controle.FP_VISANET, clienteID, clientePedidoID);
                tid += GeraComplementoTID(parcelas, visaElectron);

                string free = clienteID + ";" + sessionID + ";" + taxaEntregaID + ";" + EntregaValor + ";" + enderecoID + ";" + pDVSelecionado + ";" + dataSelecionada + ";" + formaPagamentoID + ";" + BIN + ";" + parcelas;

                //arrumar metodo
                if (InserirTransacao(clienteID, tid, clientePedidoID, parcelas, free, totalCompra, sessionID, taxaEntregaID, EntregaValor, enderecoID ,pDVSelecionado , dataSelecionada, formaPagamentoID, BIN) != 0)
                {
                    return Util.HTTPGetPage(
                                "https://comercio.locaweb.com.br/comercio.comp?merchid=" + merchid +
                                "&price=" + Util.FormataValorRedecard(totalCompra) +
                                "&damount=" + totalCompra.ToString("c") +
                                "&tid=" + tid +
                                "&orderid=" + clientePedidoID +
                                "&order=" + free +
                                "&authenttype=0" +
                                "&bin=" + BIN +
                                "&free=" + free +
                                "&visa_antipopup=0" +
                                "&PosicaoDadosVisanet=0");
                }
                else
                {
                    return "Por favor tente novamente";
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Método processar " + ex.Message);
            }
        }

        public static bool Capturar(string tid)
        {
            try
            {
                string fonte = Util.HTTPPostPage(
                    "https://comercio.locaweb.com.br/visavbv/capture.exe",
                    "merchid=" + merchid + "&tid=" + tid + "&free=" + HttpContext.Current.Session["ClienteID"] + ";" + HttpContext.Current.Session.SessionID);

                string lr_captura = RetornaDadosCaptura(fonte, "name=\"lr\" value=\"", 17);
                string tid_captura = RetornaDadosCaptura(fonte, "name=\"tid\" value=\"", 18);
                string cap_captura = RetornaDadosCaptura(fonte, "name=\"cap\" value=\"", 18);
                string ars_captura = RetornaDadosCaptura(fonte, "name=\"ars\" value=\"", 18);
                string free_captura = RetornaDadosCaptura(fonte, "name=\"free\" value=\"", 19);

                DAL oDAL = new DAL();
                string strSql = "UPDATE Visanet SET LR_captura = " + lr_captura +
                    ", TID_captura = '" + tid_captura + "', CAP_captura = '" + cap_captura +
                    "', ARS_captura = '" + ars_captura + "', FREE_captura = '" + free_captura.Replace("'", "''") +
                    "', TimeStamp_captura = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' " +
                    " WHERE (TID = '" + tid + "')";

                if (oDAL.Execute(strSql) == 1)
                {
                    if (lr_captura == "0" || lr_captura == "3")
                        return true;
                    else
                        throw new Exception("Ocorreu um erro na captura");
                }
                else
                    throw new ApplicationException("erro");
            }
            catch (Exception ex)
            {
                throw new Exception("Capturar Visanet: " + ex.Message);
            }
        }

        private static string RetornaDadosCaptura(string fonte, string busca, int count)
        {
            string retorno = fonte.Substring(fonte.IndexOf(busca) + count);
            retorno = retorno.Substring(0, retorno.IndexOf("\">"));

            return retorno;
        }

        // novo metodo                          
        public static int InserirTransacao(int ClienteID, string tid, int clientePedidoID, int parcelas, string free, Decimal totalCompra, string sessionID, int taxaEntregaID,decimal EntregaValor,int enderecoID,int pDVSelecionado,string dataSelecionada, int formaPagamentoID, string bin)
        {
            DAL oDAL = new DAL();

            int lugaresSeparados = 0;

            if (HttpContext.Current.Session["LugaresSeparados"] != null && (bool)HttpContext.Current.Session["LugaresSeparados"] != false)
                lugaresSeparados = 1;

            string strSql = "INSERT INTO Visanet (ClienteID, tid, ClientePedidoID, parcelas, free, price, sessionID, TaxaEntregaID, EntregaValor, EnderecoID, PDVSelecionado, DataSelecionada, FormaPagamentoID, BIN, TimeStamp, LugaresSeparados) VALUES " +
                            "(" + ClienteID + ", '" + tid + "', " + clientePedidoID + ", " + parcelas + ", '" + free.Replace("'", "''") + "', " + totalCompra.ToString("f").Replace(",", ".") + ", '" + sessionID + "', " + taxaEntregaID + ", " + EntregaValor + ", " + enderecoID + ", " + pDVSelecionado + ", '" + dataSelecionada + "' , " + formaPagamentoID + ", '" + bin + "', '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', " + lugaresSeparados + ")";

            return oDAL.Execute(strSql);
        }

        //novo
        public static int AtualizarTransacao(int ClienteID, string tid, int lr, string arp, string ars, string authenttype, string pan, string bank, string sessionID)
        {
            DAL oDAL = new DAL();
            string strSql = "UPDATE Visanet SET lr = '" + lr + "', arp = '" + arp + "', ars = '" + ars + "', authenttype = '" + authenttype + "', pan = '" + pan + "' , bank = '" + bank + "', TimeStamp_retorno = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' " +
                            " WHERE (ClienteID = " + ClienteID + " AND SessionID = '" + sessionID + "' AND tid = '" + tid + "')";

            return oDAL.Execute(strSql);
        }
    }
}
