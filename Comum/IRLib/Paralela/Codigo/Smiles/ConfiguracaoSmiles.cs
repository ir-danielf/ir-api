using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace IRLib.Paralela
{
    public class ConfiguracaoSmiles : ConfigurationSection
    {
        private static ConfiguracaoSmiles instancia = null;

        public static ConfiguracaoSmiles Instancia
        {
            get
            {
                try
                {
                    if (instancia == null)
                        instancia = (ConfiguracaoSmiles)ConfigurationManager.GetSection("ConfiguracaoSmiles");

                    return instancia;
                }
                catch (Exception)
                {
                    throw new Exception("O arquivo de configuração não contém a seção: ConfiguracaoSmiles!");
                }
            }
        }

        [ConfigurationProperty("Chaves")]
        public ChavesSmiles Chaves
        {
            get { return (ChavesSmiles)base["Chaves"]; }
            set { base["Chaves"] = value; }
        }
    }

    public class ChavesSmiles : ConfigurationElement
    {
        [ConfigurationProperty("PartnerAlias_1")]
        public PartnerAlias_1 PartnerAlias_1
        {
            get { return (PartnerAlias_1)base["PartnerAlias_1"]; }
            set { base["PartnerAlias_1"] = value; }
        }

        [ConfigurationProperty("PartnerProductCode_1")]
        public PartnerProductCode_1 PartnerProductCode_1
        {
            get { return (PartnerProductCode_1)base["PartnerProductCode_1"]; }
            set { base["PartnerProductCode_1"] = value; }
        }

        [ConfigurationProperty("ItemDeliveryTime_1")]
        public ItemDeliveryTime_1 ItemDeliveryTime_1
        {
            get { return (ItemDeliveryTime_1)base["ItemDeliveryTime_1"]; }
            set { base["ItemDeliveryTime_1"] = value; }
        }

        [ConfigurationProperty("Return_URL")]
        public Return_URL Return_URL
        {
            get { return (Return_URL)base["Return_URL"]; }
            set { base["Return_URL"] = value; }
        }

        [ConfigurationProperty("SmilesProductName_1")]
        public SmilesProductName_1 SmilesProductName_1
        {
            get { return (SmilesProductName_1)base["SmilesProductName_1"]; }
            set { base["SmilesProductName_1"] = value; }
        }

        [ConfigurationProperty("Source")]
        public Source Source
        {
            get { return (Source)base["Source"]; }
            set { base["Source"] = value; }
        }

        [ConfigurationProperty("Redirect_URL")]
        public Redirect_URL Redirect_URL
        {
            get { return (Redirect_URL)base["Redirect_URL"]; }
            set { base["Redirect_URL"] = value; }
        }

        [ConfigurationProperty("Login")]
        public Login Login
        {
            get { return (Login)base["Login"]; }
            set { base["Login"] = value; }
        }

        [ConfigurationProperty("Password")]
        public Password Password
        {
            get { return (Password)base["Password"]; }
            set { base["Password"] = value; }
        }

        [ConfigurationProperty("Username")]
        public Username Username
        {
            get { return (Username)base["Username"]; }
            set { base["Username"] = value; }
        }

        [ConfigurationProperty("Divisor")]
        public Divisor Divisor
        {
            get { return (Divisor)base["Divisor"]; }
            set { base["Divisor"] = value; }
        }

        [ConfigurationProperty("MultiplicadorSmiles")]
        public MultiplicadorSmiles MultiplicadorSmiles
        {
            get { return (MultiplicadorSmiles)base["MultiplicadorSmiles"]; }
            set { base["MultiplicadorSmiles"] = value; }
        }

        [ConfigurationProperty("MultiplicadorClub")]
        public MultiplicadorClub MultiplicadorClub
        {
            get { return (MultiplicadorClub)base["MultiplicadorClub"]; }
            set { base["MultiplicadorClub"] = value; }
        }

        [ConfigurationProperty("Alias_spcPartner")]
        public Alias_spcPartner Alias_spcPartner
        {
            get { return (Alias_spcPartner)base["Alias_spcPartner"]; }
            set { base["Alias_spcPartner"] = value; }
        }

        [ConfigurationProperty("Owner_spcLogin")]
        public Owner_spcLogin Owner_spcLogin
        {
            get { return (Owner_spcLogin)base["Owner_spcLogin"]; }
            set { base["Owner_spcLogin"] = value; }
        }
    }

    public class PartnerAlias_1 : BaseElementSTR { }

    public class PartnerProductCode_1 : BaseElementSTR { }

    public class ItemDeliveryTime_1 : BaseElementSTR { }

    public class Return_URL : BaseElementSTR { }

    public class SmilesProductName_1 : BaseElementSTR { }

    public class Source : BaseElementSTR { }

    public class Redirect_URL : BaseElementSTR { }

    public class Password : BaseElementSTR { }

    public class Username : BaseElementSTR { }

    public class Login : BaseElementSTR { }

    public class Divisor : BaseElementINT { }

    public class MultiplicadorSmiles : BaseElementINT { }

    public class MultiplicadorClub : BaseElementINT { }

    public class Alias_spcPartner : BaseElementSTR { }

    public class Owner_spcLogin : BaseElementSTR { }

}
