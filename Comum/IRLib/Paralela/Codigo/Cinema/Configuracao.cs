using System.Configuration;

namespace IRLib.Paralela
{
    public class ConfiguracaoCinema : ConfigurationSection
    {
        private static ConfiguracaoCinema instancia = null;
        public static ConfiguracaoCinema Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = (ConfiguracaoCinema)ConfigurationManager.GetSection("ConfiguracaoCinema");

                return instancia;
            }
        }

        [ConfigurationProperty("Empresa")]
        public EmpresaSection Empresa
        {
            get { return (EmpresaSection)base["Empresa"]; }
            set { base["Empresa"] = value; }
        }

        [ConfigurationProperty("Entrega")]
        public EntregaSection Entrega
        {
            get { return (EntregaSection)base["Entrega"]; }
            set { base["Entrega"] = value; }
        }

        [ConfigurationProperty("Precos")]
        public PrecosSection Precos
        {
            get { return (PrecosSection)base["Precos"]; }
            set { base["Precos"] = value; }
        }

        [ConfigurationProperty("Email")]
        public EmailSection Email
        {
            get { return (EmailSection)base["Email"]; }
            set { base["Email"] = value; }
        }

        [ConfigurationProperty("Categoria")]
        public CategoriaElement Categoria
        {
            get { return (CategoriaElement)base["Categoria"]; }
            set { base["Categoria"] = value; }
        }
    }

    public class EmpresaSection : ConfigurationElement
    {
        [ConfigurationProperty("EmpresaID")]
        public EmpresaIDElement EmpresaID
        {
            get { return (EmpresaIDElement)base["EmpresaID"]; }
            set { base["EmpresaID"] = value; }
        }
    }

    public class EntregaSection : ConfigurationElement
    {
        [ConfigurationProperty("Bilheteria")]
        public BilheteriaElement Bilheteria
        {
            get { return (BilheteriaElement)base["Bilheteria"]; }
            set { base["Bilheteria"] = value; }
        }

        [ConfigurationProperty("Impressao")]
        public ImpressaoElement Impressao
        {
            get { return (ImpressaoElement)base["Impressao"]; }
            set { base["Impressao"] = value; }
        }
    }

    public class PrecosSection : ConfigurationElement
    {
        [ConfigurationProperty("Distribuir")]
        public DistribuirPrecosElement Distribuir
        {
            get { return (DistribuirPrecosElement)base["Distribuir"]; }
            set { base["Distribuir"] = value; }
        }
    }

    public class EmailSection : ConfigurationElement
    {
        [ConfigurationProperty("Web")]
        public WebElement Web
        {
            get { return (WebElement)base["Web"]; }
            set { base["Web"] = value; }
        }
    }

    public class EmpresaIDElement : BaseElementINT { }
    public class BilheteriaElement : BaseElementINT { }
    public class ImpressaoElement : BaseElementINT { }
    public class DistribuirPrecosElement : BaseElementBOOL { }
    public class WebElement : BaseElementSTR { }
    public class CategoriaElement : BaseElementINT { }
}
