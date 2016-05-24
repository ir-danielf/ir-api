using System;

namespace IngressoRapido.Lib
{
    public class Veiculo
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        private string nome;

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        private DateTime timestamp;

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public void IncluirVeiculo()
        {
            throw new System.NotImplementedException();
        }
    }
}