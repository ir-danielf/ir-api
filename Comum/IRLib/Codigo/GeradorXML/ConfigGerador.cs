using System;
using System.Collections.Generic;
using System.Configuration;

namespace IRLib
{
    public class ConfigGerador : ConfigurationSection
    {
        public static ConfigGerador Config
        {
            get
            {
                return (ConfigGerador)ConfigurationManager.GetSection("gerador");
            }
        }

        [ConfigurationProperty("bondinho")]
        public ConfigSection Bondinho
        {
            get { return (ConfigSection)base["bondinho"]; }
            set { base["bondinho"] = value; }
        }

        [ConfigurationProperty("morrodaurca")]
        public ConfigSection Morrodaurca
        {
            get { return (ConfigSection)base["morrodaurca"]; }
            set { base["morrodaurca"] = value; }
        }

        [ConfigurationProperty("fontenova")]
        public ConfigSection Fontenova
        {
            get { return (ConfigSection)base["fontenova"]; }
            set { base["fontenova"] = value; }
        }
    }

    public class ConfigSection : ConfigurationElement
    {
        [ConfigurationProperty("Nome")]
        public BaseElementString Nome
        {
            get { return (BaseElementString)base["Nome"]; }
            set { base["Nome"] = value; }
        }

        [ConfigurationProperty("Ativo")]
        public BaseElementBool Ativo
        {
            get { return (BaseElementBool)base["Ativo"]; }
            set { base["Ativo"] = value; }
        }

        [ConfigurationProperty("Version")]
        public BaseElementString Version
        {
            get { return (BaseElementString)base["Version"]; }
            set { base["Version"] = value; }
        }

        [ConfigurationProperty("Issuer")]
        public BaseElementInt Issuer
        {
            get { return (BaseElementInt)base["Issuer"]; }
            set { base["Issuer"] = value; }
        }

        [ConfigurationProperty("ID")]
        public BaseElementInt ID
        {
            get { return (BaseElementInt)base["ID"]; }
            set { base["ID"] = value; }
        }

        [ConfigurationProperty("EventID")]
        public BaseElementInt EventID
        {
            get { return (BaseElementInt)base["EventID"]; }
            set { base["EventID"] = value; }
        }        

        [ConfigurationProperty("Receiver")]
        public BaseElementInt Receiver
        {
            get { return (BaseElementInt)base["Receiver"]; }
            set { base["Receiver"] = value; }
        }

        [ConfigurationProperty("AreaID")]
        public BaseElementString AreaID
        {
            get { return (BaseElementString)base["AreaID"]; }
            set { base["AreaID"] = value; }
        }

        [ConfigurationProperty("PersonCategoryID")]
        public BaseElementString PersonCategoryID
        {
            get { return (BaseElementString)base["PersonCategoryID"]; }
            set { base["PersonCategoryID"] = value; }
        }

        [ConfigurationProperty("SeassonPassID")]
        public BaseElementString SeassonPassID
        {
            get { return (BaseElementString)base["SeassonPassID"]; }
            set { base["SeassonPassID"] = value; }
        }

        [ConfigurationProperty("TicketTypeID")]
        public BaseElementInt TicketTypeID
        {
            get { return (BaseElementInt)base["TicketTypeID"]; }
            set { base["TicketTypeID"] = value; }
        }

        [ConfigurationProperty("LocalID")]
        public BaseElementInt LocalID
        {
            get { return (BaseElementInt)base["LocalID"]; }
            set { base["LocalID"] = value; }
        }

        [ConfigurationProperty("Coding")]
        public BaseElementString Coding
        {
            get { return (BaseElementString)base["Coding"]; }
            set { base["Coding"] = value; }
        }

        [ConfigurationProperty("Port")]
        public BaseElementString Port
        {
            get { return (BaseElementString)base["Port"]; }
            set { base["Port"] = value; }
        }

        [ConfigurationProperty("Host")]
        public BaseElementString Host
        {
            get { return (BaseElementString)base["Host"]; }
            set { base["Host"] = value; }
        }

        [ConfigurationProperty("Username")]
        public BaseElementString Username
        {
            get { return (BaseElementString)base["Username"]; }
            set { base["Username"] = value; }
        }

        [ConfigurationProperty("Password")]
        public BaseElementString Password
        {
            get { return (BaseElementString)base["Password"]; }
            set { base["Password"] = value; }
        }

        [ConfigurationProperty("Workingdirectory")]
        public BaseElementString Workingdirectory
        {
            get { return (BaseElementString)base["Workingdirectory"]; }
            set { base["Workingdirectory"] = value; }
        }
    }

    public class BaseElementString : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return this["Valor"] == null ? string.Empty : this["Valor"].ToString(); }
            set { this["Valor"] = value; }
        }
    }

    public class BaseElementInt : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return Convert.ToInt32(this["Valor"]); }
            set { this["Valor"] = value; }
        }
    }

    public class BaseElementBool : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public bool Valor
        {
            get { return Convert.ToBoolean(this["Valor"]); }
            set { this["Valor"] = value; }
        }
    }

    public class BaseElementDecimal : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public decimal Valor
        {
            get { return Convert.ToDecimal(this["Valor"]); }
            set { this["Valor"] = value; }
        }
    }
}
