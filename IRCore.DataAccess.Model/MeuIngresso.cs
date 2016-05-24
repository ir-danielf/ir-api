using System;
using System.Collections.Generic;
using IRCore.DataAccess.Model.Enumerator;

namespace IRCore.DataAccess.Model
{
    public class MeuIngresso
    {
        public int ID { get; set; }
        public int VendaBilheteriaIDOrigem { get; set; }
        public DateTime DataVenda { get; set; }
        public string SenhaVenda { get; set; }
        public string StatusVenda { get; set; }
        public string TipoEntrega { get; set; }
        public MeuIngressoPagamento Pagamento { get; set; }
        public MeuIngressoEntrega Entrega { get; set; }
        public List<tIngresso> Ingressos { get; set; }
        public List<Carrinho> Carrinho { get; set; }
        public enumVendaBilheteriaStatus StatusVendaAsEnum
        {
            get { return (enumVendaBilheteriaStatus)StatusVenda[0]; }
            set { StatusVenda = ((char)value).ToString(); }
        }
        public string StatusDevolucaoPendenteCodigo { get; set; }
        public string StatusDevolucaoPendente
        {
            get
            {
                if (StatusDevolucaoPendenteCodigo == null)
                    return null;
                switch (StatusDevolucaoPendenteCodigo)
                {
                    case "A":
                        return "Devolução Automática / Sem Devolução";
                    case "P":
                        return "Pendente";
                    case "C":
                        return "Cancelada";
                    case "D":
                        return "Devolvido";
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
                return StatusCancelamentoCodigo == "S" ? "Sim" : "Não";
            }
        }
        public string PermissaoImprimir { get; set; }
        public string PermissaoCancelarData { get; set; }
        public string EventosID { get; set; }
        public string EventosNome { get; set; }
        public MeuIngresso Cancelada { get; set; }
    }

    public class MeuIngressoPagamento
    {
        //Pagamento
        public string ParcelasNome { get; set; }
        public string Parcelas { get; set; }
        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }

        //Valor
        public decimal ValorTotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorTaxaEntrega { get; set; }
        public decimal ValorConveniencia { get; set; }
        public decimal ValorIngressos { get; set; }
    }

    public class MeuIngressoEntrega
    {
        public int ID { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public int PrazoEntrega { get; set; }
        public string ProcedimentoEntrega { get; set; }
        public string PermitirImpressaoInternet { get; set; }
        public int DiasTriagem { get; set; }
        public string ETicketURL { get; set; }
        public string StatusSedex { get; set; }
        public string StatusMensageiro { get; set; }
        public PontoVenda LocalPDV { get; set; }
        public tClienteEndereco LocalClienteEndereco { get; set; }
        public Local LocalEvento { get; set; }

        public bool PermitirImpressaoInternetAsBool
        {
            get { return PermitirImpressaoInternet == "T"; }
            set { PermitirImpressaoInternet = value ? "T" : "F"; }
        }

        public enumEntregaTipo TipoAsEnum
        {
            get { return (enumEntregaTipo)Tipo[0]; }
            set { Tipo = ((char)value).ToString(); }
        }
    }

}