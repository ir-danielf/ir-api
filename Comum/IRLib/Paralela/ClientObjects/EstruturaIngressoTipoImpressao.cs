using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaIngressoTipoImpressao
    {
        public int IngressoID { get; set; }
        public EstruturaTipoImpressao.TipoImpressao TipoImpressao { get; set; }
    }
}
