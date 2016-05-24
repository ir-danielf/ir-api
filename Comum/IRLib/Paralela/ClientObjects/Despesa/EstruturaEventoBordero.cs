using System;


namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaEventoBordero : ICloneable
    {
        public int ID { get; set; }
        public int EventoID { get; set; }
        public string GestorRazaoSocial { get; set; }
        public string GestorCpfCnpj{ get; set; }
        public string GestorEndereco{ get; set; }
        public string ProdutorRazaoSocial{ get; set; }
        public string ProdutorCpfCnpj{ get; set; }
        public string ProdutorEndereco{ get; set; }



        #region ICloneable Members

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        
    }


}
