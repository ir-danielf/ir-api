using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaValeIngresso
    {
        public int IngressoID { get; set; }
        public decimal Valor { get; set; }
        public int FormaPagamentoDias { get; set; }
        public decimal FormaPagamentoTaxaAdm { get; set; }
        public bool FormaPagamentoIR { get; set; }
        
        public int FormaPagamentoID
        {
			get { return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["FormaPagamentoIDValeIngresso"]); }

        }
        public int EventoVirID
        {
            get { return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EventoIDValeIngresso"]); }
        }
    }
}
