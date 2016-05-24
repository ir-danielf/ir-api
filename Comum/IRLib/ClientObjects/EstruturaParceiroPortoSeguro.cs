using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaParceiroPortoSeguro
    {
        public int EventoID { get; set; }
        public string Categoria { get; set; }
        public string NomeEvento { get; set; }
        public string Local { get; set; }
        public string Cidade { get; set; }
        public string Imagem { get; set; }

        public string Desconto { get; set; }
    }
}
