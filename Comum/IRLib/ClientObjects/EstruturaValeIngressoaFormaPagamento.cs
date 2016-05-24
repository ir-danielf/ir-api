using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaValeIngressoaFormaPagamento
    {
        public int FormaPagamentoID
        {
            get { return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FormaPagamentoIDValeIngresso"]); }
        }
        public int Tipo { get; set; }
        public int BandeiraID { get; set; }
        public decimal TaxaAdm { get; set; }
        public int Dias { get; set; }
        public string Padrao { get; set; }
        public int FormaPagamentoTipoID { get; set; }
        public bool IR { get; set; }


    }
}
