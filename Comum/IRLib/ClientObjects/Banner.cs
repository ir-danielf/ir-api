using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class Banner
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

        private string img;
        public string Img
        {
            get { return img; }
            set { img = value; }
        }

        private string alt;
        public string Alt
        {
            get { return alt; }
            set { alt = value; }
        }
        
        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        
        private int target;
        public int Target
        {
            get { return target; }
            set { target = value; }
        }
        
        private int localizacao;
        public int Localizacao
        {
            get { return localizacao; }
            set { localizacao = value; }
        }

        private string descricao;
        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

        private string localizacaoToString;

        public string LocalizacaoToString
        {
            get { return localizacaoToString; }
            set 
            {
                if (value == "1")
                    localizacaoToString = "Topo";
                else if (value == "2")
                    localizacaoToString = "Destaque";
                else if (value == "3")
                    localizacaoToString = "Rodapé";
                else if (value == "4")
                    localizacaoToString = "TopoPrefeitura";
                else if (value == "5")
                    localizacaoToString = "Cadastro";
                else if (value == "6")
                    localizacaoToString = "DestaqueMobile";
                else
                    localizacaoToString = value;
            }
        }
        
        private int posicao;
        public int Posicao
        {
            get { return posicao; }
            set { posicao = value; }
        }
       
    }
}
