using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaVendaValeIngresso
    {
        public int ID { get; set; }
        public decimal Valor { get; set; }
        public string CodigoTroca { get; set; }

    }
}
