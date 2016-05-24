using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaListaSetoreEmail
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string setor;
        public string Setor
        {
            get { return setor; }
            set { setor = value; }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string responsavel;
        public string Responsavel
        {
            get { return responsavel; }
            set { responsavel = value; }
        }
    }
}
