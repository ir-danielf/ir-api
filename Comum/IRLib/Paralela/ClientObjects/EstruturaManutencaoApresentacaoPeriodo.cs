using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaManutencaoApresentacaoPeriodo
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string local;
        public string Local
        {
            get { return local; }
            set { local = value; }
        }

        private string evento;
        public string Evento
        {
            get { return evento; }
            set { evento = value; }
        }

        private DateTime apresentacao;
        public DateTime Apresentacao
        {
            get { return apresentacao; }
            set { apresentacao = value; }
        }

        private bool disponivelVenda;
        public bool DisponivelVenda
        {
            get { return disponivelVenda; }
            set { disponivelVenda = value; }
        }

        private bool disponivelAjuste;
        public bool DisponivelAjuste
        {
            get { return disponivelAjuste; }
            set { disponivelAjuste = value; }
        }

        private bool disponivelRelatorio;
        public bool DisponivelRelatorio
        {
            get { return disponivelRelatorio; }
            set { disponivelRelatorio = value; }
        }

    }
}
