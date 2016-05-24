using System;

namespace IRLib.Assinaturas.Models.Relatorios
{
    public class RelatorioBancoIngresso
    {
        public int IngressoID { get; set; }
        public string Assinatura { get; set; }
        public DateTime Apresentacao { get; set; }
        public string Setor { get; set; }
        public string Lugar { get; set; }
        public string LoginDoador { get; set; }
        public string Doador { get; set; }
        public DateTime DataDoacao { get; set; }
        public string LoginResgate { get; set; }
        public string Resgate { get; set; }
        public DateTime DataResgate { get; set; }

    }
}
