using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaInfoEventoMondial
    {
        private int apresentacaoid;
        public int ApresentacaoID
        {
            get { return apresentacaoid; }
            set { apresentacaoid = value; }
        }

        private DateTime horario;
        public DateTime Horario
        {
            get { return horario; }
            set { horario = value; }
        }

        private string evento;
        public string Evento
        {
            get { return evento; }
            set { evento = value; }
        }

        private string local;
        public string Local
        {
            get { return local; }
            set { local = value; }
        }

        private int setorid;
        public int SetorID
        {
            get { return setorid; }
            set { setorid = value; }
        }

        private string setor;
        public string Setor
        {
            get { return setor; }
            set { setor = value; }
        }

        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }
    }
}
