using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCelular
    {
        public int ModelID { get; set; }
        public string Frabricante { get; set; }
        public int DDD { get; set; }
        public int Area { get; set; }
        public int Numero { get; set; }
    }
}
