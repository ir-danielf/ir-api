using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaEntregaPeriodo
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
    }
}
