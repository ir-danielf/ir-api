using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCanalValeIngresso
    {
        public int CanalValeIngressoID { get; set; }
        public int CanalID { get; set; }
        public string CanalNome { get; set; }
        public int ValeIngressoTipoID { get; set; }
        public bool Distribuido { get; set; }//utilizado para carregar os canais na tela
        public CanalValeIngresso.EnumAcaoCanal acao { get; set; }
    }
}
