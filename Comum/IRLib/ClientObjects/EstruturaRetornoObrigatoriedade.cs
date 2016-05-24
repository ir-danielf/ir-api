using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoObrigatoriedade
    {
        public EstruturaObrigatoriedadeSite Obrigatoriedade { get; set; }
        public EstruturaDonoIngressoSite DonoIngresso { get; set; }
        public int IngressoID { get; set; }
    }
}
