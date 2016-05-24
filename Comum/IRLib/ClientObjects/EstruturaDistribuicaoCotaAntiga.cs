using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaDistribuicaoCotaAntiga
    {
        public int ApresentacaoID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public bool TemAP { get; set; }
        public bool TemAPS { get; set; }
        public int CotaID { get; set; }
    }
}
