using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaFormaPagamentoCotaItem
    {
        public int CotaItemID { get; set; }
        public int FormaPagamentoID { get; set; }
    }


    public class EstruturaVendaFormaPagamentoSerie
    {
        public int SerieID { get; set; }
        public int FormaPagamentoID { get; set; }
    }
}
