using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaInconsistenciaIngressoLog
    {
        public int IngressoID { get; set; }
        public int IngressoLogID { get; set; }
        public char AcaoLog { get; set; }
        public char StatusIngresso { get; set; }
        public int IngressoPrecoID { get; set; }
        public int LogPrecoID { get; set; }
    }
}
