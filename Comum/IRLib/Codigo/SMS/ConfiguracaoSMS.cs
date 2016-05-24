using System.Configuration;

namespace IRLib
{
    public class ConfiguracaoSMS : ConfigurationSection
    {
        private static ConfiguracaoSMS instancia = null;
        public static ConfiguracaoSMS Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = (ConfiguracaoSMS)ConfigurationManager.GetSection("ConfiguracaoSMS");

                return instancia;
            }
        }

        [ConfigurationProperty("Chaves")]
        public SMS_Chaves Chaves
        {
            get { return (SMS_Chaves)base["Chaves"]; }
            set { base["Chaves"] = value; }
        }
    }

    public class SMS_Chaves : ConfigurationElement
    {
        [ConfigurationProperty("Senha")]
        public SMS_Senha Senha
        {
            get { return (SMS_Senha)base["Senha"]; }
            set { base["Senha"] = value; }
        }

        [ConfigurationProperty("Codigo")]
        public SMS_Login Login
        {
            get { return (SMS_Login)base["Codigo"]; }
            set { base["Codigo"] = value; }
        }

        [ConfigurationProperty("Status")]
        public SMS_Status Status
        {
            get { return (SMS_Status)base["Status"]; }
            set { base["Status"] = value; }
        }

        [ConfigurationProperty("Corpo")]
        public SMS_Corpo Corpo
        {
            get { return (SMS_Corpo)base["Corpo"]; }
            set { base["Corpo"] = value; }
        }

        [ConfigurationProperty("Link")]
        public SMS_Link Link
        {
            get { return (SMS_Link)base["Link"]; }
            set { base["Link"] = value; }
        }

        [ConfigurationProperty("From")]
        public SMS_From From
        {
            get { return (SMS_From)base["From"]; }
            set { base["From"] = value; }
        }
    }


    public class SMS_Senha : BaseElementSTR { }
    public class SMS_Login : BaseElementSTR { }
    public class SMS_Status : BaseElementBOOL { }
    public class SMS_Corpo : BaseElementSTR { }
    public class SMS_Link : BaseElementSTR { }
    public class SMS_From : BaseElementSTR { }
}
