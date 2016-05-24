using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model;

namespace IRAPI.Models
{
    public class ClienteAuthRequestModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string respCaptcha { get; set; }
    }

    public class ClienteAuthFacebookReturnUrl
    {
        public string urlRet { get; set; }
    }

    public class ClienteAuthFacebookRequestModel
    {
        public string facebookAccessToken { get; set; }
        public string facebookAccessCode { get; set; }
        public string urlRet { get; set; }
    }

    public class ClienteAuthAccountKitRequestModel
    {
        public string accessToken { get; set; }
        public string guid { get; set; }
        public string code { get; set; }
    }

    /// <summary>
    /// Objeto de request do método de cadastro com facebook
    /// </summary>
    public class ClienteAuthFacebookCadasrtoRequestModel
    {
        /// <summary>
        /// Facebook access token
        /// </summary>
        public string facebookAccessToken { get; set; }
        
        /// <summary>
        /// Facebook access code
        /// </summary>
        public string facebookAccessCode { get; set; }
        
        /// <summary>
        /// Url de retorno
        /// </summary>
        public string urlRet { get; set; }
        
        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string nome { get; set; }
        
        /// <summary>
        /// Email do cliente
        /// </summary>
        public string email { get; set; }
        
        /// <summary>
        /// Cpf do cliente
        /// </summary>
        public string cpf { get; set; }
        
        /// <summary>
        /// Ddd do telefone do cliente
        /// </summary>
        public string dddTelefone { get; set; }
        
        /// <summary>
        /// Telefone do cliente
        /// </summary>
        public string telefone { get; set; }

        /// <summary>
        /// Converte um objeto do tipo ClienteAuthFacebookCadasrtoRequestModel em Login
        /// </summary>
        /// <param name="request">Objeto do tipo ClienteAuthFacebookCadasrtoRequestModel</param>
        /// <returns>Login</returns>
        public static explicit operator Login(ClienteAuthFacebookCadasrtoRequestModel request)
        {
            return new Login
            {
                Email = request.email,
                CPF = request.cpf.Replace(".", "").Replace("-", ""),
                Senha = string.Empty,
                Cliente = new tCliente
                {
                    Email = request.email,
                    CPF = request.cpf.Replace(".", "").Replace("-", ""),
                    Nome = request.nome,
                    DDDTelefone = request.dddTelefone,
                    Telefone = request.telefone,
                }
            };
        }
    }

    public class ClienteCadastroAccountKitRequestModel
    {
        public string accessToken { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }
        public string dddTelefone { get; set; }
        public string telefone { get; set; }
        public bool recebeEmail { get; set; }
    }

    public class ClienteSenhaModel
    {
        public string senha { get; set; }
    }

    public class ClienteAuthFacebookVinculoRequestModel
    {
        public Login loginTemp { get; set; }
        public string password { get; set; }
        public enumClienteException tipoException { get; set; }
    }

    public class ClienteTrocaSenhaRequestModel
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }

    public class ClienteEnviarNovaSenhaRequestModel
    {
        public string email { get; set; }
        public string cpf { get; set; }
    }

    public class ClienteRecuperarSenhaRequestModel
    {
        public string emailcpf { get; set; }
        public string url { get; set; }
    }

    public class ClienteEnviarLinkaRequestModel
    {
        public string email { get; set; }
        public string url { get; set; }
        public string cpf { get; set; }
        public string nome { get; set; }

    }

    public class ClienteAgregadoModel
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public ClienteAgregadoModel(AgregadoModel agregado)
        {
            this.Nome = agregado.Nome;
            this.CPF = agregado.CPF;
            this.Email = agregado.Email;
            this.Telefone = agregado.Telefone;
        }
        public ClienteAgregadoModel()
        {

        }
    }
}