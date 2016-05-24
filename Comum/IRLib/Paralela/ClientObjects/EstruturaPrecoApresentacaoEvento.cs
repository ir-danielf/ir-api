using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaPrecoApresentacaoEvento
    {
        public int EventoID { get; set; }
        public string Nome { get; set; }
        public string Apresentacao { get; set; }
        public string Setor { get; set; }
        public string Preco { get; set; }
        public string Valor { get; set; }
    }
}
