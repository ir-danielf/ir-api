using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEventoLoja
    {
        public int ID { get; set; }
        public int EventoID { get; set; }
        public int LojaID { get; set; }
        public EstruturaTipoImpressao.TipoImpressao Tipo { get; set; }
        public string Evento { get; set; }
        public string TipoExibicao
        {
            get
            {
                switch (Tipo)
                {
                    case EstruturaTipoImpressao.TipoImpressao.Ambos:
                        return "Ambos";
                    case EstruturaTipoImpressao.TipoImpressao.Laser:
                        return "Laser";
                    case EstruturaTipoImpressao.TipoImpressao.Termica:
                        return "Térmica";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
