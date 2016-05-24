using System;

namespace IRLib.Paralela.ClientObjects
{
    public class EstruturaPagamento
    {
        public enum enumTipoPagamento
        {
            TEF,
            TEFDebito,
            Adyen,
            Paypal,
            VIR,
            Milhas,
            Nenhum
        }

        public string RedePreferencial { get; set; }
            
        public enumTipoPagamento TipoPagamento { get; set; }

        public string EntregaNome { get; set; }        

        public int EntregaControleID { get; set; }

        public decimal EntregaValor { get; set; }

        public int PdvID { get; set; }

        public int EnderecoClienteID { get; set; }

        public int ClienteID { get; set; }

        public string ClienteEmail { get; set; }

        public string SessionID { get; set; }

        public string ClienteNome { get; set; }

        public string DataSelecionada { get; set; }

        public int FormaPagamentoID { get; set; }

        public decimal ValorTotal { get; set; }

        public int Parcelas { get; set; }

        public string Bandeira { get; set; }

        public Sitef.enumBandeira BandeiraNome
        {
            get
            {
                switch (Convert.ToInt32(Bandeira))
                {
                    case (int)Sitef.enumBandeira.Visa:
                        return Sitef.enumBandeira.Visa;
                    case (int)Sitef.enumBandeira.Master:
                        return Sitef.enumBandeira.Master;
                    case (int)Sitef.enumBandeira.Diners:
                        return Sitef.enumBandeira.Diners;
                    case (int)Sitef.enumBandeira.Amex:
                        return Sitef.enumBandeira.Amex;
                    case (int)Sitef.enumBandeira.Aura:
                        return Sitef.enumBandeira.Aura;
                    case (int)Sitef.enumBandeira.Hipercard:
                        return Sitef.enumBandeira.Hipercard;
                    case (int)Sitef.enumBandeira.ValeCultura:
                        return Sitef.enumBandeira.ValeCultura;
                    case (int)Sitef.enumBandeira.Elo:
                        return Sitef.enumBandeira.Elo;
                    case (int)Sitef.enumBandeira.EloCultura:
                        return Sitef.enumBandeira.EloCultura;
                    default:
                        return Sitef.enumBandeira.PayPal;
                }
            }
        }

        public string NumeroCartao { get; set; }

        public string DataVencimento { get; set; }

        public string CodigoSeguranca { get; set; }

        public string NomeCartao { get; set; }

        public string CodigoTrocaFixo { get; set; }

        public string IP { get; set; }

        public int CartaoID { get; set; }

        public bool CartaoOutraPessoa { get; set; }

        public string Token { get; set; }

        public string PayerID { get; set; }

        public string TransactionID { get; set; }

        public string CorrelationID { get; set; }

        public bool IniciouTef { get; set; }

        public bool IniciouAdyen { get; set; }

        public decimal JurosValor { get; set; }

        public decimal Coeficiente { get; set; }

        public string NotaFiscalAdyen = "Pagamento efetuado através da Adyen, Não existe nota fiscal! Para estornar o valor total cobrado no Cartão de Crédito, marque a opção 'Cancelar Pagamento' na janela de cancelamento.";

        public string NotaFiscalPaypal = "Pagamento efetuado através do Paypal, Não existe nota fiscal! Para estornar o valor total cobrado no Cartão de Crédito, marque a opção 'Cancelar Pagamento' na janela de cancelamento.";

        private IRLib.Paralela.Sitef ositef { get; set; }

        public IRLib.Paralela.Sitef oSitef
        {
            get
            {
                if (ositef != null)
                    return ositef;

                ositef = new IRLib.Paralela.Sitef()
                {
                    terminal = this.ClienteID.ToString("00000000"),
                    Empresa = IRLib.Paralela.Sitef.enumEmpresa.IngressoRapido,
                    ValorCompra = (this.ValorTotal + this.JurosValor).RoundUp(2).ToString(),
                    Parcelas = this.Parcelas.ToString(),
                    ClienteID = this.ClienteID.ToString(),
                    Bandeira = (IRLib.Paralela.Sitef.enumBandeira)Enum.Parse(typeof(IRLib.Paralela.Sitef.enumBandeira), this.Bandeira),
                    NumeroCartao = this.NumeroCartao,
                    DataVencimento = this.DataVencimento,
                    CodigoSeguranca = this.CodigoSeguranca,
                    TipoFinanciamento = IRLib.Paralela.Sitef.enumTipoFinanciamento.Estabelecimento,
                    Rede = this.RedePreferencial
                };

                return ositef;
            }
        }

        private IRLib.Adyen oadyen { get; set; }

        public IRLib.Adyen oAdyen
        {
            get
            {
                if (oadyen != null)
                    return oadyen;

                oadyen = new IRLib.Adyen()
                {
                    Valor = this.ValorTotal + this.JurosValor,
                    CodigoVerificacaoCartao = this.CodigoSeguranca,
                    DataValidadeCartao = this.DataVencimento,
                    NomeCartao = this.NomeCartao,
                    NumeroCartao = this.NumeroCartao,
                    ClienteEmail = this.ClienteEmail,
                    ClienteIP = this.IP,
                    ClienteID = this.ClienteID.ToString(),
                    ClienteSessionID = this.SessionID,
                    ClienteNome = this.ClienteNome,
                    Parcelas = this.Parcelas,
                    Adquirente = this.RedePreferencial
                };
                return oadyen;
            }
        }

        public static EstruturaPagamento Montar(enumTipoPagamento tipoPagamento,
            EstruturaCompraTemporaria oCompraTemporaria, decimal valorTotalVir, string numeroCartao, string validadeCartao, string codigoSeguranca, string nomeCartao, int cartaoID, bool cartaoOutraPessoa, string PayerID, string Token, string IP, decimal Diferenca, decimal Coeficiente)
        {
            return new EstruturaPagamento()
            {
                TipoPagamento = tipoPagamento,

                ValorTotal = oCompraTemporaria.ValorTotal - valorTotalVir,
                Parcelas = oCompraTemporaria.Parcelas,
                ClienteID = oCompraTemporaria.ClienteID,
                Bandeira = oCompraTemporaria.Bandeira,
                NumeroCartao = numeroCartao,
                DataVencimento = validadeCartao,
                CodigoSeguranca = codigoSeguranca,
                NomeCartao = nomeCartao.ToUpper(),
                CartaoID = cartaoID,
                EntregaNome = oCompraTemporaria.TaxaEntrega,
                EntregaControleID = oCompraTemporaria.EntregaControleIDSelecionado,
                EntregaValor = Convert.ToDecimal(oCompraTemporaria.EntregaValor),
                PdvID = oCompraTemporaria.PDVSelecionado,
                EnderecoClienteID = oCompraTemporaria.EnderecoID,
                DataSelecionada = oCompraTemporaria.DataSelecionada,
                FormaPagamentoID = oCompraTemporaria.FormaPagamentoID,
                CodigoTrocaFixo = oCompraTemporaria.CodigoTrocaFixo,
                CartaoOutraPessoa = cartaoOutraPessoa,
                IP = IP,
                PayerID = PayerID,
                Token = Token,
                JurosValor = Diferenca,
                Coeficiente = Coeficiente,
                RedePreferencial = new IRLib.Paralela.FormaPagamento().RedePreferencialFormaPagamentoID(oCompraTemporaria.FormaPagamentoID, IRLib.Paralela.Sitef.Loja)
            };
        }
    }
}