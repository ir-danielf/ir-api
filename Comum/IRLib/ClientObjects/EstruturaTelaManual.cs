using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaTelaManual : ICloneable
    {

        public int ID { get; set; }
        public string IDTela { get; set; }
        public string Manual { get; set; }
        public string Arquivo { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public bool Ativa { get; set; }
        public string Form { get; set; }



        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
