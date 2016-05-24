using System;

namespace IRLib.Assinaturas.Models
{
    public class Pagamento
    {
        public Pagamento() { }

        public int ID { get; set; }
        public int VendaBilheteriaFormaPagamentoID { get; set; }
        public decimal Valor { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataConfirmacao { get; set; }
        public decimal ValorPago { get; set; }
        public string FormaPagamento { get; set; }
        public string EstadoPagamento { get; set; }
        public int FormaPagamentoID { get; set; }
        public int EstadoPagamentoID { get; set; }
    }
}
