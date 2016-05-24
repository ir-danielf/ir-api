using System;

namespace IRLib.Paralela.ClientObjects
{
    [Serializable]
    public class EstruturaClienteEndereco
    {
        public int ID { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public int ClienteID { get; set; }
        public int EnderecoTipoID { get; set; }
        public string EnderecoTipo { get; set; }
        public bool EnderecoPrincipal { get; set; }
        public bool PodeAlterar { get; set; }
        public ClienteEndereco.enumStatusCPF StatusConsulta { get; set; }
        public bool PodeExcluir { get; set; }
    }



}
