using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaVoceSabia
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string identificacao;
        public string Identificacao
        {
            get { return identificacao; }
            set { identificacao = value; }
        }

        private string texto;
        public string Texto
        {
            get { return texto; }
            set { texto = value; }
        }
    }
}
