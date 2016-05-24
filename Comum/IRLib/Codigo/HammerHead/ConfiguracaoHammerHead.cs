using System;
using System.Configuration;

namespace IRLib
{
    public class ConfiguracaoHammerHead : ConfigurationSection
    {
        private static ConfiguracaoHammerHead instancia;
        public static ConfiguracaoHammerHead Instancia
        {
            get
            {
                try
                {
                    if (instancia == null)
                        instancia = (ConfiguracaoHammerHead)ConfigurationManager.GetSection("HammerHead");

                    return instancia;
                }
                catch (Exception ex)
                {
                    throw ex;// new Exception("O arquivo de configuração não contém a seção: ConfiguracaoHammerHead!");
                }
            }
        }

        [ConfigurationProperty("Configuracao")]
        public HH_Configuracao Configuracao
        {
            get { return (HH_Configuracao)base["Configuracao"]; }
            set { base["Configuracao"] = value; }
        }
    }

    public class HH_Configuracao : ConfigurationElement
    {
        [ConfigurationProperty("Timer")]
        public HH_Timer Timer
        {
            get { return (HH_Timer)base["Timer"]; }
            set { base["Timer"] = value; }
        }

        [ConfigurationProperty("LimiteAguardar")]
        public HH_LimiteAguardarHH LimiteAguardar
        {
            get { return (HH_LimiteAguardarHH)base["LimiteAguardar"]; }
            set { base["LimiteAguardar"] = value; }
        }

        [ConfigurationProperty("LimiteUtilizacaoCanais")]
        public HH_LimiteUtilizacaoCanais LimiteUtilizacaoCanais
        {
            get { return (HH_LimiteUtilizacaoCanais)base["LimiteUtilizacaoCanais"]; }
            set { base["LimiteUtilizacaoCanais"] = value; }
        }

        [ConfigurationProperty("IntervaloRequisicoes")]
        public HH_IntervaloRequisicoes IntervaloRequisicoes
        {
            get { return (HH_IntervaloRequisicoes)base["IntervaloRequisicoes"]; }
            set { base["IntervaloRequisicoes"] = value; }
        }

        [ConfigurationProperty("IntervaloRequisicoesTimeOut")]
        public HH_IntervaloRequisicoesTimeOut IntervaloRequisicoesTimeOu
        {
            get { return (HH_IntervaloRequisicoesTimeOut)base["IntervaloRequisicoesTimeOut"]; }
            set { base["IntervaloRequisicoesTimeOut"] = value; }
        }

        [ConfigurationProperty("TempoEstimadoExecucaoMS")]
        public HH_TempoEstimadoExecucaoMS TempoEstimadoExecucaoMS
        {
            get { return (HH_TempoEstimadoExecucaoMS)base["TempoEstimadoExecucaoMS"]; }
            set { base["TempoEstimadoExecucaoMS"] = value; }
        }

        [ConfigurationProperty("SalvarLog")]
        public HH_SalvarLog SalvarLog
        {
            get { return (HH_SalvarLog)base["SalvarLog"]; }
            set { base["SalvarLog"] = value; }
        }

        [ConfigurationProperty("CaminhoEmailAprovacao")]
        public HH_CaminhoEmailAprovacao CaminhoEmailAprovacao
        {
            get { return (HH_CaminhoEmailAprovacao)base["CaminhoEmailAprovacao"]; }
            set { base["CaminhoEmailAprovacao"] = value; }
        }


        [ConfigurationProperty("CaminhoEmailCancelamento")]
        public HH_CaminhoEmailCancelamento CaminhoEmailCancelamento
        {
            get { return (HH_CaminhoEmailCancelamento)base["CaminhoEmailCancelamento"]; }
            set { base["CaminhoEmailCancelamento"] = value; }
        }

        [ConfigurationProperty("AmbienteDeTestes")]
        public HH_AbianteDeTestes AmbienteDeTestes
        {
            get { return (HH_AbianteDeTestes)base["AmbienteDeTestes"]; }
            set { base["AmbienteDeTestes"] = value; }
        }

        [ConfigurationProperty("SalvarLogSQL")]
        public HH_AbianteDeTestes SalvarLogSQL
        {
            get { return (HH_AbianteDeTestes)base["SalvarLogSQL"]; }
            set { base["SalvarLogSQL"] = value; }
        }

        [ConfigurationProperty("SiteID")]
        public HH_SiteID SiteID
        {
            get { return (HH_SiteID)base["SiteID"]; }
            set { base["SiteID"] = value; }
        }

        [ConfigurationProperty("MaxThreads")]
        public HH_MaxThreads MaxThreads
        {
            get { return (HH_MaxThreads)base["MaxThreads"]; }
            set { base["MaxThreads"] = value; }
        }

        [ConfigurationProperty("ModoProcessamento")]
        public HH_ModoProcessamento ModoProcessamento
        {
            get { return (HH_ModoProcessamento)base["ModoProcessamento"]; }
            set { base["ModoProcessamento"] = value; }
        }
    }

    public class HH_Timer : BaseElementINT { }
    public class HH_LimiteAguardarHH : BaseElementINT { }
    public class HH_LimiteUtilizacaoCanais : BaseElementINT { }
    public class HH_IntervaloRequisicoes : BaseElementINT { }
    public class HH_IntervaloRequisicoesTimeOut : BaseElementINT { }
    public class HH_TempoEstimadoExecucaoMS : BaseElementINT { }
    public class HH_SalvarLog : BaseElementBOOL { }
    public class HH_CaminhoEmailAprovacao : BaseElementSTR { }
    public class HH_CaminhoEmailCancelamento : BaseElementSTR { }
    public class HH_AbianteDeTestes : BaseElementBOOL { }
    public class HH_SiteID : BaseElementInt { }
    public class HH_MaxThreads : BaseElementINT { }
    public class HH_ModoProcessamento : BaseElementSTR { }

    public abstract class BaseElementSTR : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public virtual string Valor
        {
            get { return this["Valor"] == null ? string.Empty : this["Valor"].ToString(); }
            set { this["Valor"] = value; }
        }
    }

    public abstract class BaseElementINT : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public virtual int Valor
        {
            get { return Convert.ToInt32(this["Valor"]); }
            set { this["Valor"] = value; }
        }
    }

    public abstract class BaseElementBOOL : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public virtual bool Valor
        {
            get { return Convert.ToBoolean(this["Valor"]); }
            set { this["Valor"] = value; }
        }
    }

    public abstract class BaseElementDECIMAL : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public virtual decimal Valor
        {
            get { return Convert.ToDecimal(this["Valor"]); }
            set { this["Valor"] = value; }
        }
    }

}
