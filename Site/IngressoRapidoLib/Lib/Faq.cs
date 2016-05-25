using System.Collections.Generic;

namespace IngressoRapido.Lib
{
    public class Faq
    {
        public int ID { get; set; }
        public string Pergunta { get; set; }
    }

    public class FaqTipo
    {
        public string Nome { get; set; }
        public List<Faq> Perguntas { get; set; }
    }

}
