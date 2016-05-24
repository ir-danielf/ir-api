
namespace IRLib.Assinaturas.Models
{
    public class TotaisPorProcessoAssinatura
    {
        public TotaisPorProcessoAssinatura() { }

        public int InternetRenovacaoNaRenovacao { get; set; }
        public int InternetRenovacaoNaTroca { get; set; }
        public int InternetRenovacaoNaAquisicao { get; set; }
        public int InternetTroca { get; set; }
        public int InternetAquisicao { get; set; }

        public int OperadorRenovacaoNaRenovacao { get; set; }
        public int OperadorRenovacaoNaTroca { get; set; }
        public int OperadorRenovacaoNaAquisicao { get; set; }
        public int OperadorTroca { get; set; }
        public int OperadorAquisicao { get; set; }
    }
}
