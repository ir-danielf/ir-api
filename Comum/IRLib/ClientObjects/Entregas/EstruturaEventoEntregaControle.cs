using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEventoEntregaControle:ICloneable
    {
        public int ID{ get; set; }
        public int EventoID { get; set; }
        public int EntregaControleID { get; set; }
        public string ProcedimentoEntrega{ get; set; }
        public int DiasTriagem { get; set; }
        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
