using System;

namespace IRLib.Assinaturas.Models
{
    public class Boleto
    {
        public Boleto() { }

        public int ID { get; set; }
        public int VendaBilheteriaFormaPagamentoID { get; set; }
        public decimal Valor { get; set; }
        public int Parcela { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public bool Impresso { get; set; }
    }
}
