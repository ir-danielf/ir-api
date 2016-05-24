using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaPrecoPreReservaSite
    {
        private int iD;
        private string nome;
        private decimal valor;
        private int quantidadePorCliente;
 
        public int QuantidadePorCliente
        {
            get { return quantidadePorCliente; }
            set { quantidadePorCliente = value; }
        }

        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }
    }
}
