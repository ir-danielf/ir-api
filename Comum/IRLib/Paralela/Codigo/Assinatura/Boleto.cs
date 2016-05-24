using System.Configuration;


namespace IRLib.Paralela
{
    public class Boleto : ConfigurationSection
    {
        private static Boleto instancia = null;
        public static Boleto Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = (Boleto)ConfigurationManager.GetSection("boleto");

                return instancia;
            }
        }

        [ConfigurationProperty("info")]
        public InfoSection Info
        {
            get { return (InfoSection)base["info"]; }
            set { base["info"] = value; }
        }


    }

    #region /********************/ Seção de SEPARAÇÃO das áreas /********************/

    #region Info
    public class InfoSection : ConfigurationElement
    {
        [ConfigurationProperty("Identificacao")]
        public InfoIdentificacaoElement Identificacao
        {
            get { return (InfoIdentificacaoElement)base["Identificacao"]; }
            set { base["Identificacao"] = value; }
        }

        [ConfigurationProperty("BoletoGerenciado")]
        public InfoBoletoGerenciadoElement BoletoGerenciado
        {
            get { return (InfoBoletoGerenciadoElement)base["BoletoGerenciado"]; }
            set { base["BoletoGerenciado"] = value; }
        }

        [ConfigurationProperty("DisponivelCliente")]
        public InfoBoletoGerenciadoElement DisponivelCliente
        {
            get { return (InfoBoletoGerenciadoElement)base["DisponivelCliente"]; }
            set { base["DisponivelCliente"] = value; }
        }

        [ConfigurationProperty("Modulo")]
        public InfoModuloElement Modulo
        {
            get { return (InfoModuloElement)base["Modulo"]; }
            set { base["Modulo"] = value; }
        }

        [ConfigurationProperty("Ambiente")]
        public InfoAmbienteElement Ambiente
        {
            get { return (InfoAmbienteElement)base["Ambiente"]; }
            set { base["Ambiente"] = value; }
        }

        [ConfigurationProperty("Instr1")]
        public InfoInstr1Element Instr1
        {
            get { return (InfoInstr1Element)base["Instr1"]; }
            set { base["Instr1"] = value; }
        }

        [ConfigurationProperty("Instr2")]
        public InfoInstr2Element Instr2
        {
            get { return (InfoInstr2Element)base["Instr2"]; }
            set { base["Instr2"] = value; }
        }

        [ConfigurationProperty("Instr3")]
        public InfoInstr3Element Instr3
        {
            get { return (InfoInstr3Element)base["Instr3"]; }
            set { base["Instr3"] = value; }
        }

        [ConfigurationProperty("Instr4")]
        public InfoInstr4Element Instr4
        {
            get { return (InfoInstr4Element)base["Instr4"]; }
            set { base["Instr4"] = value; }
        }

        [ConfigurationProperty("Instr5")]
        public InfoInstr5Element Instr5
        {
            get { return (InfoInstr5Element)base["Instr5"]; }
            set { base["Instr5"] = value; }
        }

        [ConfigurationProperty("LogoLoja")]
        public InfoLogoLojaElement LogoLoja
        {
            get { return (InfoLogoLojaElement)base["LogoLoja"]; }
            set { base["LogoLoja"] = value; }
        }

        [ConfigurationProperty("TituloLoja")]
        public InfoTituloLojaElement TituloLoja
        {
            get { return (InfoTituloLojaElement)base["TituloLoja"]; }
            set { base["TituloLoja"] = value; }
        }

        [ConfigurationProperty("BotoesBoleto")]
        public InfoBotoesBoletoElement BotoesBoleto
        {
            get { return (InfoBotoesBoletoElement)base["BotoesBoleto"]; }
            set { base["BotoesBoleto"] = value; }
        }

        [ConfigurationProperty("UrlTopoLoja")]
        public InfoUrlTopoLojaElement UrlTopoLoja
        {
            get { return (InfoUrlTopoLojaElement)base["UrlTopoLoja"]; }
            set { base["UrlTopoLoja"] = value; }
        }

        [ConfigurationProperty("Cabecalho")]
        public InfoCabecalhoElement Cabecalho
        {
            get { return (InfoCabecalhoElement)base["Cabecalho"]; }
            set { base["Cabecalho"] = value; }
        }

        [ConfigurationProperty("LinkVisualizacao")]
        public InfoLinkVisualizacaoElement LinkVisualizacao
        {
            get { return (InfoLinkVisualizacaoElement)base["LinkVisualizacao"]; }
            set { base["LinkVisualizacao"] = value; }
        }


        [ConfigurationProperty("TemplateEmail")]
        public InfoTemplateEmailElement TemplateEmail
        {
            get { return (InfoTemplateEmailElement)base["TemplateEmail"]; }
            set { base["TemplateEmail"] = value; }
        }

        [ConfigurationProperty("DiasPrimeiroVencimento")]
        public InfoDiasPrimeiroVencimentoElement DiasPrimeiroVencimento
        {
            get { return (InfoDiasPrimeiroVencimentoElement)base["DiasPrimeiroVencimento"]; }
            set { base["DiasPrimeiroVencimento"] = value; }
        }

        [ConfigurationProperty("EstornoTemplateEmail")]
        public InfoEstornoTemplateEmailElement EstornoTemplateEmail
        {
            get { return (InfoEstornoTemplateEmailElement)base["EstornoTemplateEmail"]; }
            set { base["EstornoTemplateEmail"] = value; }
        }
    }
    #endregion

    #endregion

    #region /********************/ Daqui pra baixo ficam as Seções inferiores que contém o VALOR /********************/

    #region Info
    public class InfoIdentificacaoElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }

    public class InfoBoletoGerenciadoElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoModuloElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoAmbienteElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoInstr1Element : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoInstr2Element : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoInstr3Element : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoInstr4Element : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoInstr5Element : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoLogoLojaElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoTituloLojaElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoBotoesBoletoElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoUrlTopoLojaElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoCabecalhoElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class InfoLinkVisualizacaoElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }

    public class InfoTemplateEmailElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }

    public class InfoDiasPrimeiroVencimentoElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }

    public class InfoEstornoTemplateEmailElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }

    #endregion

    #endregion
}
