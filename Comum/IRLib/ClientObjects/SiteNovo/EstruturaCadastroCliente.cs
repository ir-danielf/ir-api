using System;
using System.Runtime.Serialization;

namespace IRLib
{
    [DataContract]
    public class EstruturaCadastroCliente
    {
        [DataMember]
        public int ClienteID { get; set; }
        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string CPF { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Senha { get; set; }
        [DataMember]
        public string ConfirmacaoSenha { get; set; }
        [DataMember]
        public string DataNascimento { get; set; }
        [DataMember]
        public char Sexo { get; set; }
        [DataMember]
        public string CodPaisTelefoneResidencial { get; set; }
        [DataMember]
        public string DDDTelefoneResidencial { get; set; }
        [DataMember]
        public string TelefoneResidencial { get; set; }
        [DataMember]
        public string CodPaisTelefoneComercial { get; set; }
        [DataMember]
        public string DDDTelefoneComercial { get; set; }
        [DataMember]
        public string TelefoneComercial { get; set; }
        [DataMember]
        public string CodPaisTelefoneCelular { get; set; }
        [DataMember]
        public string DDDTelefoneCelular { get; set; }
        [DataMember]
        public string TelefoneCelular { get; set; }
        [DataMember]
        public string Logradouro { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string Complemento { get; set; }
        [DataMember]
        public string Bairro { get; set; }
        [DataMember]
        public string Cidade { get; set; }
        [DataMember]
        public string UF { get; set; }
        [DataMember]
        public string CEP { get; set; }
        [DataMember]
        public bool ReceberEmail { get; set; }
        [DataMember]
        public string MelhorFormaContato { get; set; }
    }
}
