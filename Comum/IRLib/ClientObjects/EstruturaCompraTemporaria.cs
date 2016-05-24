
namespace IRLib.ClientObjects
{
    public class EstruturaCompraTemporaria
    {
        public int ID { get; set; }
        public int ClienteID { get; set; }
        public int FormaPagamentoID { get; set; }
        public string FormaPagamento { get; set; }
        public int TaxaEntregaID { get; set; }
        public string TaxaEntrega { get; set; }
        public decimal TaxaEntregaValor { get; set; }
        public int Parcelas { get; set; }
        public bool SomenteCortesias { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal TotalIngressos { get; set; }
        public string Bandeira { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string CNPJ { get; set; }
        public int BIN { get; set; }
        public bool Encontrado { get; set; }
        public string CodigoTrocaFixo { get; set; }
        public bool SomenteVir { get; set; }
        public int EnderecoID { get; set; }
        public int PDVSelecionado { get; set; }
        public int EntregaControleIDSelecionado { get; set; }
        public string DataSelecionada { get; set; }
        public decimal EntregaValor { get; set; }
        public decimal ConvenienciaValor { get; set; }
        public string Evento { get; set; }
        public string Setor { get; set; }
        public string PrecoNome { get; set; }
        public string Codigo { get; set; }
    }
}
