using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoIngresso
    {
        public int IngressoID { get; set; }
        public string Lote { get; set; }
        public string Preco { get; set; }
        public int QuantidadeIntegrantes { get; set; }
    }
}
