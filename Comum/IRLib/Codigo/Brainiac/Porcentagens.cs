using System.Configuration;

namespace IRLib.Codigo.Brainiac
{
    public class Porcentagens : ConfigurationSection
    {
        public static Porcentagens instancia = null;
        public static Porcentagens Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = (Porcentagens)ConfigurationManager.GetSection("Porcentagens");

                return instancia;
            }
        }

        [ConfigurationProperty("Elementos")]
        public PorcentagensElement Elementos
        {
            get { return (PorcentagensElement)base["Elementos"]; }
            set { base["Elementos"] = value; }
        }


    }

    public class PorcentagensElement : ConfigurationElement
    {
        [ConfigurationProperty("PorcentagemMinimaNomes", IsRequired = true)]
        public float PorcentagemNomes
        {
            get { return (float)base["PorcentagemMinimaNomes"]; }
            set { base["PorcentagemMinimaNomes"] = value; }
        }

        [ConfigurationProperty("PorcentagemNome", IsRequired = true)]
        public float PorcentagemAcertosPorNome
        {
            get { return (float)base["PorcentagemMinimaNomes"]; }
            set { base["PorcentagemMinimaNomes"] = value; }
        }

        [ConfigurationProperty("PorcentagemNomeRelevante", IsRequired = true)]
        public float PorcentagemAcertosPorNomeRelevante
        {
            get { return (float)base["PorcentagemNomeRelevante"]; }
            set { base["PorcentagemNomeRelevante"] = value; }
        }
        [ConfigurationProperty("PorcentagemMaximaSobra", IsRequired = true)]
        public float PorcentagemLimiteSobra
        {
            get { return (float)base["PorcentagemMaximaSobra"]; }
            set { base["PorcentagemMaximaSobra"] = value; }
        }
    }

}
