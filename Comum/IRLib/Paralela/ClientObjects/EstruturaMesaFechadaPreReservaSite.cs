using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaMesaFechadaPreReservaSite
    {
        private int ingressoID;

        public int IngressoID
        {
            get { return ingressoID; }
            set { ingressoID = value; }
        }
        private string codigo;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

    }
}
