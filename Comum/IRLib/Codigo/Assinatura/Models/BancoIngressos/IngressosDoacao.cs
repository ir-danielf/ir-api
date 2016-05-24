using System.Collections.Generic;

namespace IRLib.Assinaturas.Models
{

    public class DataDoacao
    {
        public int ApresentacaoID { get; set; }
        public string Horario { get; set; }
        public string Programacao { get; set; }
        public List<IngressoDoacao> Ingressos { get; set; }
    }

    public class IngressoDoacao
    {
        public int IngressoID { get; set; }
        public string Setor { get; set; }
        public string Codigo { get; set; }
    }
}
