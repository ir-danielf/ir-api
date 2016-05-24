using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaPontoDeVenda
    {
        public int ID { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Cidade { get; set; }
        public string Nome { get; set; }
        public string Horario { get; set; }
        public string FormaPagamento { get; set; }
        public string Estado { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Distancia { get; set; }
    }
}
