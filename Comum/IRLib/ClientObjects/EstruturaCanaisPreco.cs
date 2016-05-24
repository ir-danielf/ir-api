using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaCanaisPreco
    {
        public enum EnumAcaoCanaisPreco { Manter = 0, Distribuir = 1, Remover = 2 };

        public enum EnumTipoFiltragemCanaisPreco { PontoVenda = 0, CallCenter = 1, Internet = 2 };

        public int ID { get; set; }
        public int CanalID { get; set; }
        public int PrecoID { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public int Quantidade { get; set; }
    }
}
