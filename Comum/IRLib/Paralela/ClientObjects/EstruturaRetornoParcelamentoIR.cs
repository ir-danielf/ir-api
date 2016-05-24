using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoParcelamentoIR
    {
        public int Parcela { get; set; }
        public string ValorParcela { get; set; }
        public string ValorTotalParcelado { get; set; }
        public bool ParcelamentoEvento { get; set; }
    }
}