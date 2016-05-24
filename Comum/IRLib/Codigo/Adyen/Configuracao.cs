using System;
using System.Configuration;

namespace IRLib
{
    public class ConfiguracaoAdyen : ConfigurationSection
    {
        private static ConfiguracaoAdyen instancia = null;

        public static ConfiguracaoAdyen Instancia
        {
            get
            {
                try
                {
                    if (instancia == null)
                        instancia = (ConfiguracaoAdyen)ConfigurationManager.GetSection("ConfiguracaoAdyen");

                    return instancia;
                }
                catch (Exception)
                {
                    throw new Exception("O arquivo de configuração não contém a seção: ConfiguracaoAdyen!");
                }
            }
        }

        [ConfigurationProperty("Chaves")]
        public ChavesAdyen Chaves
        {
            get { return (ChavesAdyen)base["Chaves"]; }
            set { base["Chaves"] = value; }
        }
    }

    public class ChavesAdyen : ConfigurationElement
    {
        [ConfigurationProperty("MerchantAccount")]
        public MerchantAccountElement MerchantAccount
        {
            get { return (MerchantAccountElement)base["MerchantAccount"]; }
            set { base["MerchantAccount"] = value; }
        }

        [ConfigurationProperty("Username")]
        public UsernameElement Username
        {
            get { return (UsernameElement)base["Username"]; }
            set { base["Username"] = value; }
        }

        [ConfigurationProperty("Password")]
        public PasswordElement Password
        {
            get { return (PasswordElement)base["Password"]; }
            set { base["Password"] = value; }
        }

        [ConfigurationProperty("Currency")]
        public CurrencyElement Currency
        {
            get { return (CurrencyElement)base["Currency"]; }
            set { base["Currency"] = value; }
        }

        [ConfigurationProperty("Ativo")]
        public AtivoElement Ativo
        {
            get { return (AtivoElement)base["Ativo"]; }
            set { base["Ativo"] = value; }
        }

        [ConfigurationProperty("Teste")]
        public TesteElement Teste
        {
            get { return (TesteElement)base["Teste"]; }
            set { base["Teste"] = value; }
        }

        [ConfigurationProperty("DiasProcessamento")]
        public DiasProcessamentoElement DiasProcessamento
        {
            get { return (DiasProcessamentoElement)base["DiasProcessamento"]; }
            set { base["DiasProcessamento"] = value; }
        }

        [ConfigurationProperty("MensagemFraud")]
        public MensagemFraudElement MensagemFraud
        {
            get { return (MensagemFraudElement)base["MensagemFraud"]; }
            set { base["MensagemFraud"] = value; }
        }

        [ConfigurationProperty("MensagemRefused")]
        public MensagemRefusedElement MensagemRefused
        {
            get { return (MensagemRefusedElement)base["MensagemRefused"]; }
            set { base["MensagemRefused"] = value; }
        }

        [ConfigurationProperty("MensagemDefault")]
        public MensagemDefaultElement MensagemDefault
        {
            get { return (MensagemDefaultElement)base["MensagemDefault"]; }
            set { base["MensagemDefault"] = value; }
        }
    }

    public class MerchantAccountElement : BaseElementSTR { }

    public class UsernameElement : BaseElementSTR { }

    public class PasswordElement : BaseElementSTR { }

    public class CurrencyElement : BaseElementSTR { }

    public class AtivoElement : BaseElementBOOL { }

    public class TesteElement : BaseElementBOOL { }

    public class DiasProcessamentoElement : BaseElementINT { }

    public class MensagemFraudElement : BaseElementSTR { }

    public class MensagemRefusedElement : BaseElementSTR { }

    public class MensagemDefaultElement : BaseElementSTR { }
}