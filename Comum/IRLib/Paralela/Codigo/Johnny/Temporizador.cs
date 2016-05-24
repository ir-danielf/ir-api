using System.Configuration;


namespace IRLib.Paralela
{
    public class Temporizador : ConfigurationSection
    {
        private static Temporizador instancia = null;
        public static Temporizador Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = (Temporizador)ConfigurationManager.GetSection("Temporizador");

                return instancia;
            }
        }

        [ConfigurationProperty("Feedback")]
        public FeedbackSection FeedBack
        {
            get { return (FeedbackSection)base["Feedback"]; }
            set { base["Feedback"] = value; }
        }

        [ConfigurationProperty("Mensagens")]
        public MensagemSection Mensagens
        {
            get { return (MensagemSection)base["Mensagens"]; }
            set { base["Mensagens"] = value; }
        }

        [ConfigurationProperty("Menu")]
        public MenuSection Menu
        {
            get { return (MenuSection)base["Menu"]; }
            set { base["Menu"] = value; }
        }

        [ConfigurationProperty("CodigoBarra")]
        public CodigoBarraSection CodigoBarra
        {
            get { return (CodigoBarraSection)base["CodigoBarra"]; }
            set { base["CodigoBarra"] = value; }
        }

        [ConfigurationProperty("EmailAssinaturas")]
        public EmailAssinaturasSection EmailAssinaturas
        {
            get { return (EmailAssinaturasSection)base["EmailAssinaturas"]; }
            set { base["EmailAssinaturas"] = value; }
        }
    }

    #region /********************/ Seção de SEPARAÇÃO das áreas /********************/

    #region Feedback
    public class FeedbackSection : ConfigurationElement
    {
        [ConfigurationProperty("AtribuirHorario")]
        public FeedbackAtribuirHorarioElement AtribuirHorario
        {
            get { return (FeedbackAtribuirHorarioElement)base["AtribuirHorario"]; }
            set { base["AtribuirHorario"] = value; }
        }

        [ConfigurationProperty("QuantidadePorHorario")]
        public FeedbackQuantidadePorHorarioElement QuantidadePorHorario
        {
            get { return (FeedbackQuantidadePorHorarioElement)base["QuantidadePorHorario"]; }
            set { base["QuantidadePorHorario"] = value; }
        }

        [ConfigurationProperty("HorasAposApresentacao")]
        public FeedbackHorasAposApresentacaoElement HorasAposApresentacao
        {
            get { return (FeedbackHorasAposApresentacaoElement)base["HorasAposApresentacao"]; }
            set { base["HorasAposApresentacao"] = value; }
        }

        [ConfigurationProperty("EnviarFeedback")]
        public FeedbackEnviarFeedbackElement EnviarFeedback
        {
            get { return (FeedbackEnviarFeedbackElement)base["EnviarFeedback"]; }
            set { base["EnviarFeedback"] = value; }
        }

        [ConfigurationProperty("QuantidadeDeBusca")]
        public FeedbackQuantidadeDeBuscaElement QuantidadeDeBusca
        {
            get { return (FeedbackQuantidadeDeBuscaElement)base["QuantidadeDeBusca"]; }
            set { base["QuantidadeDeBusca"] = value; }
        }

        [ConfigurationProperty("Respiro")]
        public FeedbackRespiroElement Respiro
        {
            get { return (FeedbackRespiroElement)base["Respiro"]; }
            set { base["Respiro"] = value; }
        }

        [ConfigurationProperty("Quantidade")]
        public FeedbackQuantidadeElement Quantidade
        {
            get { return (FeedbackQuantidadeElement)base["Quantidade"]; }
            set { base["Quantidade"] = value; }
        }

    }
    #endregion

    #region Mensagem
    public class MensagemSection : ConfigurationElement
    {
        [ConfigurationProperty("CarregarMensagens")]
        public FeedbackAtribuirHorarioElement CarregarMensagens
        {
            get { return (FeedbackAtribuirHorarioElement)base["CarregarMensagens"]; }
            set { base["CarregarMensagens"] = value; }
        }

    }
    #endregion

    #region Menu

    public class MenuSection : ConfigurationElement
    {
        [ConfigurationProperty("Intervalo")]
        public MenuIntervaloElement Intervalo
        {
            get { return (MenuIntervaloElement)base["Intervalo"]; }
            set { base["Intervalo"] = value; }
        }

        [ConfigurationProperty("Local")]
        public MenuLocalElement Local
        {
            get { return (MenuLocalElement)base["Local"]; }
            set { base["Local"] = value; }
        }
    }
    #endregion

    #region Código de Barras
    public class CodigoBarraSection : ConfigurationElement
    {
        [ConfigurationProperty("Intervalo")]
        public Bar_IntervaloElement Intervalo
        {
            get { return (Bar_IntervaloElement)base["Intervalo"]; }
            set { base["Intervalo"] = value; }
        }

        [ConfigurationProperty("Telefones")]
        public Bar_Telefones Telefones
        {
            get { return (Bar_Telefones)base["Telefones"]; }
            set { base["Telefones"] = value; }
        }

        [ConfigurationProperty("CriarCodigos")]
        public Bar_CriarCodigos CriarCodigos
        {
            get { return (Bar_CriarCodigos)base["CriarCodigos"]; }
            set { base["CriarCodigos"] = value; }
        }

        [ConfigurationProperty("Minimo")]
        public Bar_Minimo Minimo
        {
            get { return (Bar_Minimo)base["Minimo"]; }
            set { base["Minimo"] = value; }
        }

    }
    #endregion

    #region Envio de Emails - Assinaturas

    public class EmailAssinaturasSection : ConfigurationElement
    {
        [ConfigurationProperty("Intervalo")]
        public Assint_IntervaloElement Intervalo
        {
            get { return (Assint_IntervaloElement)base["Intervalo"]; }
            set { base["Intervalo"] = value; }
        }

        [ConfigurationProperty("Respiro")]
        public Assint_RespiroElement Respiro
        {
            get { return (Assint_RespiroElement)base["Respiro"]; }
            set { base["Respiro"] = value; }
        }

    }

    #endregion

    #endregion

    #region /********************/ Daqui pra baixo ficam as Seções inferiores que contém o VALOR /********************/

    #region Feedback
    public class FeedbackAtribuirHorarioElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class FeedbackQuantidadePorHorarioElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class FeedbackEnviarFeedbackElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class FeedbackQuantidadeDeBuscaElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class FeedbackHorasAposApresentacaoElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class FeedbackRespiroElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class FeedbackQuantidadeElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    #endregion

    #region Mensagem
    public class CarregarMensagensElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    #endregion

    #region Menu
    public class MenuIntervaloElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public int Valor
        {
            get { return (int)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    public class MenuLocalElement : ConfigurationElement
    {
        [ConfigurationProperty("Valor")]
        public string Valor
        {
            get { return (string)this["Valor"]; }
            set { this["Valor"] = value; }
        }
    }
    #endregion

    #region Códigos de Barra

    public class Bar_IntervaloElement : BaseElementINT { }
    public class Bar_Telefones : BaseElementSTR { }
    public class Bar_CriarCodigos : BaseElementBOOL { }
    public class Bar_Minimo : BaseElementINT { }
    #endregion

    #region Assinaturas _ Envio de Emails

    public class Assint_IntervaloElement : BaseElementINT { }
    public class Assint_RespiroElement : BaseElementINT { }

    #endregion

    #endregion
}
