using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEntregaAreaCep : ICloneable
    {
        public int EntregaAreaCepID { get; set; }
        public int EntregaAreaID { get; set; }
        public string CepInicio { get; set; }
        public string CepFim { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}