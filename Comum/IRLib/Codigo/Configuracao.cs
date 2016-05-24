using CTLib;
using IRLib.IRConfigs;
using System;
using System.Configuration;

namespace IRLib
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class Configuracao : MarshalByRefObject
    {
        [Serializable()]
        public class ConfiguracaoInfo
        {
            public bool Beta { get; set; }
            public bool BancoProducao { get; set; }
            public string MensagemBeta { get; set; }
        }

        public ConfiguracaoInfo Get()
        {
            ConfiguracaoInfo objConfiguracao = new ConfiguracaoInfo();
            objConfiguracao.BancoProducao = Convert.ToBoolean(ConfigurationManager.AppSettings["BancoProducao"]);
            objConfiguracao.Beta = Convert.ToBoolean(ConfigurationManager.AppSettings["Beta"]);
            objConfiguracao.MensagemBeta = ConfigurationManager.AppSettings["MensagemBeta"];
            return objConfiguracao;
        }

        public static bool GetBoolean(Keys key, string version = "default")
        {
            var valor = GetString(key, version);
            return Convert.ToBoolean(valor);
        }

        public static string GetString(Keys key, string version = "default")
        {
            return Get(key.ToString(), version);
        }

        public static string Get(string key, string version = "default")
        {
            try
            {
                var servico = new ConfigServiceSoapClient();
                var res = servico.BuscarConfiguracao(key, version);
                servico.Close();
                return res.ConfigValue;
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }

        }

        public static int GetInt(Keys key, string version = "default")
        {
            var valor = GetString(key, version);

            if (!string.IsNullOrEmpty(valor))
                return Convert.ToInt32(valor);

            return 0;
        }

        public enum Keys
        {
            CanaisDistribuicaoRealTime,
            UsuarioCEP,
            SenhaCEP,
            SpecialEventsID,
            PortaInicial,
            PortaFinal,
            PortaSpecialEvent,
            IPRemoting,
            QuantidadeDiasApolice,
            MensagemAvisoMondial,
            IPTEF,
            SingleTonAtivo,
            CanaisIDGeradoNoCadastro,
            CarregarTodos,

            EventoTipoCodigoBarra,
            EventoTipoImpressao,
            EventoVendaSemAlvara,
            PrecoCorID,
            URLJira,
            usuarioAPI,
            senhaAPI,
            CancelSemDevolucaoLiberada,
            IPAuttar,
            PortaAuttar,
            EstabelecimentoAuttar,
            LojaAuttar,
            TerminalAuttar,
            CriptografiaAuttar,
            LogAuttar,
            NumSitesAuttar,
            InterativoAuttar
                    
        }
    }
}
