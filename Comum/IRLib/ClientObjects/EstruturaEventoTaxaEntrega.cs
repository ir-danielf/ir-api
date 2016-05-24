using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEventoTaxaEntrega
    {
        public int EventoID { get; set; }
        public int EventoTaxaEntregaID { get; set; }
        public string Regiao { get; set; }
        public int TaxaEntregaID { get; set; }
        public int RegiaoID { get; set; }
        public string TaxaEntrega { get; set; }
        public decimal Valor { get; set; }
        public bool Disponivel { get; set; }
        public bool DisponivelOld { get; set; }
        public bool Padrao { get; set; }
        public string PadraoT { get { return (this.Padrao ? "Sim" : "Não"); } }
        public string Procedimento { get; set; }
        public string Detalhes { get; set; }
    }
}
