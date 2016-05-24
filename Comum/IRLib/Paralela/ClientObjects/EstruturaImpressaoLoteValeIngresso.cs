using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaImpressaoLoteValeIngresso
    {
        //Venda
        public int VemdaBilheteriaID { get; set; }
        public string Senha { get; set; }
        public string ClienteNome { get; set; }
        public string Canal { get; set; }
        public DateTime DataVenda { get; set; }
        public char StatusVenda { get; set; }
        public string CanalVenda { get; set; }
        public int TaxaEntregaID { get; set; }
        public string TaxaEntrega { get; set; }
        public int QuantidadeValeIngressos { get; set; }
        public string Loja { get; set; }

        //VIR
        public int ValeIngressoID { get; set; }
        public string CodigoBarra { get; set; }
        public string ProcedimentoTroca { get; set; }
        public string CodigoTroca { get; set; }
        public string ClientePresenteado { get; set; }
        public string SaudacaoPadrao { get; set; }
        public string SaudacaoNominal { get; set; }
        public bool CodigoTrocaFixo { get; set; }
        public int ValidadeEmDiasImpressao { get; set; }
        public decimal Valor { get; set; }
        public int ValeIngressoTipoID { get; set; }
        public string ValeIngressoNome { get; set; }
        public ValeIngresso.enumStatus Status { get; set; }
        public DateTime ValidadeData { get; set; }
    }
}
