using System;
using System.Collections.Generic;

namespace IRLib
{
    [Serializable]
    public class MapaEsquematicoInfo : ICloneable
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public int ApresentacaoID { get; set; }
        public List<SetorInfo> Setores { get; set; }
        public bool UsarMapaEsquematico { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public DateTime UltimaAtualizacaoLocal { get; set; }
        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
