using System;

namespace IRLib.Paralela.Assinaturas.Models
{
    public class Comprovante
    {
        public int ID { get; set; }
        public string Acao { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Responsavel { get; set; }
        public int Quantidade { get; set; }
        public int ApresentacaoID { get; set; }
    }

    public class ComprovanteItem
    {
        public int ID { get; set; }
        public IRLib.Paralela.AssinaturaBancoIngressoComprovante.AcaoComprovante Acao { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ApresentacaoID { get; set; }
        public DateTime Horario { get; set; }
        public string Assinatura { get; set; }
        public string Setor { get; set; }
        public string Codigo { get; set; }
        public bool Invalido { get; set; }
    }
}
