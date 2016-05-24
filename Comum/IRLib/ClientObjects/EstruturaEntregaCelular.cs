using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaEntregaCelular
    {
        public long OrderID { get; set; }
        public long IngressoID { get; set; }
        public long CPF { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string Local { get; set; }
        public int EventoID { get; set; }
        public string Evento { get; set; }
        public DateTime Data { get; set; }
        public string Setor { get; set; }
        public string PrecoNome { get; set; }
        public string Codigo { get; set; }
        public string Loja { get; set; }
        public string Pacote { get; set; }
        public string Pagamento { get; set; }
    }
}
