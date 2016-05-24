using IRCore.DataAccess.Model.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.DataAccess.Model
{
    public class SenhaCancelamentoDetalhe
    {
        public int CancelamentoID { get; set; }
        public int CompraID { get; set; }
        public string SenhaCompra { get; set; }
        public string SenhaCancelamento { get; set; }
        public DateTime DataCompra { get; set; }
        public DateTime DataCancelamento { get; set; }
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
                        return "Devolução automática, durante o cancelamento";
                    case "P":
                        return "Ingressos pendentes de devolução";
                    case "D":
                        return "Ingressos Devolvidos";
                    case "S":
                        return "Solicitação foi cancelada";
                    default:
                        return "";
                }
            }
        }
        public int CaixaID { get; set; }
        public string Obs { get; set; }
        public decimal ValorSeguroCompra { get; set; }
        public decimal ValorSeguroCancelamento { get; set; }
        public List<Ingresso> IngressosCancelados { get; set; }
        public List<Historico> Historico { get; set; }
        public CompraTotal TotalCompra { get; set; }
        public CompraTotal TotalCancelado { get; set; }
    }

    public class Historico
    {
        public DateTime DataMovimentacao { get; set; }
        public string Movimentacao { get; set; }
    }

    public class EstornoDepositoBancario
    {
        public int ID { get; set; }
        public int VendaBilheteriaIDVenda { get; set; }
        public DateTime DataDeposito { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public decimal Valor { get; set; }
        public string CPFCorrentista { get; set; }
        public string NomeCorrentista { get; set; }
        public DateTime DataInsert { get; set; }
        public string Email { get; set; }
        public string CancelamentoPor { get; set; }
        public string Status { get; set; }
    }

    public class EstornoCartaoCredito
    {
        public int ID { get; set; }
        public int VendaBilheteriaIDVenda { get; set; }
        public string Bandeira { get; set; }
        public string Cartao { get; set; }
        public decimal Valor { get; set; }
        public string Cliente { get; set; }
        public string CPFCliente { get; set; }
        public string CancelamentoPor { get; set; }
        public DateTime DataInsert { get; set; }
        public DateTime DataEnvio { get; set; }
        public string Email { get; set; }
        public char Status { get; set; }
        public string StatusCompleto
        {
            get
            {
                switch ((enumStatusEstorno) Status)
                {
                    case enumStatusEstorno.Automatico:
                        return "Estorno automático";
                    case enumStatusEstorno.Solicitado:
                        return "Estorno solicitado aguardando devolução de Ingressos";
                    case enumStatusEstorno.Pendente:
                        return "Estorno pendente";
                    case enumStatusEstorno.Cancelado:
                        return "Solicitação de estorno foi cancelada";
                    default:
                        return null;
                }
            }
        }
    }

    public class EstornoDinheiro
    {
        public int ID { get; set; }
        public int VendaBilheteriaIDVenda { get; set; }
        public decimal Valor { get; set; }
        public string Cliente { get; set; }
        public string CancelamentoPor { get; set; }
        public string Email { get; set; }
        public DateTime DataInsert { get; set; }
    }

    public class ProtocoloCancelamento
    {
        public int IngressoID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Acao { get; set; }
        public bool Impresso { get; set; }
    }
}
