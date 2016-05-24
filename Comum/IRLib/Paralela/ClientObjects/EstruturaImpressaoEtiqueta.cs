using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaImpressaoEtiqueta
    {
        public string Nome { get; set; }
        public string RG { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Senha { get; set; }
        public int QuantidadeImprimir { get; set; }
    }
}
