using System;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class EstruturaTransacoesCliente
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string IP { get; set; }
        public string Endereco { get; set; }
    }
}
