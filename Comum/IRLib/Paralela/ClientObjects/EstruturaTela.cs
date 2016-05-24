using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaTela : ICloneable
    {

        public int ID { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public string Ativa { get; set; }



        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
