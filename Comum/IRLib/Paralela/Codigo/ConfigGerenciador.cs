using CTLib;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace IRLib.Paralela
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class ConfigGerenciadorParalela : MarshalByRefObject
    {

        public string getCodigoSms()
        {
            return ConfigurationManager.AppSettings["CodigoSms"];
        }

        public string getSenhaSms()
        {
            return ConfigurationManager.AppSettings["SenhaSms"];
        }

        public bool getStatusEnvioSms()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["StatusSms"]);
        }

        public string getEventosNaoImpressos()
        {
            if (ConfigurationManager.AppSettings["EventosNaoImpressos"] != null)
                return ConfigurationManager.AppSettings["EventosNaoImpressos"];
            else
                return "0";
        }

        public int getValorTempoReserva()
        {
            int valor = 0;
            if (ConfigurationManager.AppSettings["TempoCarrinho"] == null)
                valor = 20;
            else
                valor = Convert.ToInt32(ConfigurationManager.AppSettings["TempoCarrinho"]);

            return valor;
        }


        public decimal getValorMaximoAntiFraude()
        {
            decimal valor = 0;
            if (ConfigurationManager.AppSettings["ValorMaximoComprasCanaisRisco"] == null)
                valor = 2500;
            else
                valor = Convert.ToDecimal(ConfigurationManager.AppSettings["ValorMaximoComprasCanaisRisco"]);

            return valor;
        }

        public decimal getValorMinimoAntiFraude()
        {
            decimal valor = 0;
            if (ConfigurationManager.AppSettings["AntiFraudeValorMaximoCompra"] == null)
                valor = 500;
            else
                valor = Convert.ToDecimal(ConfigurationManager.AppSettings["AntiFraudeValorMaximoCompra"]);

            return valor;
        }

        public string getPathHtml()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["CaminhoEmailMotivoCancelamento"] == null)
                msg = "O caminho dos HTMLs não foi associado. Entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["CaminhoEmailMotivoCancelamento"].ToString();
            return msg;

        }
        public string getDownloadPathHtml()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["CaminhoDownloadEmailMotivoCancelamento"] == null)
                msg = "O caminho dos HTMLs não foi associado. Entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["CaminhoDownloadEmailMotivoCancelamento"].ToString();
            return msg;
        }

        public string getPathManual()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["PathManual"] == null)
                msg = "O caminho dos Manuais não foi associado. Entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["PathManual"].ToString();
            return msg;

        }
        public string getDownloadPathManual()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["DownloadPathManual"] == null)
                msg = "O caminho dos Manuais não foi associado. Entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["DownloadPathManual"].ToString();
            return msg;
        }


        public string getDownloadPathAssinatura()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["DownloadPathAssinatura"] == null)
                msg = "O caminho das Assinaturas não foi associado. Entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["DownloadPathAssinatura"].ToString();
            return msg;
        }


        public string getMensagemIngressoTriagem()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["MensagemIngressoTriagem"] == null)
                msg = "A Mensagem de Triagem não foi associada, entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["MensagemIngressoTriagem"].ToString();
            return msg;
        }

        public string getMensagemIngressoTriagemEmLote()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["MensagemIngressoTriagemEmLote"] == null)
                msg = "A Mensagem de Triagem não foi associada, entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["MensagemIngressoTriagemEmLote"].ToString();
            return msg;
        }

        public string getMensagemFraude()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["MensagemFraude"] == null)
                msg = "A Mensagem de Triagem não foi associada, entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["MensagemFraude"].ToString();
            return msg;
        }

        public string getMensagemClienteBloqueado()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["MensagemClienteBloqueado"] == null)
                msg = "A Mensagem de Cliente Bloqueado não foi associada, entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["MensagemClienteBloqueado"].ToString();
            return msg;
        }

        public string getMensagemClienteBloqueadoEmLote()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["MensagemClienteBloqueadoEmLote"] == null)
                msg = "A Mensagem de Cliente Bloqueado (Lote) não foi associada, entre em contato com o suporte.";
            else
                msg = ConfigurationManager.AppSettings["MensagemClienteBloqueadoEmLote"].ToString();
            return msg;
        }


        public int getFormaPagamentoIDValeIngresso()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["FormaPagamentoIDValeIngresso"]);
        }

        public string getAntiFraudeCanais()
        {
            return System.Configuration.ConfigurationManager.AppSettings["AntiFraudeCanais"];
        }

        public string getMsgSelecioneCliente()
        {
            return System.Configuration.ConfigurationManager.AppSettings["MensagemSelecioneCliente"];
        }

        public int getIDBilheteria()
        {
            return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IDRetiradaBilheteria"]);
        }

        public int getIDPDV()
        {
            return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IDRetiradaPdv"]);
        }

        public string getMensagemApresentacoes()
        {
            string msg = string.Empty;
            if (ConfigurationManager.AppSettings["MensagemApresentacoes"] == null)
                msg = "Seu limite de compra para as seguintes apasentações foi excedido. ";
            else
                msg = ConfigurationManager.AppSettings["MensagemApresentacoes"].ToString();
            return msg;
        }

        public decimal getValorMaximoComprasCanaisRisco()
        {
            decimal valor = 0;
            if (ConfigurationManager.AppSettings["ValorMaximoComprasCanaisRisco"] == null)
                valor = 2500;
            else
                valor = Convert.ToDecimal(ConfigurationManager.AppSettings["ValorMaximoComprasCanaisRisco"]);

            return valor;
        }

        public List<int> getSpecialEventIDs()
        {
            try
            {
                List<int> ids = new List<int>();
                string specialIDS = ConfiguracaoParalela.GetString(ConfiguracaoParalela.Keys.SpecialEventsID, "default");

                if (!string.IsNullOrEmpty(specialIDS))
                {
                    foreach (string id in specialIDS.Split(','))
                        ids.Add(Convert.ToInt32(id));
                }
                return ids;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool getSingleTonObjectsAtivo()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["SingleTonObjectsAtivo"]);
        }

        public bool BloqueioMultiplasComprasApresentacaoAtivo()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["BloqueioMultiplasComprasApresentacao"]);
        }

        public int getCanalCallCenterID()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["CanalCallCenter"]);
        }

        public int getCanalInternetID()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["CanalInternet"]);

        }
        public int getCanalMobileID()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["CanalMobile"]);

        }

        public string getMensagemSetorAlterado()
        {
            if (ConfigurationManager.AppSettings["MensagemSetorAlterado"] != null)
                return ConfigurationManager.AppSettings["MensagemSetorAlterado"].ToString();
            else
                return "Caso este setor esteja disponível para Internet, será necessário passar por aprovação do Atendimento.";
        }

        public int TamanhoMaximoBG()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["TamanhoMaximoBG"]);
        }

        public int TamanhoMaximoYSite()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["TamanhoMaximoYSite"]);
        }

        public int TamanhoMaximoXSite()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["TamanhoMaximoXSite"]);
        }

        public int TamanhoMaximoEsquematico()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["TamanhoMaximoEsquematico"]);
        }
        public int TamanhoMaximoYEsquematico()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["TamanhoMaximoYEsquematico"]);
        }
        public int TamanhoMaximoPerspectiva()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["TamanhoMaximoPerspectiva"]);
        }

        public string getProcedimentoTrocaPadrao()
        {
            return ConfigurationManager.AppSettings["VIRProcedimentoTroca"];
        }

        public string getSaudacaoPadrao()
        {
            return ConfigurationManager.AppSettings["VIRSaudacaoPadrao"];
        }
        public string getSaudacaoNominalPadrao()
        {
            return ConfigurationManager.AppSettings["VIRSaudacaoNominal"];
        }
        public bool ImprimirSomenteDataApresentacao()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["ImprimirSomenteDataApresentacao"]);
        }

        public string getBannerPath()
        {
            return ConfigurationManager.AppSettings["DownloadPathBanners"];
        }

        public bool getNaoImprimirCpf()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["NaoImprimirCpf"]);
        }

        public string ImagemChat(Enumerators.TipoChat tipo)
        {
            string img = string.Empty;
            switch (tipo)
            {
                case Enumerators.TipoChat.IR:
                    img = ConfigurationManager.AppSettings["ImagemChatIR"];
                    break;
                case Enumerators.TipoChat.NIR:
                    img = ConfigurationManager.AppSettings["ImagemChatNIR"];
                    break;
                default:
                    break;
            }
            return img.Replace('*', '&');
        }

        public string LinkChat(Enumerators.TipoChat tipo)
        {
            string img = string.Empty;
            switch (tipo)
            {
                case Enumerators.TipoChat.IR:
                    img = ConfigurationManager.AppSettings["LinkChatIR"];
                    break;
                case Enumerators.TipoChat.NIR:
                    img = ConfigurationManager.AppSettings["LinkChatNIR"];
                    break;
                default:
                    break;
            }
            return img.Replace('*', '&');
        }

        public int getPedidoCancelamento()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["PedidoCancelamentoID"]);
        }

        public int getInconsistenciaCancelamento()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["InconsistenciaCancelamentoID"]);
        }

        public DateTime Data()
        {
            return DateTime.Now;
        }

        public string getEnderecoDestaque()
        {
            return ConfigurationManager.AppSettings["DownloadPathDestaque"];
        }

        public double PorcentagemListaBranca()
        {
            return Convert.ToDouble(ConfigurationManager.AppSettings["PorcentagemListaBraca"]);
        }

        public int MinimoCodigoBarra()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["QuantidadeMinimaCodigoBarra"]);
        }

        public bool GerarCodigoAntigo()
        {
            return Convert.ToBoolean(ConfigurationManager.AppSettings["GerarCodigoAntigo"]);
        }

        public string getDownloadPathEventosPos()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["DownloadPathEventosPos"]);
        }

        public string getPathEventosPos()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["PathEventosPos"]);
        }
    }
}
