using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaValeIngressoTipo
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public int ValidadeDiasImpressao { get; set; }
        public DateTime ValidadeData { get; set; }
        public string CodigoTrocaFixo { get; set; }
        public ValeIngressoTipo.EnumClienteTipo ClienteTipo { get; set; }
        public string ProcedimentoTroca { get; set; }
        public string SaudacaoPadrao { get; set; }
        public string SaudacaoNominal { get; set; }
        public bool QuantidadeLimitada { get; set; }
        public int EmpresaID { get; set; }
        public int QuantidadeDisponivelTipo { get; set; }
        public bool Acumulativo { get; set; }
        public string Imagem { get; set; }
        public bool TrocaEntrega { get; set; }
        public bool TrocaIngresso { get; set; }
        public bool TrocaConveniencia { get; set; }
        public char ValorTipo { get; set; }
        public decimal ValorPagamento { get; set; }

        public bool PublicarInternet { get; set; }
        public string ReleaseInternet { get; set; }


    }
}
