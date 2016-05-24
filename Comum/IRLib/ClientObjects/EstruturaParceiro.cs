using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaParceiro
    {
        public int ID { get; set; }
        public string Parceiro { get; set; }
        public int Tipo { get; set; }
    }

    [Serializable]
    public class EstruturaParceiroNovo : ICloneable
    {
        public int ID { get; set; }
        public string Parceiro { get; set; }
        public Enumerators.TipoParceiro Tipo { get; set; }
        public string TipoT
        {
            get
            {
                switch (Tipo)
                {
                    case Enumerators.TipoParceiro.Bin:
                        return  "Bin";
                    case Enumerators.TipoParceiro.Codigo:
                        return "Código";
                    case Enumerators.TipoParceiro.CodigoExterno:
                        return "Código Externo";
                    default:
                        return "Não Definido";
                }
            }
        }
        public int QuantidadeDistribuida { get; set; }
        public string Url { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
