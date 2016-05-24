using System;

namespace IRLib.Assinaturas.Models
{
    public class Fase
    {
        public string FaseDescricao { get; set; }
        public string FaseDescritivoDesmonstrativo
        {
            get
            {
                switch (this.FaseAssinatura)
                {

                    case IRLib.Utils.Enums.EnumFaseAssinatura.TrocaPrioritaria:
                        return string.Format("Lembramos que o seu período para troca(s) será de {0} à {1}.", DataInicialString, DataFinalString);
                    case IRLib.Utils.Enums.EnumFaseAssinatura.Troca:
                        return string.Format("Lembramos que o seu período para troca(s) será de {0} à {1}.", DataInicialString, DataFinalString);
                    case IRLib.Utils.Enums.EnumFaseAssinatura.Aquisicoes:
                        return string.Format("Lembramos que o perído para novas aquisições será de {0} à {1}.", DataInicialString, DataFinalString);
                    case IRLib.Utils.Enums.EnumFaseAssinatura.Invalido:
                    case IRLib.Utils.Enums.EnumFaseAssinatura.Renovacao:
                    default:
                        return string.Empty;
                }
            }
        }

        public DateTime? DataInicial { private get; set; }
        public string DataInicialString
        {
            get
            {
                if (this.DataInicial == null)
                    return "--";

                return ((DateTime)this.DataInicial).ToString("dd/MM/yyyy");
            }
        }

        public DateTime? DataFinal { private get; set; }
        public string DataFinalString
        {
            get
            {
                if (this.DataFinal == null)
                    return "--";

                return ((DateTime)this.DataFinal).ToString("dd/MM/yyyy");
            }
        }

        public IRLib.Utils.Enums.EnumFaseAssinatura FaseAssinatura { get; set; }


    }
}
