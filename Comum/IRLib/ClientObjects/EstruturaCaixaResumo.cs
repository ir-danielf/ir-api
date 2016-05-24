using System;
using System.Collections.Generic;
using System.Data;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCaixaResumo
    {
        private List<EstruturaCaixaResumoPrincipal> principal;
        public List<EstruturaCaixaResumoPrincipal> Principal
        {
            get { return principal; }
            set { principal = value; }
        }

        private List<EstruturaCaixaResumoTotais> totais;
        public List<EstruturaCaixaResumoTotais> Totais
        {
            get { return totais; }
            set { totais = value; }
        }

        private DataTable relatorioprincipal;
        public DataTable RelatorioPrincipal
        {
            get { return relatorioprincipal; }
            set { relatorioprincipal = value; }
        }

        private DataTable relatoriototais;
        public DataTable RelatorioTotais
        {
            get { return relatoriototais; }
            set { relatoriototais = value; }
        }

    }

    [Serializable]
    public class EstruturaCaixaResumoPrincipal
    {
        private string acao;
        public string Acao
        {
            get { return acao; }
            set { acao = value; }
        }

        private string formapagamento;
        public string FormaPagamento
        {
            get { return formapagamento; }
            set { formapagamento = value; }
        }

        private int formapagamentoid;
        public int FormaPagamentoID
        {
            get { return formapagamentoid; }
            set { formapagamentoid = value; }
        }

        private int quantidade;
        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }

        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        private decimal valorconveniencia;
        public decimal ValorConveniencia
        {
            get { return valorconveniencia; }
            set { valorconveniencia = value; }
        }

        private decimal valorentrega;
        public decimal ValorEntrega
        {
            get { return valorentrega; }
            set { valorentrega = value; }
        }

        private decimal valortotal;
        public decimal ValorTotal
        {
            get { return valortotal; }
            set { valortotal = value; }
        }
        public decimal ValorTaxaProcessamento { get; set; }
    }

    [Serializable]
    public class EstruturaCaixaResumoTotais
    {
        private string acao;
        public string Acao
        {
            get { return acao; }
            set { acao = value; }
        }

        private int quantidadeingressos;
        public int QuantidadeIngressos
        {
            get { return quantidadeingressos; }
            set { quantidadeingressos = value; }
        }

        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        private decimal valorconveniencia;
        public decimal ValorConveniencia
        {
            get { return valorconveniencia; }
            set { valorconveniencia = value; }
        }

        private decimal valorentrega;
        public decimal ValorEntrega
        {
            get { return valorentrega; }
            set { valorentrega = value; }
        }

        private decimal valortotal;
        public decimal ValorTotal
        {
            get { return valortotal; }
            set { valortotal = value; }
        }

        public decimal ValorTaxaProcessamento { get; set; }
    }
}
