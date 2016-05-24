using System;

namespace IRLib.ClientObjects
{

	[Serializable]
    public class IngressoImpressaoCancelar
    {
        public int IngressoID;
        public int VendaBilheteriaID;
        public int PrecoID;
        public int EventoID;
        public string CodigoBarras;
        public bool BlackList;
        public string TimeStamp;
        public Enumerators.TipoCodigoBarra TipoCodigoBarra { get; set; }
        public int ApresentacaoSetorID { get; set; }
    }
}
