using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRegistroImpressaoVir
    {
        public int valeIngressoID { get; set; }
        public ValeIngresso.enumStatus StatusAtual { get; set; }
        public string ClientePresenteado { get; set; }
        public int DiasImpressao { get; set; }
        public string DataExpiracao { get; set; }
        public bool CodigoTrocaFixo { get; set; }
        public string CodigoTroca { get; set; }

    }
}
