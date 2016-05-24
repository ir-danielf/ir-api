
namespace IRLib.Paralela.Assinaturas.Models.Relatorios
{
    public class HistoricoPorPeriodo
    {
        public int Periodo { get; set; }
        public string PeriodoExibicao
        {
            get
            {
                switch (Periodo)
                {
                    case 1:
                        return "PERÍODO DE RENOVAÇÕES";
                    case 2:
                        return "PERÍODO DE TROCAS";
                    case 3:
                        return "NOVAS ASSINATURAS";
                    default:
                        return "FORA DE PERÍODO";
                }
            }
        }
        public HistoricoAcoes Historico { get; set; }
    }

    public class HistoricoAcoes
    {
        public int Renovados { get; set; }
        public int TrocasSinalizadas { get; set; }
        public int Desistencias { get; set; }
        public int Aquisicoes { get; set; }
        public int TrocasEfetuadas { get; set; }

        public int Total
        {
            get { return Renovados + TrocasSinalizadas + Desistencias + Aquisicoes + TrocasEfetuadas; }
        }

        public int TotalRenovadasTrocas
        {
            get { return Renovados + TrocasEfetuadas; }
        }
    }
}
