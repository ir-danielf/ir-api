using CTLib;
using IngressoRapido.Lib;
using System;
using System.Configuration;
using System.Web;

namespace IngressoRapido.Ecommerce
{
    public class Redecard
    {
        private static string numFiliacao = ConfigurationManager.AppSettings["RedecardFiliacao"].ToString();

        public Redecard() { }

        public static string GerarTid()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssff");
        }




        public static string Processar(int clienteID, int parcelas, Decimal totalCompra, string free, string sessionID, int taxaEntregaID, int formaPagamentoID, bool diners)
        {
            int clientePedidoID = Controle.GetOrderID(Controle.FP_REDECARD, clienteID);
            string tid = Controle.GeraTID(Controle.FP_REDECARD, clienteID, clientePedidoID);

            int popup = 1;
            string loja = ConfigurationManager.AppSettings["RedecardMerchID"].ToString();

            string strBandeira = "";
            int bandeira = 1;

            if (diners)
                bandeira = 2;

            if (bandeira == 1)
                strBandeira = "CREDICARD";
            else if (bandeira == 2)
                strBandeira = "DINERS";
            else
                throw new Exception("Bandeira Inexistente");

            string urlRetorno = ConfigurationManager.AppSettings["RetornoRedecard"].ToString();

            string AVS = String.Empty; //usar "s" para habilitar         

            if (InserirTransacao(clienteID, tid, clientePedidoID, parcelas, totalCompra, sessionID, taxaEntregaID, formaPagamentoID, bandeira) != 0)
            {
                return Util.HTTPPostPage("https://comercio.locaweb.com.br/comercio.comp",
                    "meio_pagamento_seguro=REDECARD&metodo=SAFENET&BANDEIRA=" + strBandeira + "&loja=" + loja + "&pedido=" + tid + "&valor=" + Util.FormataValorRedecard(totalCompra) +
                    "&parcelas=" + parcelas + "&juros=0&urlback=" + urlRetorno + "&popup=" + popup + "&PAX1=" + tid +
                    "&AVS=" + AVS + "&RedecardIdioma=pt&TARGET=&urlcima=");
            }
            else
            {
                return "Por favor tente novamente";
            }
        }



        public static int InserirTransacao(int clienteID, string tid, int clientePedidoID, int parcelas, Decimal totalCompra, string sessionID, int taxaEntregaID, int formaPagamentoID, int bandeira)
        {
            DAL oDAL = new DAL();

            int lugaresSeparados = 0;

            if (HttpContext.Current.Session["LugaresSeparados"] != null && (bool)HttpContext.Current.Session["LugaresSeparados"] != false)
                lugaresSeparados = 1;

            string strSql = "INSERT INTO Redecard (ClienteID, tid, ClientePedidoID, Parcelas, total, SessionID, TaxaEntregaID, FormaPagamentoID, bandeira, TimeStamp, LugaresSeparados) VALUES (" + clienteID + ",'" + tid + "', " + clientePedidoID + ", " + parcelas + ", " + totalCompra.ToString("f").Replace(",", ".") + ", '" + sessionID + "', " + taxaEntregaID + ", " + formaPagamentoID + ", " + bandeira + ", '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "', " + lugaresSeparados + " )";

            return oDAL.Execute(strSql);
        }

        public static int AtualizarRetorno(int codret, string msgret, string numautor, string numcv, string numsqn, string data, string respavs, string msgavs, string numautent, string nr_cartao, string origem_bin, string free, int clienteID, string sessionID, string tid)
        {
            DAL oDAL = new DAL();
            string strSql = "UPDATE Redecard SET CODRET = " + codret + ", MSGRET = '" + msgret.Replace("'", "''") + "', NUMAUTOR = '" + numautor + "', NUMCV = '" + numcv + "', NUMSQN = '" + numsqn + "' , DATA = '" + data + "', RESPAVS = '" + respavs + "', MSGAVS = '" + msgavs + "', NUMAUTENT = '" + numautent + "', NR_CARTAO = '" + nr_cartao + "', ORIGEM_BIN = '" + origem_bin + "', PAX1 = '" + free.Replace("'", "''") + "', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' " +
            " WHERE (ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' AND tid = '" + tid + "')";

            return oDAL.Execute(strSql);
        }
        public static int AtualizarRetorno(int codret, string msgret, int clienteID, string sessionID, string tid)
        {
            DAL oDAL = new DAL();
            string strSql = "UPDATE Redecard SET CODRET = " + codret + ", MSGRET = '" + msgret.Replace("'", "''") + "', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' " +
            " WHERE (ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' AND tid = '" + tid + "')";

            return oDAL.Execute(strSql);
        }

        public static int AtualizarConfirmacao(int CODRET_confirmacao, string MSGRET_confirmacao, int clienteID, string sessionID, string tid)
        {
            DAL oDAL = new DAL();
            string strSql = "UPDATE Redecard SET CODRET_confirmacao = " + CODRET_confirmacao + ", MSGRET_confirmacao = '" + MSGRET_confirmacao.Replace("'", "''") + "', TimeStamp = '" + DateTime.Now.ToString("yyyyMMddHHmmss") + "' " +
            " WHERE (ClienteID = " + clienteID + " AND SessionID = '" + sessionID + "' AND tid = '" + tid + "')";

            return oDAL.Execute(strSql);
        }


        public static bool Capturar(string valores, string tid)
        {
            try
            {
                //************************ Confirma transação com a Redecard ***************

                // contacta RedeCard e confirma transação
                string strRetornoRedecard = Util.HTTPGetPage("http://ecommerce.redecard.com.br/pos_virtual/confirma.asp?" + valores);
                //CODRET=8&MSGRET=DADOS NAO COINCIDEM", CODRET=9&MSGRET=TRANSACAO NAO ENCONTRADA

                // formata o retorno da confirmação da compra
                string strCODRET = strRetornoRedecard.Substring(strRetornoRedecard.IndexOf("=") + 1);
                strCODRET = strCODRET.Substring(0, strCODRET.IndexOf("&"));

                int CODRET = int.Parse(strCODRET);
                string MSGRET = HttpUtility.UrlDecode(strRetornoRedecard.Substring(strRetornoRedecard.LastIndexOf("=") + 1), System.Text.Encoding.Default);

                // grava confirmacao                
                if (Redecard.AtualizarConfirmacao(CODRET, MSGRET, (int)HttpContext.Current.Session["ClienteID"], HttpContext.Current.Session.SessionID.ToString(), tid) != 0)
                {

                }

                if (CODRET == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Captura Redecard: " + ex.Message);
            }
        }
    }
}
