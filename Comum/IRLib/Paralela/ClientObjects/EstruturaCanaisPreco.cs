using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCanaisPreco
    {
        public enum EnumAcaoCanaisPreco { Manter = 0, Distribuir = 1, Remover = 2 };

        public int ID { get; set; }
        public int CanalID { get; set; }
        public int PrecoID { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public int Quantidade { get; set; }
    }
}
