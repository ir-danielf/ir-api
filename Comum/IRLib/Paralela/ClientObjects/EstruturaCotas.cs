using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaCotas
    {
        public int EventoID { get; set; }
        public string EventoNome { get; set; }
        public int ApresentacaoID { get; set; }
        public string Apresentacao { get; set; }
    }
}
