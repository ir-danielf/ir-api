using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCota
    {
        public bool Novo { get; set; }
        public int ID { get; set; }
        public string Nome { get; set; }
        public int LocalID { get; set; }
        public int Quantidade { get; set; }
        public int QuantidadePorCliente { get; set; }
    }
}
