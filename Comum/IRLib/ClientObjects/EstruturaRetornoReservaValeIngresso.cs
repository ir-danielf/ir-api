using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaRetornoReservaValeIngresso
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public DateTime Validade { get; set; }
        public string CodigoTroca { get; set; }
        public int ValidadeDias { get; set; }
        public string ClienteNome { get; set; }
        public int ValeIngressoTipoID { get; set; }
        public int IndiceBufferValeIngressoTipoID { get; set; }
        public decimal ValorPagamento { get; set; }
        public char ValorTipo { get; set; }
        public bool TrocaConveniencia { get; set; }
        public bool TrocaEntrega { get; set; }
        public bool TrocaIngresso { get; set; }
    }
}
