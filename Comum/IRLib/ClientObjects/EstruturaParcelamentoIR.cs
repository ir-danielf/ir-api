using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaParcelamentoIR
    {
        public int ID { get; set; }
        public int Parcelas { get; set; }
        public decimal Coeficiente { get; set; }
    }
}