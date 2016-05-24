using System;

namespace IRLib.ClientObjects
{
	[Serializable]
    public class EstruturaEventoInfo
    {
        public int EventoID;
        public string Atencao;
        public string PdvsSemTaxa;
        public string Censura;
        public string DuracaoEvento;
        public string RetiradaIngressos;
        public string MeiaEntrada;
        public string Promocoes;
        public string AberturaPortoes;
        public string Release;
        public string DescricaoPadrao;
        public int TaxaConveniencia;
        public Decimal TaxaMinima;
        public Decimal TaxaMaxima;
        public Decimal TaxaMaximaPorEmpresa;
        public bool Carregou;
    }
}
