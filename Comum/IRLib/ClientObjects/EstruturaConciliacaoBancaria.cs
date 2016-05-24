using System;

namespace IRLib.ClientObjects
{
	[Serializable]
    public class EstruturaConciliacaoBancaria
    {
        public string DataDeposito;
        public int FormaPagamentoID;
        public string FormaPagamento;
        public decimal ValorBruto;
        public decimal ValorLiquido;
    }
}
