using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class SenhaCompraDetalhe
    {
        public int ID { get; set; }
        public string Senha { get; set; }
        public DateTime DataCompra { get; set; }
        public string StatusCodigo { get; set; }
        public string Status
        {
            get
            {
                if (StatusCodigo == null)
                    return null;
                switch (StatusCodigo)
                {
                    case "A":
                        return "Aguardando Aprovação";
                    case "P":
                        return "Pago";
                    case "C":
                        return "Cancelado";
                    case "E":
                        return "Entregue";
                    case "R":
                        return "Re-impresso";
                    case "M":
                        return "Pré-reservado";
                    case "F":
                        return "Fraude";
                    case "N":
                        return "Em Análise";
                    default:
                        return "";
                }
            }
        }
        public string StatusCancelamentoCodigo { get; set; }
        public string StatusCancelamento
        {
            get
            {
                if (StatusCancelamentoCodigo == null)
                    return null;
                switch (StatusCancelamentoCodigo)
                {
                    case "N":
                        return "Não";
                    case "S":
                        return "Sim";
                    default:
                        return "";
                }
            }
        }
        public int EntregaID { get; set; }
        public string EntregaNome { get; set; }
        public int EntregaControleID { get; set; }
        public int CaixaID { get; set; }
        public string Obs { get; set; }
        public string NotaFiscalCliente { get; set; }
        public string NotaFiscalEstabelecimento { get; set; }
        public int ModalidadePagamentoCodigo { get; set; }
        public string ModalidadePagamentoTexto { get; set; }
        public int QuantidadeImpressoesInternet { get; set; }
        public int PdvID { get; set; }
        public bool PagamentoProcessado { get; set; }
        public decimal ValorSeguro { get; set; }
        public int Score { get; set; }
        public int ClienteEnderecoID { get; set; }
        public List<ValeIngresso> ValeIngresso { get; set; }
        public string VoucherCodigo { get; set; }
        public int VoucherID { get; set; }
        public AgregadoModel Agredado { get; set; }
        public List<string> SenhasCancelamento { get; set; }
        public List<Pagamento> Pagamentos { get; set; }
        public List<Ingresso> Ingressos { get; set; }
        public CompraTotal Total { get; set; }
    }

    public class ValeIngresso
    {
        public int ValeIngressoID { get; set; }
        public string ValeIngressoCodigoTroca { get; set; }
        public string ValeIngressoCodigoBarra { get; set; }
    }

    public class VoucherModel
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
    }

    public class CompraTotal
    {
        public decimal ValorTotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorTaxaEntrega { get; set; }
        public decimal ValorTaxaConveniencia { get; set; }
        public decimal ValorIngressos { get; set; }
    }

    public class Pagamento
    {
        public int ID { get; set; }
        public string FormaPagamentoNome { get; set; }
        public int Parcelas { get; set; }
        public decimal Valor { get; set; }
    }

    public class Ingresso
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
        public int LugarID { get; set; }
        public int PrecoID { get; set; }
        public int EventoID { get; set; }
        public string EventoNome { get; set; }
        public int LocalID { get; set; }
        public string LocalNome { get; set; }
        public string PrecoNome { get; set; }
        public decimal PrecoValor { get; set; }
        private DateTime? _timeStampReserva;
        public DateTime? TimeStampReserva
        {
            get
            {
                if (_timeStampReserva == DateTime.MinValue)
                {
                    return null;
                }
                else
                {
                    return _timeStampReserva;
                }
            }
            set
            {
                _timeStampReserva = value;
            }
        }
        public decimal TaxaConveniencia { get; set; }
        public string StatusCancelamentoCodigo { get; set; }
        public string StatusCancelamento
        {
            get
            {
                if (StatusCancelamentoCodigo == null)
                    return null;
                switch (StatusCancelamentoCodigo)
                {
                    case "N":
                        return "Não";
                    case "S":
                        return "Sim";
                    default:
                        return "";
                }
            }
        }
        public string SenhaCancelamento { get; set; }
        public string PacoteGrupo { get; set; }
        public int PacoteID { get; set; }
        public int SetorID { get; set; }
        public string SetorNome { get; set; }
        public int ApresentacaoID { get; set; }
        public DateTime ApresentacaoHorario { get; set; }
    }
}
