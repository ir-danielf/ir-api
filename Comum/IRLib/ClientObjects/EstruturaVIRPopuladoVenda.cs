using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaImpressaoVir
    {
        public int ValeIngressoID { get; set; }
        public string ValeIngressoNome { get; set; }
        public ValeIngresso.enumStatus Status { get; set; }
        public string StatusDetalhado { get; set; }
        public int TipoTaxaEntrega { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorExibicao { get; set; }
        public int Porcentagem { get; set; }
        public string ClientePresenteado { get; set; }
        public string CodigoTroca { get; set; }
        public string CodigoBarra { get; set; }
        public string ProcedimentoTroca { get; set; }
        public string SaudacaoPadrao { get; set; }
        public string SaudacaoNominal { get; set; }
        public bool CodigoTrocaFixo { get; set; }
        public int ValidadeEmDiasImpressao { get; set; }
        public int IndiceBufferValeIngressoTipoID { get; set; }
        //Utilizado para buscar a imagem
        public int ValeIngressoTipoID { get; set; }
        public DateTime ValidadeData { get; set; }
        public string NomeCartao { get; set; }
        public decimal ValorPagamento { get; set; }
        public char ValorTipo { get; set; }
    }
}
