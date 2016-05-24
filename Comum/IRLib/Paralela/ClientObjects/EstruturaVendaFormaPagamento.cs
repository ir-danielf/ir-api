using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaVendaFormaPagamento
    {
        public int FormaPagamentoID { get; set; }
        public int Tipo { get; set; }
        public int BandeiraID { get; set; }
        public decimal TaxaAdm { get; set; }
        public int Dias { get; set; }
        public string Padrao { get; set; }
        public int FormaPagamentoTipoID { get; set; }
        public bool IR { get; set; }


    }
}
