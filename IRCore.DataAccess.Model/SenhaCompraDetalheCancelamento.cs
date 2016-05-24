using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class SenhaCompraDetalheCancelamento
    {
        public int CompraID { get; set; }
        public string SenhaCompra { get; set; }
        public DateTime DataCompra { get; set; }
        public int CaixaID { get; set; }
        public bool CancelamentoIndisponivel { get; set; }
        public string CancelamentoIndisponivelMotivo { get; set; }
        public decimal ValorSeguro { get; set; }
        public bool TaxaConvencienciaCancelada { get; set; }
        public bool TaxaEntregaCancelada { get; set; }
        public bool ValorSeguroCancelado { get; set; }
        public string SenhaTaxaConvencienciaCancelada { get; set; }
        public string SenhaTaxaEntregaCancelada { get; set; }
        public string SenhaValorSeguroCancelado { get; set; }
        public bool PodeCancelarEntrega { get; set; }
        public bool PodeCancelarSeguro { get; set; }
        public List<IngressoDetalheCancelamento> Ingressos { get; set; }
        public List<PagamentoDetalheCancelamento> Pagamentos { get; set; }
        public CompraTotal TotalCompra { get; set; }
        public string FormaDevolucaoDisponivel { get; set; }
        public List<ListaBancos> BancosIR { get; set; }
    }

    public class PagamentoDetalheCancelamento
    {
        public int ID { get; set; }
        public string FormaPagamentoNome { get; set; }
        public string FormaPagamentoTipo { get; set; }
        public int Parcelas { get; set; }
        public decimal Valor { get; set; }
        public string NomeTitular { get; set; }
        public string CPFTitular { get; set; }
        public string NumeroCartao { get; set; }
        public string Bandeira { get; set; }
        public string TransactionID { get; set; }
    }

    public class IngressoDetalheCancelamento
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
        public string PacoteGrupo { get; set; }
        public int PacoteID { get; set; }
        public string PacoteNome { get; set; }
        public bool PacotePermitirCancelamentoAvulso { get; set; }
        public int SetorID { get; set; }
        public string SetorNome { get; set; }
        public int ApresentacaoID { get; set; }
        public DateTime ApresentacaoHorario { get; set; }
        public bool Cancelado { get; set; }
        public string StatusCancelamentoCodigo { get; set; }
        public string StatusCancelamento
        {
            get
            {
                if (StatusCancelamentoCodigo == null)
                    return null;
                switch (StatusCancelamentoCodigo)
                {
                    case "P":
                        return "Pendente de Devolução";
                    case "D":
                        return "Devolvido";
                    case "A":
                        return "Devolução Automatica";
                    case "I":
                        return "Impresso";
                    case "V":
                        return "Vendido";
                    case "C":
                        return "Cancelado";
                    case "S":
                        return "Solicitação Cancelada";
                    case "B":
                        return "Bloquiado";
                    case "R":
                        return "Retirado";
                    default:
                        return "";
                }
            }
        }
        public bool PodeCancelarTaxaConveniencia { get; set; }
        public string SenhaCancelamento { get; set; }
        public bool CancelamentoIndisponivel { get; set; }
        public string CancelamentoIndisponivelMotivo { get; set; }
        public bool TemDevolucao { get; set; }
    }
}
