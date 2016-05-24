using System;
using System.Drawing;

namespace IRLib.Paralela.ClientObjects.Serie
{
    [Serializable]
    public class EstruturaCanalSerie
    {
        public int CanalSerieID { get; set; }
        public Enumerators.TipoAcaoCanal Acao { get; set; }
        public Bitmap ImagemAcao { get; set; }
        public string Regional { get; set; }
        public string Empresa { get; set; }
        public int CanalID { get; set; }
        public string Canal { get; set; }
    }

    [Serializable]
    public class EstruturaCanalSerieDistribuicao
    {
         public int ID { get; set; }
        public int CanalID { get; set; }
        public bool Distribuido { get; set; }
       
    }

}
