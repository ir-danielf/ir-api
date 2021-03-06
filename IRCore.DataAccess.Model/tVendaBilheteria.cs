//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IRCore.DataAccess.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class tVendaBilheteria
    {
        public tVendaBilheteria()
        {
            this.tIngresso = new HashSet<tIngresso>();
            this.Voucher = new HashSet<Voucher>();
            this.tVendaBilheteriaItem = new HashSet<tVendaBilheteriaItem>();
            this.tVendaBilheteriaFormaPagamento = new HashSet<tVendaBilheteriaFormaPagamento>();
            this.EstornoDadosCartaoCredito = new HashSet<EstornoDadosCartaoCredito>();
            this.EstornoDadosCartaoCredito1 = new HashSet<EstornoDadosCartaoCredito>();
            this.EstornoDadosDepositoBancario = new HashSet<EstornoDadosDepositoBancario>();
            this.EstornoDadosDepositoBancario1 = new HashSet<EstornoDadosDepositoBancario>();
            this.EstornoDadosDinheiro = new HashSet<EstornoDadosDinheiro>();
            this.EstornoDadosDinheiro1 = new HashSet<EstornoDadosDinheiro>();
            this.tVendaBilheteriaEntrega = new HashSet<tVendaBilheteriaEntrega>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CaixaID { get; set; }
        public Nullable<int> ClienteID { get; set; }
        public string Senha { get; set; }
        public string DataVenda { get; set; }
        public string Status { get; set; }
        public Nullable<int> TaxaEntregaID { get; set; }
        public Nullable<decimal> TaxaEntregaValor { get; set; }
        public Nullable<decimal> TaxaConvenienciaValorTotal { get; set; }
        public Nullable<decimal> ValorTotal { get; set; }
        public string Obs { get; set; }
        public string NotaFiscalCliente { get; set; }
        public string NotaFiscalEstabelecimento { get; set; }
        public Nullable<decimal> IndiceInstituicaoTransacao { get; set; }
        public Nullable<decimal> IndiceTipoCartao { get; set; }
        public Nullable<decimal> NSUSitef { get; set; }
        public Nullable<decimal> NSUHost { get; set; }
        public Nullable<decimal> CodigoAutorizacaoCredito { get; set; }
        public Nullable<decimal> ModalidadePagamentoCodigo { get; set; }
        public string ModalidadePagamentoTexto { get; set; }
        public string BIN { get; set; }
        public string TipoCancelamento { get; set; }
        public Nullable<decimal> ComissaoValorTotal { get; set; }
        public string IR { get; set; }
        public string DataDeposito { get; set; }
        public Nullable<int> NumeroCelular { get; set; }
        public Nullable<int> ModelIDCelular { get; set; }
        public string FabricanteCelular { get; set; }
        public Nullable<int> DDD { get; set; }
        public Nullable<int> NivelRisco { get; set; }
        public string IP { get; set; }
        public string MensagemRetorno { get; set; }
        public string HoraTransacao { get; set; }
        public string DataTransacao { get; set; }
        public string CodigoIR { get; set; }
        public string NumeroAutorizacao { get; set; }
        public string Cupom { get; set; }
        public string DadosConfirmacaoVenda { get; set; }
        public Nullable<int> Rede { get; set; }
        public Nullable<int> CodigoRespostaTransacao { get; set; }
        public Nullable<int> CartaoID { get; set; }
        public Nullable<bool> Fraude { get; set; }
        public string AprovacaoAutomatica { get; set; }
        public Nullable<int> QuantidadeImpressoesInternet { get; set; }
        public string VendaCancelada { get; set; }
        public Nullable<int> MotivoID { get; set; }
        public Nullable<int> ClienteEnderecoID { get; set; }
        public Nullable<int> EntregaControleID { get; set; }
        public Nullable<int> EntregaAgendaID { get; set; }
        public Nullable<int> PdvID { get; set; }
        public string EmailSincronizado { get; set; }
        public string EmailEnviado { get; set; }
        public string EmailEnviar { get; set; }
        public string FeedbackPosVenda { get; set; }
        public string PagamentoProcessado { get; set; }
        public string NomeCartao { get; set; }
        public Nullable<decimal> ValorSeguro { get; set; }
        public Nullable<decimal> TaxaProcessamentoValor { get; set; }
        public string TaxaProcessamentoCancelada { get; set; }
        public Nullable<int> Score { get; set; }
        public string RetornoAccertify { get; set; }
        public string AccertifyForceStatus { get; set; }
        public Nullable<int> VendaBilhereriaIDTroca { get; set; }
        public Nullable<int> VendaBilheteriaIDOrigem { get; set; }
        public string CodigoRastreio { get; set; }
        public string EntregaCancelada { get; set; }
        public string ConvenienciaCancelada { get; set; }
        public string SeguroCancelado { get; set; }
        public Nullable<System.DateTime> CalcDataVenda { get; set; }
    
        public virtual tCaixa tCaixa { get; set; }
        public virtual ICollection<tIngresso> tIngresso { get; set; }
        public virtual ICollection<Voucher> Voucher { get; set; }
        public virtual ICollection<tVendaBilheteriaItem> tVendaBilheteriaItem { get; set; }
        public virtual tCliente tCliente { get; set; }
        public virtual ICollection<tVendaBilheteriaFormaPagamento> tVendaBilheteriaFormaPagamento { get; set; }
        public virtual ICollection<EstornoDadosCartaoCredito> EstornoDadosCartaoCredito { get; set; }
        public virtual ICollection<EstornoDadosCartaoCredito> EstornoDadosCartaoCredito1 { get; set; }
        public virtual ICollection<EstornoDadosDepositoBancario> EstornoDadosDepositoBancario { get; set; }
        public virtual ICollection<EstornoDadosDepositoBancario> EstornoDadosDepositoBancario1 { get; set; }
        public virtual ICollection<EstornoDadosDinheiro> EstornoDadosDinheiro { get; set; }
        public virtual ICollection<EstornoDadosDinheiro> EstornoDadosDinheiro1 { get; set; }
        public virtual ICollection<tVendaBilheteriaEntrega> tVendaBilheteriaEntrega { get; set; }
    }
}
