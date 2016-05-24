using System.Collections.Generic;

namespace IRLib.Paralela.Assinaturas.Models.Relatorios
{
    public class Recebimento
    {
        public RecebimentoMes RecebimentoMes { get; set; }
        public RecebimentoPeriodo RecebimentoPeriodo { get; set; }
    }

    public class RecebimentoMes
    {
        public RecebimentoMes() { this.ListaRecebimentoMesSerieFormaPagamento = new List<RecebimentoSerieFormaPagamento>(); }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string MesExibicao { get; set; }
        public List<RecebimentoSerieFormaPagamento> ListaRecebimentoMesSerieFormaPagamento { get; set; }
    }

    public class RecebimentoPeriodo
    {
        public RecebimentoPeriodo() { this.ListaRecebimentoMesSerieFormaPagamento = new List<RecebimentoSerieFormaPagamento>(); }

        public IRLib.Paralela.AssinaturaTipo.EnumPeriodos Periodo { get; set; }
        public string PeriodoExibicao
        {
            get
            {
                switch (Periodo)
                {
                    case IRLib.Paralela.AssinaturaTipo.EnumPeriodos.Renovacao:
                        return "Renovação";
                    case IRLib.Paralela.AssinaturaTipo.EnumPeriodos.Troca:
                        return "Troca";
                    case IRLib.Paralela.AssinaturaTipo.EnumPeriodos.Aquisicao:
                        return "Novas aquisições";
                    default:
                    case IRLib.Paralela.AssinaturaTipo.EnumPeriodos.ForaDePeriodo:
                        return "Fora de Período";
                }
            }
        }
        public List<RecebimentoSerieFormaPagamento> ListaRecebimentoMesSerieFormaPagamento { get; set; }
    }

    public class RecebimentoSerieFormaPagamento
    {
        public int AssinaturaID { get; set; }
        public string Assinatura { get; set; }
        public int QuantidadeFaturasAbertas { get; set; }
        public int QuantidadeBoletosPagos { get; set; }
        public int QuantidadeBoletosAbertos { get; set; }
        public int QuantidadeCartoes { get; set; }
        public int QuantidadeDinheiros { get; set; }
        public int QuantidadeCheques { get; set; }
        public decimal FaturasAbertas { get; set; }
        public decimal BoletosPagos { get; set; }
        public decimal BoletosAbertos { get; set; }
        public decimal Cartoes { get; set; }
        public decimal Dinheiros { get; set; }
        public decimal Cheques { get; set; }
    }
}
