using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaHistoricoDeAcaoDaVenda
    {
        public string VendaBilheteriaID { get; set; }
        public int senhaPesquisada { get; set; }
        public string acao { get; set; }
        public DateTime dataMudanca { get; set; }
        public string motivo { get; set; }
        public string observacao { get; set; }
        public string usuario { get; set; }
    }
}
