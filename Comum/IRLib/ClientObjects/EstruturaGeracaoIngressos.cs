using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaGeracaoIngressos
    {
        private int eventoid;
        public int EventoID
        {
            get { return eventoid; }
            set { eventoid = value; }
        }

        private string eventonome;
        public string EventoNome
        {
            get { return eventonome; }
            set { eventonome = value; }
        }

        private int apresentacaoid;
        public int ApresentacaoID
        {
            get { return apresentacaoid; }
            set { apresentacaoid = value; }
        }

        private string apresentacaohorario;
        public string ApresentacaoHorario
        {
            get { return apresentacaohorario; }
            set { apresentacaohorario = value; }
        }

        private int setorid;
        public int SetorID
        {
            get { return setorid; }
            set { setorid = value; }
        }

        private string setornome;
        public string SetorNome
        {
            get { return setornome; }
            set { setornome = value; }
        }

        private int apresentacaosetorid;
        public int ApresentacaoSetorID
        {
            get { return apresentacaosetorid; }
            set { apresentacaosetorid = value; }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string statuserro;
        public string StatusErro
        {
            get { return statuserro; }
            set { statuserro = value; }
        }
    }
}