using System;
using System.Configuration;

namespace IRLib.Paralela
{
    public class ConfigurationFacebook : ConfigurationSection
    {
        private static ConfigurationFacebook instancia = null;
        public static ConfigurationFacebook Instancia
        {
            get
            {
                try
                {
                    if (instancia == null)
                        instancia = (ConfigurationFacebook)ConfigurationManager.GetSection("ConfigurationFacebook");

                    return instancia;
                }
                catch (Exception ex)
                {
                    throw new Exception("O arquivo de configuração não contém a seção: ConfigurationFacebook: " + ex.Message);
                }
            }
        }

        [ConfigurationProperty("Chaves")]
        public Chaves Chaves
        {
            get { return (Chaves)base["Chaves"]; }
            set { base["Chaves"] = value; }
        }
    }

    public class Chaves : ConfigurationElement
    {
        [ConfigurationProperty("API_Key")]
        public API_Key API_Key
        {
            get { return (API_Key)base["API_Key"]; }
            set { base["API_Key"] = value; }
        }

        [ConfigurationProperty("API_Secret")]
        public API_Secret API_Secret
        {
            get { return (API_Secret)base["API_Secret"]; }
            set { base["API_Secret"] = value; }
        }
    }

    public class API_Key : BaseElementSTR { }
    public class API_Secret : BaseElementSTR { }

}
