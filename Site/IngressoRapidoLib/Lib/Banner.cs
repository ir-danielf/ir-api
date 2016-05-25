


namespace IngressoRapido.Lib
{
    public class Banner
    {
        public Banner() {}

        public Banner(int id) 
        {
            this.id = id;
        }

        private int id = 0;
        public int ID
        {
            get { return id; }
        }

        private string nome = string.Empty;
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private string img = string.Empty;
        public string Img
        {
            get { return img; }
            set { img = value; }
        }

        private string alt = string.Empty;
        public string Alt
        {
            get { return alt; }
            set { alt = value; }
        }

        private string url = string.Empty;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private int target = 0;
        public int Target
        {
            get { return target; }
            set { target = value; }
        }

        private int localizacao = 0;
        public int Localizacao
        {
            get { return localizacao; }
            set { localizacao = value; }
        }

        private int posicao = 0;
        public int Posicao
        {
            get { return posicao; }
            set { posicao = value; }
        }

        private string descricao = string.Empty;
        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

    }
}
