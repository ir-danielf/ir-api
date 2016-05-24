using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaFormaPagamentoCota
    {
        public int CotaID { get; set; }
        public int FormaPagamentoID { get; set; }
    }
}
