using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaTrocaValeIngresso
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public DateTime Validade { get; set; }
        public string CodigoTroca { get; set; }
        public string CodigoTrocaFixo { get; set; }
        public int ValidadeDias { get; set; }
        public string Acumulativo { get; set; }
        public bool Encontrado { get; set; }
        public string Status { get; set; }
        public bool TrocaEntrega { get; set; }
        public bool TrocaIngresso { get; set; }
        public bool TrocaConveniencia { get; set; }
        public char ValorTipo { get; set; }
        public decimal ValorPagamento { get; set; }

    }
}
