using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaApresentacaoRaioX
    {
        private int indice;
        public int Indice
        {
            get { return indice; }
            set { indice = value; }
        }

        private int empresaid;
        public int EmpresaID
        {
            get { return empresaid; }
            set { empresaid = value; }
        }

        private string empresanome;
        public string EmpresaNome
        {
            get { return empresanome; }
            set { empresanome = value; }
        }

        private int localid;
        public int LocalID
        {
            get { return localid; }
            set { localid = value; }
        }

        private string localnome;
        public string LocalNome
        {
            get { return localnome; }
            set { localnome = value; }
        }

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

        private string eventocodigo;
        public string EventoCodigo
        {
            get { return eventocodigo; }
            set { eventocodigo = value; }
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

        private string setorcodigo;
        public string SetorCodigo
        {
            get { return setorcodigo; }
            set { setorcodigo = value; }
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

        private string apresentacaocodigo;
        public string ApresentacaoCodigo
        {
            get { return apresentacaocodigo; }
            set { apresentacaocodigo = value; }
        }

        private string precocodigo;
        public string PrecoCodigo
        {
            get { return precocodigo; }
            set { precocodigo = value; }
        }
    }

}
