using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRLib.Codigo.NetPromoterService
{
    public class ConfiguracaoNetPromoterService : ConfigurationSection
    {

        private static ConfiguracaoNetPromoterService instancia;

        public static ConfiguracaoNetPromoterService Instancia
        {
            get
            {
                try
                {
                    if (instancia == null)
                        instancia = (ConfiguracaoNetPromoterService)ConfigurationManager.GetSection("NetPromoterService");

                    return instancia;
                }
                catch (Exception ex)
                {
                    throw ex;// new Exception("O arquivo de configuração não contém a seção: ConfiguracaoNetPromoterService!");
                }
            }
        }

        [ConfigurationProperty("Configuracao")]
        public NPS_Configuracao Configuracao
        {
            get { return (NPS_Configuracao)base["Configuracao"]; }
            set { base["Configuracao"] = value; }
        }
    }

    public class NPS_Configuracao : ConfigurationElement
    {
        [ConfigurationProperty("Username")]
        public NPS_Username Username
        {
            get { return (NPS_Username)base["Username"]; }
            set { base["Username"] = value; }
        }

        [ConfigurationProperty("Password")]
        public NPS_Password Password
        {
            get { return (NPS_Password)base["Password"]; }
            set { base["Password"] = value; }
        }
    }

    public class NPS_Username : BaseElementSTR { }

    public class NPS_Password : BaseElementSTR { }

    public abstract class BaseElementSTR : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public virtual string Valor
        {
            get { return this["Valor"] == null ? string.Empty : this["Valor"].ToString(); }
            set { this["Valor"] = value; }
        }
    }

}
