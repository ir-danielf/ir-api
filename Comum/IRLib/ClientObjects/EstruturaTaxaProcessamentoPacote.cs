
namespace IRLib.ClientObjects
{
    public class EstruturaTaxaProcessamentoPacote
    {
        public decimal TaxaProcessamento { get; set; }
        public int LimiteIngressosEvento { get; set; }
        public int LimiteIngressosEstado { get; set; }
        public bool PossuiTaxaProcessamento { get; set; }

        public string Estado { get; set; }
    }
}
