using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoRegistroImpressao
    {
        public int ID { get; set; }
        public string  CodigoBarra { get; set; }
        public int CodigoImpressao { get; set; }
        public string CodigoTrocaVir{ get; set; }
    }
}
