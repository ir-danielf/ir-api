using CTLib;
using IRLib.IRConfigs;
using System;
using System.Configuration;

namespace IRLib.Paralela
{
    [ObjectType(ObjectType.RemotingType.SingleCall)]
    public class ConfiguracaoParalela : MarshalByRefObject
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
            var servico = new ConfigServiceSoapClient();
            var res = servico.BuscarConfiguracao(key, version);
            servico.Close();

            return res.ConfigValue;
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
            CarregarTodos
        }
    }
}
