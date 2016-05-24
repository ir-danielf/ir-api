using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCotaItemControle
    {
        public int ID { get; set; }
        public int ApresentacaoID { get; set; }
        public int ApresentacaoSetorID { get; set; }
        public int Quantidade { get; set; }
        public int CotaItemID { get; set; }
    }
}
