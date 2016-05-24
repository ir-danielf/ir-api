using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaBoleto
    {
        public int ID { get; set; }
        public string Assinante { get; set; }
        public string CPF { get; set; }
        public int ClienteID { get; set; }
        public string SenhaVenda { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataConfirmacao { get; set; }
        public DateTime DataPagamento { get; set; }
        public FormaPagamento.ESTADO EstadoPagamento { get; set; }
        public bool Impresso { get; set; }
        public bool Cancelado { get; set; }
        public string BoletoCancelado
        {
            get
            {
                return Cancelado ? "Sim" : "Não";
            }
        }
        public string AssinaturaNome { get; set; }
        public bool Selecionar { get; set; }

    }



}
