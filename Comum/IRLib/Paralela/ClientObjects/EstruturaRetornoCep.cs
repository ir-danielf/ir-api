using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoCep
    {
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public int CidadeID { get; set; }
        public int EstadoID { get; set; }
    }
}
