using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaObrigatoriedade
    {
        public int ID { get; set; }
        public bool Nome { get; set; }
        public bool RG { get; set; }
        public bool CPF { get; set; }
        public bool Telefone { get; set; }
        public bool TelefoneComercial { get; set; }
        public bool Celular { get; set; }
        public bool DataNascimento { get; set; }
        public bool Email { get; set; }
        public bool CEPCliente { get; set; }
        public bool EnderecoCliente { get; set; }
        public bool NumeroCliente { get; set; }
        public bool ComplementoCliente { get; set; }
        public bool BairroCliente { get; set; }
        public bool CidadeCliente { get; set; }
        public bool EstadoCliente { get; set; }
        public bool NomeEntrega { get; set; }
        public bool CPFEntrega { get; set; }
        public bool RGEntrega { get; set; }
        public bool CEPEntrega { get; set; }
        public bool EnderecoEntrega { get; set; }
        public bool NumeroEntrega { get; set; }
        public bool ComplementoEntrega { get; set; }
        public bool BairroEntrega { get; set; }
        public bool CidadeEntrega { get; set; }
        public bool EstadoEntrega { get; set; }
        public bool NomeResponsavel { get; set; }
        public bool CPFResponsavel { get; set; }
        public bool Mudou { get; set; }
    }
    [Serializable]
    public class EstruturaObrigatoriedadeSite
    {
        public bool Nome { get; set; }
        public bool RG { get; set; }
        public bool CPF { get; set; }
        public bool Telefone { get; set; }
        public bool DataNascimento { get; set; }
        public bool Email { get; set; }
        public bool NomeResponsavel { get; set; }
        public bool CPFResponsavel { get; set; }
    }
}
