using System;
using System.Collections.Generic;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaTransacoesDetalhes
    {
        public EstruturaTransacoesDetalhes()
        {
            Cliente = new EstruturaTransacoesCliente();
            DadosCompra = new List<EstruturaFormaPagamento>();
            ListaIngressos = new List<EstruturaTransacoesIngressosComprados>();
        }
        public int VendaBilheteriaID { get; set; }
        public EstruturaTransacoesCliente Cliente { get; set; }
        public List<EstruturaFormaPagamento> DadosCompra { get; set; }
        public List<EstruturaTransacoesIngressosComprados> ListaIngressos { get; set; }
        public decimal ValorTotal { get; set; }
        public string Senha { get; set; }
        public VendaBilheteria.StatusAntiFraude Status { get; set; }
        public string NumeroAutorizacao { get; set; }
        public string Atendente { get; set; }
        public string NumeroCartao { get; set; }
        public int TaxaEntregaID { get; set; }
        public int EntregaControleID { get; set; }
        public int CanalID { get; set; }
        public string Canal { get; set; }
        public DateTime DataVenda { get; set; }
        public string TaxaEntrega { get; set; }
        public string ProcedimentoEntrega { get; set; }
        public decimal TaxaEntregaValor { get; set; }
        public int NivelRisco { get; set; }
        public string NSU { get; set; }
        public int MotivoID { get; set; }
        public string MotivoRisco { get; set; }

        public string FormasPagamento()
        {
            string ret = string.Empty;
            foreach (var fp in this.DadosCompra)
            {
                if (ret.Length > 0)
                    ret += ", ";
                ret += fp.FormaPagamento;
            }
            return ret;
        }

        public string TipoTaxa { get; set; }

        public bool PermitirImpressaoInternet { get; set; }

        public decimal TaxaProcessamento { get; set; }
    }
}
