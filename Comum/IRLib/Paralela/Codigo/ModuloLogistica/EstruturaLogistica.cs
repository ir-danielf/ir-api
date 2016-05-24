using System;

namespace IRLib.Paralela.Codigo.ModuloLogistica
{
    [Serializable]
    public class EstruturaLogistica : ICloneable
    {
        public string TaxaEntrega { get; set; }
        public string Cliente { get; set; }
        public string Email { get; set; }
        public int VendaBilheteriaID { get; set; }
        public Enumeradores.TaxaEntregaTipo TaxaEntregaTipo { get; set; }
        public string Senha { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
