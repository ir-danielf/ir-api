using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCanaisEvento
    {
        public enum EnumAcaoCanaisEvento { Manter = 0, Distribuir = 1, Remover = 2 };

        public enum EnumTipoFiltragemCanaisEvento { PontoVenda = 0, CallCenter = 1, Internet = 2 };

        public int ID { get; set; }
        public int CanalID { get; set; }
        public int EventoID { get; set; }
        public decimal TaxaConveniencia { get; set; }
        public decimal TaxaMinima { get; set; }
        public decimal TaxaMaxima { get; set; }
        public decimal TaxaComissao { get; set; }
        public decimal ComissaoMinima { get; set; }
        public decimal ComissaoMaxima { get; set; }
        public string CanalTipo { get; set; }
    }
}
