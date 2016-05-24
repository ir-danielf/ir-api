using System;
using System.Configuration;

namespace IRLib.Paralela
{
    public class ConfiguracaoAccertify : ConfigurationSection
    {
        private static ConfiguracaoAccertify instancia = null;
        public static ConfiguracaoAccertify Instancia
        {
            get
            {
                try
                {
                    if (instancia == null)
                        instancia = (ConfiguracaoAccertify)ConfigurationManager.GetSection("ConfiguracaoAccertify");

                    return instancia;
                }
                catch (Exception ex)
                {
                    throw new Exception("O arquivo de configuração não contém a seção: ConfiguracaoAccertify: " + ex.Message);
                }
            }
        }

        [ConfigurationProperty("Chaves")]
        public ChavesAccertify Chaves
        {
            get { return (ChavesAccertify)base["Chaves"]; }
            set { base["Chaves"] = value; }
        }
    }

    public class ChavesAccertify : ConfigurationElement
    {
        [ConfigurationProperty("Ativo")]
        public AccertifyAtivo Ativo
        {
            get { return (AccertifyAtivo)base["Ativo"]; }
            set { base["Ativo"] = value; }
        }

        [ConfigurationProperty("ScoreAltoRisco")]
        public AccertifyScoreAltoRisco ScoreAltoRisco
        {
            get { return (AccertifyScoreAltoRisco)base["ScoreAltoRisco"]; }
            set { base["ScoreAltoRisco"] = value; }
        }

        [ConfigurationProperty("ScoreReview")]
        public AccertifyScoreReview ScoreReview
        {
            get { return (AccertifyScoreReview)base["ScoreReview"]; }
            set { base["ScoreReview"] = value; }
        }

        [ConfigurationProperty("URL")]
        public AccertifyURL URL
        {
            get { return (AccertifyURL)base["URL"]; }
            set { base["URL"] = value; }
        }

        [ConfigurationProperty("Usuario")]
        public AccertifyUsuario Usuario
        {
            get { return (AccertifyUsuario)base["Usuario"]; }
            set { base["Usuario"] = value; }
        }

        [ConfigurationProperty("Senha")]
        public AccertifySenha Senha
        {
            get { return (AccertifySenha)base["Senha"]; }
            set { base["Senha"] = value; }
        }

        [ConfigurationProperty("ScoreAceitarTempoExcedido")]
        public AccertifyScoreAceitarTempoExcedido ScoreAceitarTempoExcedido
        {
            get { return (AccertifyScoreAceitarTempoExcedido)base["ScoreAceitarTempoExcedido"]; }
            set { base["ScoreAceitarTempoExcedido"] = value; }
        }

        [ConfigurationProperty("UsuarioID")]
        public AccertifyUsuarioID UsuarioID
        {
            get { return (AccertifyUsuarioID)base["UsuarioID"]; }
            set { base["UsuarioID"] = value; }
        }

        [ConfigurationProperty("ListenerUsuario")]
        public AccertifyListenerUsuario ListenerUsuario
        {
            get { return (AccertifyListenerUsuario)base["ListenerUsuario"]; }
            set { base["ListenerUsuario"] = value; }
        }

        [ConfigurationProperty("ListenerSenha")]
        public AccertifyListenerUsuario ListenerSenha
        {
            get { return (AccertifyListenerUsuario)base["ListenerSenha"]; }
            set { base["ListenerSenha"] = value; }
        }

        [ConfigurationProperty("ValorPeanut")]
        public AccertifyValorPeanut ValorPeanut
        {
            get { return (AccertifyValorPeanut)base["ValorPeanut"]; }
            set { base["ValorPeanut"] = value; }
        }


    }

    public class AccertifyAtivo : BaseElementBOOL { }
    public class AccertifyScoreAltoRisco : BaseElementINT { }
    public class AccertifyScoreReview : BaseElementINT { }
    public class AccertifyURL : BaseElementSTR { }
    public class AccertifyUsuario : BaseElementSTR { }
    public class AccertifySenha : BaseElementSTR { }
    public class AccertifyScoreAceitarTempoExcedido : BaseElementINT { }
    public class AccertifyUsuarioID : BaseElementINT { }
    public class AccertifyListenerUsuario: BaseElementSTR { }
    public class AccertifyListenerSenha : BaseElementSTR { }
    public class AccertifyValorPeanut : BaseElementDECIMAL{ }
    

}
