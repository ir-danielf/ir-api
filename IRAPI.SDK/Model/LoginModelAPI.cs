using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRAPI.SDK.Model
{
    public class RetornoModelAPI<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string Tipo { get; set; }
        public T Retorno { get; set; }
    }

    public class EnderecoList
    {
        public bool EntregaDisponivel { get; set; }
        public object EntregaControles { get; set; }
        public object EntregaArea { get; set; }
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
        public int? EnderecoTipoID { get; set; }
        public string EnderecoPrincipal { get; set; }
        public int? StatusConsulta { get; set; }
    }

    public class Cliente
    {
        public List<EnderecoList> EnderecoList { get; set; }
        public string DataNascimentoAsDateTime { get; set; }
        public bool RecebeEmailAsBool { get; set; }
        public bool AtivoAsBool { get; set; }
        public int StatusAtualAsEnum { get; set; }
        public int ID { get; set; }
        public string Nome { get; set; }
        public string RG { get; set; }
        public string CPF { get; set; }
        public string CarteiraEstudante { get; set; }
        public string Sexo { get; set; }
        public string DDDTelefone { get; set; }
        public string Telefone { get; set; }
        public string DDDTelefoneComercial { get; set; }
        public string TelefoneComercial { get; set; }
        public string DDDCelular { get; set; }
        public string Celular { get; set; }
        public string DataNascimento { get; set; }
        public string Email { get; set; }
        public string RecebeEmail { get; set; }
        public object CEP { get; set; }
        public object Endereco { get; set; }
        public object Numero { get; set; }
        public object Cidade { get; set; }
        public object Estado { get; set; }
        public int ClienteIndicacaoID { get; set; }
        public string Obs { get; set; }
        public object Complemento { get; set; }
        public object Bairro { get; set; }
        public string Senha { get; set; }
        public string Ativo { get; set; }
        public string StatusAtual { get; set; }
        public string LoginOSESP { get; set; }
        public string CEPEntrega { get; set; }
        public string EnderecoEntrega { get; set; }
        public string NumeroEntrega { get; set; }
        public string CidadeEntrega { get; set; }
        public string EstadoEntrega { get; set; }
        public string ComplementoEntrega { get; set; }
        public string BairroEntrega { get; set; }
        public string CEPCliente { get; set; }
        public string EnderecoCliente { get; set; }
        public string NumeroCliente { get; set; }
        public string CidadeCliente { get; set; }
        public string EstadoCliente { get; set; }
        public string ComplementoCliente { get; set; }
        public string BairroCliente { get; set; }
        public string NomeEntrega { get; set; }
        public string CPFEntrega { get; set; }
        public string RGEntrega { get; set; }
        public object CPFConsultado { get; set; }
        public object NomeConsultado { get; set; }
        public int StatusConsulta { get; set; }
        public object CPFConsultadoEntrega { get; set; }
        public object NomeConsultadoEntrega { get; set; }
        public object StatusConsultaEntrega { get; set; }
        public string Pais { get; set; }
        public string CPFResponsavel { get; set; }
        public object Updated { get; set; }
        public int ContatoTipoID { get; set; }
        public object NivelCliente { get; set; }
        public string CNPJ { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string InscricaoEstadual { get; set; }
        public object TipoCadastro { get; set; }
        public string Profissao { get; set; }
        public int SituacaoProfissionalID { get; set; }
        public string DDDTelefoneComercial2 { get; set; }
        public string TelefoneComercial2 { get; set; }
        public object DataCadastro { get; set; }
        public List<object> tVendaBilheteria { get; set; }
        public List<object> Voucher { get; set; }
        public List<object> ClienteCredito { get; set; }
    }

    public class Login
    {
        public int ID { get; set; }
        public int ClienteID { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public string Email { get; set; }
        public string DataCadastro { get; set; }
        public string UltimoAcesso { get; set; }
        public string Ativo { get; set; }
        public string StatusAtual { get; set; }
        public string FaceBookUserID { get; set; }
        public string FaceBookUserToken { get; set; }
        public string FaceBookUserInfos { get; set; }
        public Cliente Cliente { get; set; }
        public string UltimoAcessoAsDateTime { get; set; }
        public string DataCadastroAsDateTime { get; set; }
        public bool AtivoAsBool { get; set; }
        public string StatusAtualAsEnum { get; set; }
    }

}
