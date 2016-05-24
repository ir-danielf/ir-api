using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaIngressoTipoImpressao
    {
        public int IngressoID { get; set; }
        public EstruturaTipoImpressao.TipoImpressao TipoImpressao { get; set; }
    }
}
