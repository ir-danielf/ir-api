using System.Configuration;

namespace IRLib.Paralela
{
    public class ConfiguracaoCriptografia : ConfigurationSection
    {
        private static ConfiguracaoCriptografia instancia = null;
        public static ConfiguracaoCriptografia Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = (ConfiguracaoCriptografia)ConfigurationManager.GetSection("ConfiguracaoCriptografia");

                return instancia;
            }
        }

        [ConfigurationProperty("Chaves")]
        public ChavesSection Chaves
        {
            get { return (ChavesSection)base["Chaves"]; }
            set { base["Chaves"] = value; }
        }
    }

    public class ChavesSection : ConfigurationElement
    {

        [ConfigurationProperty("Cartao")]
        public CartaoElement Cartao
        {
            get { return (CartaoElement)base["Cartao"]; }
            set { base["Cartao"] = value; }
        }

        [ConfigurationProperty("CVV")]
        public CVVElement CVV
        {
            get { return (CVVElement)base["CVV"]; }
            set { base["CVV"] = value; }
        }

        [ConfigurationProperty("Data")]
        public DataElement Data
        {
            get { return (DataElement)base["Data"]; }
            set { base["Data"] = value; }
        }

    }


    public class CartaoElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }

    public class CVVElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }

    public class DataElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
}
