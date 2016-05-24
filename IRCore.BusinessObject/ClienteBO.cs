using Facebook;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using IRCore.Util.Enumerator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Security;
using Flurl;
using IRCore.BusinessObject.Models.AccountKitModel;
using IRCore.BusinessObject.Models.Bileto;
using IRCore.BusinessObject.Models.Bileto.Enums;
using IRCore.DataAccess.ADO.Models;
using Newtonsoft.Json;

namespace IRCore.BusinessObject
{
    public class ClienteBO : MasterBO<ClienteADO>
    {
        protected LoginADO _loginADO;

        #region [ Propriedades Facebook ]

        // Cliente
        private FacebookClient _FacebookClient;

        #endregion
        private string biletoEndpoint;

        #region [ Propriedades Account Kit ]

        // Account Kit
        private readonly string _AccountKitApiUrl = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.accountKitApiUrl);

        #endregion

        private int siteId;

        private string authInfo
        {
            get
            {
                var username = ConfiguracaoAppUtil.Get("BILETO_USERNAME");
                var password = ConfiguracaoAppUtil.Get("BILETO_PASSWORD");
                var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);

                return Convert.ToBase64String(byteArray);
            }
        }

        public List<ClienteComprasCotaNetModel> ListaComprasCotaNet(string cpf)
        {
            return ado.ListaComprasCotaNet(cpf);
        }

        public ClienteBO(MasterADOBase ado)
            : this(1, ado)
        {
        }

        public ClienteBO(int siteId, MasterADOBase ado)
            : base(ado)
        {
            _FacebookClient = new FacebookClient
            {
                AppId = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppID),
                AppSecret = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppSecret)
            };

            _loginADO = new LoginADO(siteId, ado);
            this.ado.SiteId = siteId;
            this.biletoEndpoint = ConfiguracaoAppUtil.Get("BILETO_ENDPOINT");
            this.siteId = siteId;
        }

        public ClienteBO()
            : this(1)
        {
        }

        public ClienteBO(int siteId)
            : base(null)
        {
            _FacebookClient = new FacebookClient();
            _FacebookClient.AppId = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppID);
            _FacebookClient.AppSecret = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppSecret);
            _loginADO = new LoginADO(siteId, ado);
            ado.SiteId = siteId;
        }

        public RetornoModel<Login, enumClienteException> Cadastrar(Login login, int usuarioID = 0)
        {
            // Validações
            if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.CPF) || !login.Email.IsEmail())
            {
                LogUtil.Info(string.Format("##ClienteBO.Cadastrar.EmailouCPFInválido.ERROR## MSG {0}", "E-mail ou CPF utilizado para o cadastro é inválido."));

                return new RetornoModel<Login, enumClienteException>()
                {
                    Sucesso = false,
                    Mensagem = "E-mail ou CPF utilizado para o cadastro é inválido.",
                    Retorno = login,
                    Tipo = enumClienteException.emailInvalido
                };
            }
            else
            {
                //Ajuste Email e CPF
                login.Email = login.Email.Trim().ToLower();
                login.CPF = login.CPF.DigitsOnly();
            }

            if (login.Cliente == null)
            {
                LogUtil.Info(string.Format("##ClienteBO.Cadastrar.ERROR## CLIENTEID {0}, EMAIL {1}, CPF {2}, MSG {3}", login.ClienteID, login.Email, login.CPF, "Objeto Cliente deve estar preenchido"));

                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Objeto Cliente deve estar preenchido", Retorno = login, Tipo = enumClienteException.nenhum };
            }

            var loginByEmail = ConsultarEmail(login.Email, false);
            if ((loginByEmail != null) && (loginByEmail.ClienteID != login.ClienteID))
            {
                LogUtil.Info(string.Format("##ClienteBO.Cadastrar.ERROR## CLIENTEID {0}, EMAIL {1}, CPF {2}, MSG {3}", login.ClienteID, login.Email, login.CPF, "Já existe um usuário cadastrado com este e-mail. Por favor clique em esqueci minha senha ou realize o login."));

                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Já existe um usuário cadastrado com este e-mail. Por favor clique em esqueci minha senha ou realize o login.", Retorno = login, Tipo = enumClienteException.usuarioJaCadastradoComEmail };
            }

            var loginByCpf = ConsultarCPF(login.CPF, false);
            if ((loginByCpf != null) && (loginByCpf.ClienteID != login.ClienteID))
            {
                LogUtil.Info(string.Format("##ClienteBO.Cadastrar.ERROR## CLIENTEID {0}, EMAIL {1}, CPF {2}, MSG {3}", login.ClienteID, login.Email, login.CPF, "Já existe um usuário cadastrado com este CPF. Por favor clique em esqueci minha senha ou realize o login."));

                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Já existe um usuário cadastrado com este CPF. Por favor clique em esqueci minha senha ou realize o login.", Retorno = login, Tipo = enumClienteException.usuarioJaCadastradoComCPF };
            }

            // Ajuste senha
            if (string.IsNullOrWhiteSpace(login.Senha))
            {
                if (!string.IsNullOrWhiteSpace(login.Cliente.Senha))
                {
                    login.Senha = Criptografar(login.Cliente.Senha);
                }
            }
            else
            {
                login.Senha = Criptografar(login.Senha);
            }
            login.Cliente.Senha = string.Empty;

            LogUtil.Info(string.Format("##ClienteBO.Cadastrar## LOGINID {0}, CLIENTEID {1}, EMAIL {2}, CPF {3}, SENHA {4}, FACEBOOKID {5}, ACCOUNTKITID {6}", login.ID, login.ClienteID, login.Email, login.CPF, login.Senha, login.FaceBookUserID, login.AccountKitId));

            // Ajuste objeto Login
            login.AtivoAsBool = true;
            login.StatusAtualAsEnum = enumClienteStatus.liberado;
            login.UltimoAcessoAsDateTime = login.DataCadastroAsDateTime = DateTime.Now;
            login.Cliente.StatusAtual = login.StatusAtual;
            login.Cliente.Ativo = login.Ativo;

            // Ajuste objeto Cliente
            login.Cliente.Email = login.Email;
            login.Cliente.CPF = login.CPF;

            login.Cliente.Cidade = login.Cliente.CidadeEntrega = login.Cliente.CidadeCliente;
            login.Cliente.Estado = login.Cliente.EstadoEntrega = login.Cliente.EstadoCliente;
            login.Cliente.Endereco = login.Cliente.EnderecoEntrega = login.Cliente.EnderecoCliente;
            login.Cliente.Numero = login.Cliente.NumeroEntrega = login.Cliente.NumeroCliente;
            login.Cliente.Complemento = login.Cliente.ComplementoEntrega = login.Cliente.ComplementoCliente;
            login.Cliente.CEP = login.Cliente.CEPEntrega = login.Cliente.CEPCliente;
            login.Cliente.Bairro = login.Cliente.BairroEntrega = login.Cliente.BairroCliente;

            // SiteID em Login e Cliente 
            login.Cliente.SiteID = login.SiteID = this.ado.SiteId;

            Salvar(login, usuarioID);

            LogUtil.Info(string.Format("##ClienteBO.Cadastrar.SUCCESS## LOGINID {0}, CLIENTEID {1}, EMAIL {2}, CPF {3}, SENHA {4}, FACEBOOKID {5}, ACCOUNTKITID {6}", login.ID, login.ClienteID, login.Email, login.CPF, login.Senha, login.FaceBookUserID, login.AccountKitId));

            return VerificarLogin(login, false);
        }

        public RetornoModel<Login, enumClienteException> LogarOsesp(string username, string password)
        {

            var retorno = Logar(username, password);

            if (retorno.Sucesso)
            {
                if (!ado.VerificarOsesp(retorno.Retorno.ClienteID))
                {
                    retorno.Sucesso = false;
                    retorno.Tipo = enumClienteException.osespNaoEncontrado;
                    retorno.Mensagem = "Este Login não é de um Assinante OSESP";
                    retorno.Retorno = null;
                }
            }

            return retorno;
        }

        public RetornoModel<Login, enumClienteException> Logar(string username, string password, int? apiUsuarioId = null)
        {
            var login = ConsultarUsername(username, false);

            if (login == null)
            {
                return new RetornoModel<Login, enumClienteException> { Sucesso = false, Mensagem = "Login ou Senha Inválidos", Tipo = enumClienteException.usuarioNaoEncontrado };
            }

            if (login.Senha == Criptografar(password))
            {
                if (apiUsuarioId.HasValue)
                    return VerificarLogin(login, true, apiUsuarioId);
                else
                    return VerificarLogin(login);
            }

            return new RetornoModel<Login, enumClienteException> { Sucesso = false, Mensagem = "Login ou Senha Inválidos", Tipo = enumClienteException.senhaInvalida };
        }


        private RetornoModel<Login, enumClienteException> VerificarLogin_Old(Login login, bool salvarLogin = true)
        {
            if (salvarLogin)
            {
                login.UltimoAcessoAsDateTime = DateTime.Now;
                _loginADO.Salvar(login);
            }

            if (login.StatusAtualAsEnum == enumClienteStatus.bloqueado || login.Cliente.StatusAtualAsEnum == enumClienteStatus.bloqueado)
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Retorno = login, Mensagem = "Conta bloqueada, por favor entre em contato para maiores informações.", Tipo = enumClienteException.usuarioBloqueado };
            }

            if (login.AtivoAsBool)
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = true, Retorno = login, Mensagem = "OK", Tipo = enumClienteException.nenhum };
            }
            else
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Retorno = login, Mensagem = "Usuário Inativo", Tipo = enumClienteException.usuarioInativo };
            }

        }

        private RetornoModel<Login, enumClienteException> VerificarLogin(Login login, bool salvarUltimoAcesso = true, int? apiUsuarioId = null)
        {
            LogUtil.Info(string.Format("##ClienteBO.VerificaLogin## LOGINID {0}, CLIENTEID {1}, EMAIL {2}, CPF {3}, SALVARULTIMOACESSO {4}", login.ID, login.ClienteID, login.Email, login.CPF, salvarUltimoAcesso));

            if (salvarUltimoAcesso)
            {
                login.UltimoAcessoAsDateTime = DateTime.Now;

                if (login.ID > 0)
                {
                    if (apiUsuarioId.HasValue)
                        _loginADO.AtualizarUltimoAcesso(login, Convert.ToInt32(apiUsuarioId));
                    else
                        _loginADO.AtualizarUltimoAcesso(login);
                }
                else
                {
                    _loginADO.Salvar(login);
                }
            }

            if (login.StatusAtualAsEnum == enumClienteStatus.bloqueado || login.Cliente.StatusAtualAsEnum == enumClienteStatus.bloqueado)
            {
                LogUtil.Debug(string.Format("##ClienteBO.VerificarLogin## MSG {0}", "Conta bloqueada, por favor, entre em contato para maiores informações."));

                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Retorno = login, Mensagem = "Conta bloqueada, por favor, entre em contato para maiores informações.", Tipo = enumClienteException.usuarioBloqueado };
            }

            if (!login.AtivoAsBool)
            {
                LogUtil.Debug(string.Format("##ClienteBO.VerificarLogin## MSG {0}", "Usuário inativo"));

                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Retorno = login, Mensagem = "Usuário inativo", Tipo = enumClienteException.usuarioInativo };
            }

            LogUtil.Info(string.Format("##ClienteBO.VerificaLogin.SUCCESS## LOGINID {0}, CLIENTEID {1}, EMAIL {2}, CPF {3}", login.ID, login.ClienteID, login.Email, login.CPF));

            return new RetornoModel<Login, enumClienteException>() { Sucesso = true, Retorno = login, Mensagem = "OK", Tipo = enumClienteException.nenhum };
        }

        public RetornoModel<string, enumClienteException> FacebookUrl(string absoluteUriCallback)
        {
            try
            {
                var loginUrl = _FacebookClient.GetLoginUrl(new
                {
                    client_id = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppID),
                    client_secret = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppSecret),
                    redirect_uri = absoluteUriCallback,
                    response_type = "code",
                    scope = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppPermissao)
                });
                return new RetornoModel<string, enumClienteException>() { Sucesso = true, Retorno = loginUrl.AbsoluteUri, Mensagem = "OK", Tipo = enumClienteException.nenhum };
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<string, enumClienteException>() { Sucesso = true, Retorno = "", Mensagem = "Erro ao Logar com Facebook", Tipo = enumClienteException.facebookConection };
            }
        }

        public RetornoModel<dynamic, enumClienteException> FacebookGetUsuario(string access_token)
        {
            if (string.IsNullOrEmpty(access_token))
            {
                return new RetornoModel<dynamic, enumClienteException>() { Sucesso = false, Retorno = null, Mensagem = "Erro ao Logar com Facebook", Tipo = enumClienteException.facebookConection };
            }

            try
            {
                _FacebookClient.AccessToken = access_token;
                return new RetornoModel<dynamic, enumClienteException>() { Sucesso = true, Retorno = _FacebookClient.Get("me?fields=id,email,name"), Mensagem = "OK", Tipo = enumClienteException.nenhum };
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<dynamic, enumClienteException>() { Sucesso = true, Retorno = _FacebookClient.Get("me"), Mensagem = "Erro ao Logar com Facebook", Tipo = enumClienteException.facebookConection };
            }
        }

        public RetornoModel<UserModel, enumClienteException> GetUsuarioAccountKit(string accessToken)
        {
            try
            {
                LogUtil.Info(string.Format("##ClienteBO.AccountKitGetUsuario##"));

                if (string.IsNullOrEmpty(accessToken))
                {
                    LogUtil.Debug(string.Format("##ClienteBO.AccountKitGetUsuario## MSG {0}", "Access token nulo ou vazio"));

                    return new RetornoModel<UserModel, enumClienteException>() { Sucesso = false, Retorno = null, Mensagem = "Não foi possível efetuar login com o Account Kit", Tipo = enumClienteException.accountKitConection };
                }

                LogUtil.Debug(string.Format("##ClienteBO.AccountKitGetUsuario## TOKEN {0}", accessToken));

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_AccountKitApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var respostaRequisicao = client.GetAsync(string.Format("me/?access_token={0}", accessToken)).Result;

                    if (!respostaRequisicao.IsSuccessStatusCode)
                    {
                        return new RetornoModel<UserModel, enumClienteException>() { Sucesso = false, Retorno = null, Mensagem = "Não foi possível efetuar login com o Account Kit", Tipo = enumClienteException.accountKitConection };
                    }

                    var respostaAccountKit = JsonConvert.DeserializeObject<UserModel>(respostaRequisicao.Content.ReadAsStringAsync().Result);

                    LogUtil.Info(string.Format("##ClienteBO.AccountKitGetUsuario.SUCCESS## TOKEN {0}", accessToken));

                    return new RetornoModel<UserModel, enumClienteException>() { Sucesso = true, Retorno = respostaAccountKit, Mensagem = "OK", Tipo = enumClienteException.nenhum };
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##ClienteBO.AccountKitGetUsuario.ERROR## TOKEN {0}, MSG {1}", accessToken, ex.Message), ex);
                return new RetornoModel<UserModel, enumClienteException>() { Sucesso = false, Retorno = null, Mensagem = "Não foi possível efetuar login com o Account Kit", Tipo = enumClienteException.accountKitConection };
            }
        }

        public RetornoModel<AccessTokenModel, enumClienteException> GetAccessTokenAccountKit(string code)
        {
            try
            {
                LogUtil.Info(string.Format("##ClienteBO.AccountKitGetAccessToken## CODE {0}", code));

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_AccountKitApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var appId = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.accountKitAppId);
                    var appSecret = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.accountKitAppSecret);
                    var appAccessToken = string.Join("|", new string[] { "EM", appId, appSecret });

                    var respostaRequisicao = client.GetAsync(string.Format("/access_token?grant_type={0}&code={1}&access_token={2}", "authorization_code", code, appAccessToken)).Result;

                    if (!respostaRequisicao.IsSuccessStatusCode)
                    {
                        var respostaRequisicaoErro = JsonConvert.DeserializeObject<ErrorModel>(respostaRequisicao.Content.ReadAsStringAsync().Result);

                        LogUtil.Debug(string.Format("##ClienteBO.AccountKitGetAccessToken.ERROR## MSG {0}", respostaRequisicaoErro.error.message));

                        return new RetornoModel<AccessTokenModel, enumClienteException>() { Sucesso = false, Retorno = null, Mensagem = "Não foi possível efetuar login com o Account Kit", Tipo = enumClienteException.accountKitConection };
                    }

                    var respostaAccountKit = JsonConvert.DeserializeObject<AccessTokenModel>(respostaRequisicao.Content.ReadAsStringAsync().Result);

                    LogUtil.Info(string.Format("##ClienteBO.AccountKitGetAccessToken.SUCCESS## ACCESS TOKEN {0}", respostaAccountKit.accessToken));

                    return new RetornoModel<AccessTokenModel, enumClienteException>() { Sucesso = true, Retorno = respostaAccountKit, Mensagem = "OK", Tipo = enumClienteException.nenhum };
                }

            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##ClienteBO.AccountKitGetAccessToken.ERROR## CODE {0}, MSG {1}", code, ex.Message), ex);
                return new RetornoModel<AccessTokenModel, enumClienteException>() { Sucesso = false, Retorno = null, Mensagem = "Não foi possível efetuar login com o Account Kit", Tipo = enumClienteException.accountKitConection };
            }
        }

        public RetornoModel<dynamic, enumClienteException> FacebookGetUsuario(string code, string absoluteUriCallback)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new RetornoModel<dynamic, enumClienteException>() { Sucesso = false, Retorno = null, Mensagem = "Erro ao Logar com Facebook", Tipo = enumClienteException.facebookConection };
            }
            try
            {
                dynamic result = _FacebookClient.Post("oauth/access_token", new
                {
                    client_id = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppID),
                    client_secret = ConfiguracaoAppUtil.Get(enumConfiguracaoGeral.facebookAppSecret),
                    redirect_uri = absoluteUriCallback,
                    code = code
                });
                return FacebookGetUsuario(result.access_token);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<dynamic, enumClienteException>() { Sucesso = false, Mensagem = ex.Message, Tipo = enumClienteException.facebookConection };
            }
        }

        public RetornoModel<Login, enumClienteException> FacebookLogar(dynamic userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.id))
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Não foi possível efetuar login com o Facebook", Retorno = null, Tipo = enumClienteException.facebookConection };
            }

            try
            {
                Login login = ConsultarFacebook(userInfo.id, false);

                if (login == null)
                {
                    return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Cliente não encontrado", Retorno = login, Tipo = enumClienteException.usuarioNaoEncontrado };
                }

                login.FaceBookUserID = userInfo.id;
                login.FaceBookUserToken = _FacebookClient.AccessToken;
                login.FaceBookUserInfos = userInfo.ToString();
                login.UltimoAcessoAsDateTime = DateTime.Now;

                _loginADO.Salvar(login);

                return VerificarLogin(login);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = ex.Message, Tipo = enumClienteException.facebookConection };
            }
        }

        public RetornoModel<Login, enumClienteException> LogarAccountKit(UserModel usuario)
        {
            try
            {
                LogUtil.Info(string.Format("##ClienteBO.LogarAccountKit##"));

                if (string.IsNullOrEmpty(usuario.id))
                {
                    LogUtil.Info(string.Format("##ClienteBO.LogarAccountKit## MSG {0}", "Account Kit Id nulo ou vazio"));

                    return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Não foi possível efetuar login com o Account Kit", Retorno = null, Tipo = enumClienteException.accountKitConection };
                }

                var login = ConsultarAccountKit(usuario.id, false);

                if (login == null)
                {
                    LogUtil.Info(string.Format("##ClienteBO.ConsultarAccountKit## MSG {0}", "Cliente não encontrado"));

                    return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Cliente não encontrado", Retorno = null, Tipo = enumClienteException.usuarioNaoEncontrado };
                }

                LogUtil.Info(string.Format("##ClienteBO.ConsultarAccountKit.SUCCESS## ACCOUNT KIT ID = {0}, CLIENTE ID {1}", login.AccountKitId, login.ClienteID));

                LogUtil.Info(string.Format("##ClienteBO.ConsultarAccountKit.SalvarLogin## ACCOUNT KIT ID = {0}, CLIENTE ID {1}", login.AccountKitId, login.ClienteID));

                var sucesso = _loginADO.Salvar(login);

                LogUtil.Info(string.Format("##ClienteBO.ConsultarAccountKit.SalvarLogin.SUCCESS## {0} ACCOUNT KIT ID = {1}, CLIENTE ID {2}", sucesso, login.AccountKitId, login.ClienteID));

                return VerificarLogin(login);
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("##ClienteBO.ConsultarAccountKit.ERROR## MSG {0}", ex.Message), ex);

                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = ex.Message, Tipo = enumClienteException.accountKitConection };
            }
        }

        public Login FacebookUsuarioExistente(Login login, enumClienteException tipoException = enumClienteException.nenhum)
        {
            string username = login.Email;
            if (tipoException == enumClienteException.usuarioJaCadastradoComCPF)
            {
                username = login.CPF;
            }

            return ConsultarUsername(username, false);
        }

        public RetornoModel<Login, enumClienteException> FacebookVincular(Login login, dynamic me)
        {
            if (string.IsNullOrEmpty(me.id))
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Erro ao Logar com Facebook", Retorno = login, Tipo = enumClienteException.facebookConection };
            }
            Login loginBd = ConsultarEmail(login.Email, false);
            loginBd.FaceBookUserID = me.id;
            loginBd.FaceBookUserToken = _FacebookClient.AccessToken;
            loginBd.FaceBookUserInfos = me.ToString();
            loginBd.UltimoAcessoAsDateTime = DateTime.Now;

            _loginADO.Salvar(loginBd);

            return VerificarLogin(loginBd, false);
        }

        public RetornoModel<Login, enumClienteException> FacebookVincular(Login login, string password, enumClienteException tipoException = enumClienteException.nenhum)
        {

            Login loginBD = FacebookUsuarioExistente(login, tipoException);

            if (loginBD == null)
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Erro ao Vincular Usuário", Retorno = login, Tipo = enumClienteException.usuarioNaoEncontrado };
            }
            else
            {
                if (loginBD.Senha == Criptografar(password))
                {
                    loginBD.FaceBookUserID = login.FaceBookUserID;
                    loginBD.FaceBookUserToken = login.FaceBookUserToken;
                    loginBD.FaceBookUserInfos = login.FaceBookUserInfos;
                    loginBD.UltimoAcessoAsDateTime = DateTime.Now;

                    _loginADO.Salvar(loginBD);

                    return VerificarLogin(loginBD, false);
                }
                else
                {
                    return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Senha Inválida", Retorno = login, Tipo = enumClienteException.senhaInvalida };
                }

            }
        }

        public Login FacebookLoadCliente(dynamic me)
        {

            Login login = new Login();

            login.Cliente = new tCliente();

            if (me.birthday != null)
            {
                login.Cliente.DataNascimentoAsDateTime = DateTime.ParseExact(me.birthday, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            if (me.gender != null)
            {
                login.Cliente.Sexo = (me.gender == "male") ? "M" : "F";
            }

            if (me.location != null)
            {
                var location = _FacebookClient.Get(me.location.id);
                if ((location.location != null) && (location.location.latitude != null) && (location.location.longitude != null))
                {
                    var localizacao = LocalizacaoUtil.Get(location.location.latitude, location.location.longitude);
                    login.Cliente.CidadeCliente = localizacao.Cidade;
                    login.Cliente.EstadoCliente = localizacao.Estado;
                    login.Cliente.Pais = localizacao.Pais;
                }

            }
            if (string.IsNullOrEmpty(login.Cliente.Pais))
            {
                login.Cliente.Pais = "Brasil";
            }
            if (!string.IsNullOrEmpty(me.first_name))
            {
                login.Cliente.Nome = me.first_name;
            }
            if (!string.IsNullOrEmpty(me.last_name))
            {
                if (string.IsNullOrEmpty(login.Cliente.Nome))
                {
                    login.Cliente.Nome = me.last_name;
                }
                else
                {
                    login.Cliente.Nome += " " + me.last_name;
                }
            }
            if (!string.IsNullOrEmpty(me.email))
            {
                login.Cliente.Email = me.email;
                login.Email = me.email;
            }
            login.FaceBookUserID = me.id;
            login.FaceBookUserToken = _FacebookClient.AccessToken;
            login.FaceBookUserInfos = me.ToString();

            return login;
        }

        public string FacebookGetImagem(Login login, int largura = 60, int altura = 60)
        {
            _FacebookClient.AccessToken = login.FaceBookUserToken;
            dynamic picture = _FacebookClient.Get("me/picture?redirect=false&width=" + largura + "&height=" + altura);
            if ((picture != null) && (picture.data != null) && (picture.data.url != null))
            {
                return picture.data.url;
            }
            return null;
        }

        public RetornoModel<Login, enumUsuarioException> MudarSenha(Login login, string oldPassword, string newPassword)
        {
            if (login.Senha == Criptografar(oldPassword))
            {
                MudarSenha(login, newPassword);
                return new RetornoModel<Login, enumUsuarioException>() { Sucesso = true, Mensagem = "OK", Retorno = login, Tipo = enumUsuarioException.nenhum };
            }
            else
            {
                return new RetornoModel<Login, enumUsuarioException>() { Sucesso = false, Mensagem = "A Senha atual não confere", Retorno = login, Tipo = enumUsuarioException.senhaInvalida };
            }
        }

        public void MudarSenha(Login login, string password)
        {
            login.Senha = Criptografar(password);
            _loginADO.Salvar(login);
        }

        public RetornoModel<Login, enumClienteException> MudarSenha(string token, string password)
        {
            RetornoModel<Login> retornoLogin = ConsultarToken(token, false);
            if (retornoLogin.Sucesso)
            {
                MudarSenha(retornoLogin.Retorno, password);
                return new RetornoModel<Login, enumClienteException>() { Sucesso = true, Mensagem = "OK", Retorno = retornoLogin.Retorno, Tipo = enumClienteException.nenhum };
            }
            else
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = retornoLogin.Mensagem, Retorno = retornoLogin.Retorno, Tipo = enumClienteException.usuarioNaoEncontrado };
            }
        }

        public RetornoModel<Login, enumClienteException> EnviarEmailLinkRedefinirSenha(string nome, string cpf, string email, string absoluteUriCallback)
        {
            Login login = ConsultarUsername(cpf, true);
            if ((login != null) && ((login.Cliente.Nome ?? "").ToLower() == (nome ?? "").ToLower()))
            {
                login.Email = email;
                EnviarEmailLinkRedefinirSenha(login, absoluteUriCallback);
                return new RetornoModel<Login, enumClienteException>() { Sucesso = true, Retorno = login, Mensagem = "OK", Tipo = enumClienteException.nenhum };
            }
            else
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Retorno = login, Mensagem = "Não foi possível localizar nem usuário com este nome e cpf", Tipo = enumClienteException.usuarioNaoEncontrado };
            }
        }

        public RetornoModel<Login, enumClienteException> EnviarEmailLinkRedefinirSenha(string email, string absoluteUriCallback)
        {
            Login login = ConsultarUsername(email, true);
            if (login != null)
            {
                EnviarEmailLinkRedefinirSenha(login, absoluteUriCallback);
                return new RetornoModel<Login, enumClienteException>() { Sucesso = true, Retorno = login, Mensagem = "OK", Tipo = enumClienteException.nenhum };
            }
            else
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Retorno = login, Mensagem = "Não foi possível localizar nem usuário com este email", Tipo = enumClienteException.usuarioNaoEncontrado };
            }
        }

        public RetornoModel<string> EnviarEmailRedefinirSenha(string username, string absoluteUriCallback)
        {
            var login = ConsultarUsername(username, true);
            if (login != null && (!string.IsNullOrWhiteSpace(login.Email) && login.Email.IsEmail())) // Se login existe e e-mail está válido, então...
            {
                EnviarEmailLinkRedefinirSenha(login, absoluteUriCallback);
                var emailFormatado = login.Email.EmailAddressHidingSomeCaracters();

                var retorno = new RetornoModel<string>()
                {
                    Sucesso = true,
                    Retorno = emailFormatado,
                    Mensagem = "OK"
                };

                return retorno;
            }
            else if (login != null && (string.IsNullOrEmpty(login.Email) || !login.Email.IsEmail())) // Senão retorna mensagens de erros...
            {
                var retorno = new RetornoModel<string>()
                {
                    Sucesso = false,
                    Retorno = null,
                    Mensagem = "Nesta conta o email é inexistente ou inválido."
                };

                return retorno;
            }
            else
            {
                var retorno = new RetornoModel<string>()
                {
                    Sucesso = false,
                    Retorno = null,
                    Mensagem = "Conta não encontrada."
                };

                return retorno;
            }
        }

        private void EnviarEmailLinkRedefinirSenha(Login login, string absoluteUriCallback)
        {
            Mail.EnviarNovaSenha(login.Cliente.Nome, absoluteUriCallback.SetQueryParams(new { token = GerarToken(login) }), login.Email);
        }

        public RetornoModel<Login, enumClienteException> EnviarSenha(string email, string cpf)
        {
            Login login = ConsultarEmailCPF(email, cpf, true);
            if (login != null)
            {
                string novasenha = Membership.GeneratePassword(8, 2);
                MudarSenha(login, novasenha);

                Mail.EnviarNovaSenhaMobile(login.Cliente.Nome, novasenha, login.Email);
                return new RetornoModel<Login, enumClienteException>() { Sucesso = true, Mensagem = "OK", Retorno = login, Tipo = enumClienteException.nenhum };
            }
            else
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Não foi possível localizar o usuário com este email e cpf", Retorno = login, Tipo = enumClienteException.usuarioNaoEncontrado };
            }
        }

        public bool Salvar(tCliente cliente, int usuarioId = 0)
        {
            if (usuarioId == 0)
            {
                usuarioId = ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoBO.usuarioIdSistema);
            }
            return ado.Salvar(cliente, usuarioId);
        }

        public bool Salvar(Login login, int usuarioId = 0)
        {
            LogUtil.Info(string.Format("##ClienteBO.Salvar## LOGINID {0}, CLIENTEID {1}, EMAIL {2}, CPF {3}, SENHA {4}, FACEBOOKID {5}, ACCOUNTKITID {6}", login.ID, login.ClienteID, login.Email, login.CPF, login.Senha, login.FaceBookUserID, login.AccountKitId));

            if (Salvar(login.Cliente, usuarioId))
            {
                login.ClienteID = login.Cliente.ID;
                var result = _loginADO.Salvar(login);

                LogUtil.Info(string.Format("##ClienteBO.Salvar.SUCCESS## LOGINID {0}, CLIENTEID {1}, EMAIL {2}, CPF {3}, SENHA {4}, FACEBOOKID {5}, ACCOUNTKITID {6}", login.ID, login.ClienteID, login.Email, login.CPF, login.Senha, login.FaceBookUserID, login.AccountKitId));

                return result;
            }

            LogUtil.Info(string.Format("##ClienteBO.Salvar.ERROR## LOGINID {0}, CLIENTEID {1}, EMAIL {2}, CPF {3}, SENHA {4}, FACEBOOKID {5}, ACCOUNTKITID {6}, MSG {7}", login.ID, login.ClienteID, login.Email, login.CPF, login.Senha, login.FaceBookUserID, login.AccountKitId, "Não foi possivel cadastrar ou atualizar o Cliente"));

            return false;
        }

        public tClienteEndereco Consultar(int clienteID, string CEP, string numero)
        {
            return ado.Consultar(clienteID, CEP, numero);
        }

        public tCliente Salvar(tClienteEndereco endereco, tCliente cliente, int usuarioId = 0)
        {
            int enderecoId = InserirEndereco(endereco);
            if (enderecoId > 0)
            {
                if (string.IsNullOrEmpty(cliente.CEP))
                {
                    cliente.BairroCliente = endereco.Bairro;
                    cliente.CEPCliente = endereco.CEP;
                    cliente.CidadeCliente = endereco.Cidade;
                    cliente.ComplementoCliente = endereco.Complemento;
                    cliente.EnderecoCliente = endereco.Endereco;
                    cliente.EstadoCliente = endereco.Estado;
                    cliente.NumeroCliente = endereco.Numero;
                    cliente.StatusConsulta = endereco.StatusConsulta;
                    ado.Salvar(cliente, usuarioId);
                }
                cliente = CarregarEnderecos(cliente);
            }
            return cliente;
        }

        public RetornoModel<tClienteEndereco> SalvarEndereco(tClienteEndereco endereco, tCliente cliente, int usuarioId = 0)
        {
            int enderecoId = InserirEndereco(endereco);
            if (enderecoId > 0)
            {
                if (string.IsNullOrEmpty(cliente.CEP))
                {
                    cliente.BairroCliente = endereco.Bairro;
                    cliente.CEPCliente = endereco.CEP;
                    cliente.CidadeCliente = endereco.Cidade;
                    cliente.ComplementoCliente = endereco.Complemento;
                    cliente.EnderecoCliente = endereco.Endereco;
                    cliente.EstadoCliente = endereco.Estado;
                    cliente.NumeroCliente = endereco.Numero;
                    cliente.StatusConsulta = endereco.StatusConsulta;
                    ado.Salvar(cliente, usuarioId);
                }
                return new RetornoModel<tClienteEndereco>() { Retorno = endereco, Mensagem = "OK", Sucesso = true };
            }
            return new RetornoModel<tClienteEndereco>() { Retorno = endereco, Mensagem = "Ocorreu um erro ao cadastrar o endereço", Sucesso = false };
        }

        public bool Remover(tClienteEndereco endereco, int usuarioId = 0)
        {
            if (usuarioId == 0)
            {
                usuarioId = ConfiguracaoAppUtil.GetAsInt(enumConfiguracaoBO.usuarioIdSistema);
            }
            return ado.Remover(endereco, usuarioId);
        }

        private Login _CarregarLoginFromCliente(tCliente cliente, bool salvarLogin = true)
        {
            if (string.IsNullOrWhiteSpace(cliente.Email) && !cliente.Email.IsEmail())
            {
                LogUtil.Info(string.Format("##ClienteBO._CarregarLoginFromCliente.ERROR## Cliente {0}, EMAIL {1}, CPF {2} MSG {3}", cliente.ID, cliente.Email, cliente.CPF, "Cadastro do cliente na tCliente não possui email."));

                throw new Exception("Não existe um e-mail cadastrado.");
            }

            var login = new Login
            {
                ClienteID = cliente.ID,
                CPF = cliente.CPF,
                Email = cliente.Email,
                AtivoAsBool = true,
                SiteID = cliente.SiteID,
                Senha = !string.IsNullOrWhiteSpace(cliente.Senha) ? Criptografar(cliente.Senha) : "",
                StatusAtualAsEnum = enumClienteStatus.liberado,
                UltimoAcessoAsDateTime = DateTime.Now,
                DataCadastroAsDateTime = DateTime.Now,
                Cliente = cliente
            };

            if (salvarLogin)
            {
                _loginADO.Salvar(login);
            }
            return login;
        }

        public Login Consultar(int clienteID, bool salvarLogin = true)
        {
            LogUtil.Debug(string.Format("##ClienteBO.Consultar## CLIENTE {0}", clienteID));

            Login login = _loginADO.Consultar(clienteID);
            if (login == null)
            {
                tCliente cliente = ado.Consultar(clienteID);
                if (cliente != null)
                {
                    login = _CarregarLoginFromCliente(cliente, salvarLogin);
                }
            }
            else
            {
                login.Cliente = ado.Consultar(login.ClienteID);
                if (login.Cliente == null)
                {
                    login = null;
                }
            }

            LogUtil.Debug(string.Format("##ClienteBO.Consultar.SUCCESS## CLIENTE {0} LOGIN_CPF {1}, LOGIN_EMAIL {2}", clienteID, login.CPF, login.Email));

            return login;
        }

        public Login ConsultarOSESP(int clienteID, bool salvarLogin = true)
        {
            Login login = _loginADO.ConsultarOSESP(clienteID);
            if (login == null)
            {
                tCliente cliente = ado.ConsultarOSESP(clienteID);
                if (cliente != null)
                {
                    login = _CarregarLoginFromCliente(cliente, salvarLogin);
                }
            }
            else
            {
                login.Cliente = ado.ConsultarOSESP(login.ClienteID);
                if (login.Cliente == null)
                {
                    login = null;
                }
            }

            return login;
        }

        public Login ConsultarEmailCPF(string email, string cpf, bool salvarLogin = true)
        {
            Login login = _loginADO.ConsultarEmailCPF(email, cpf);
            if (login == null)
            {
                tCliente cliente = ado.ConsultarEmailCPF(email, cpf);
                if (cliente != null)
                {
                    login = _CarregarLoginFromCliente(cliente, salvarLogin);
                }
            }
            else
            {
                login.Cliente = ado.Consultar(login.ClienteID);
                if (login.Cliente == null)
                {
                    login = null;
                }
            }
            return login;
        }

        public Login ConsultarCPF(string cpf, bool salvarLogin = true)
        {
            LogUtil.Debug(string.Format("##ClienteBO.ConsultarCPF##"));

            Login login = _loginADO.ConsultarCPF(cpf);

            return login;
        }

        public Login ConsultarEmail(string email, bool salvarLogin = true)
        {
            LogUtil.Debug(string.Format("##ClienteBO.ConsultarEmail##"));

            Login login = _loginADO.ConsultarEmail(email);

            return login;
        }

        public Login ConsultarUsername(string username, bool salvarLogin = true)
        {
            LogUtil.Debug(string.Format("##ClienteBO.ConsultarUserName## USERNAME {0}", username));

            Login login = _loginADO.ConsultarUsername(username);

            if (login != null)
            {
                login.Cliente = ado.Consultar(login.ClienteID);

                if (login.Cliente == null)
                {
                    login = null;
                }
            }

            return login;
        }

        public RetornoModel<Login> ConsultarToken(string token, bool salvarLogin = true)
        {
            token = HttpUtility.UrlDecode(token.Replace("$$$", "%"));
            string chaveToken = Decriptografar(token);
            string clienteIDString = chaveToken.LeftOfIndexOf("+");
            string data = chaveToken.RightOfIndexOf("+").LeftOfIndexOf("|");
            string senha = chaveToken.RightOfIndexOf("|");
            if (clienteIDString != chaveToken && data != chaveToken && senha != chaveToken)
            {
                try
                {
                    int clienteID = Convert.ToInt32(clienteIDString);
                    DateTime date = DateTime.ParseExact(data, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    if (DateTime.Now <= date.AddDays(2))
                    {
                        Login login = Consultar(clienteID, salvarLogin);
                        if (login != null && login.Senha == senha)
                        {
                            return new RetornoModel<Login>() { Sucesso = true, Mensagem = "OK", Retorno = login };
                        }
                        else
                        {
                            return new RetornoModel<Login>() { Sucesso = false, Mensagem = "Este token de recuperação de senha já foi utilizado, ou não representa um token válido. Tente novamente recuperar sua senha!", Retorno = null };
                        }
                    }
                    return new RetornoModel<Login>() { Sucesso = false, Mensagem = "O link enviado no e-mail de Alteração de Senha expira após 2 dias de seu envio, se você não conseguiu recuperar. Tente novamente recuperar sua senha!", Retorno = null };
                }
                catch (Exception ex)
                {
                    return new RetornoModel<Login>() { Sucesso = false, Mensagem = "Provavelmente este link já tenha sido utilizado, ou não representa um código válido. Tente novamente recuperar sua senha!", Retorno = null };
                }

            }
            return new RetornoModel<Login>() { Sucesso = false, Mensagem = "Provavelmente este link já tenha sido utilizado, ou não representa um código válido. Tente novamente recuperar sua senha!", Retorno = null };
        }

        public string GerarToken(Login login)
        {
            string chaveToken = Criptografar(login.ClienteID + "+" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|" + login.Senha);
            chaveToken = HttpUtility.UrlEncode(chaveToken).Replace("%", "$$$");
            return chaveToken;
        }

        public Login ConsultarFacebook(string _faceBookUserId, bool salvarLogin = true)
        {
            var login = _loginADO.ConsultarFacebook(_faceBookUserId);

            if (login != null)
            {
                login.Cliente = ado.Consultar(login.ClienteID);
            }

            return login;
        }

        public Login ConsultarAccountKit(string id, bool salvarLogin = true)
        {
            LogUtil.Info(string.Format("##ConsultarAccountKit## ACCOUNT KIT ID {0}", id));

            var login = _loginADO.ConsultarAccountKit(id);

            if (login != null)
            {
                login.Cliente = ado.Consultar(login.ClienteID);
            }

            return login;
        }

        public tCliente CarregarEnderecos(tCliente cliente)
        {
            LogUtil.Debug(string.Format("##ClienteBO.CarregarEnderecos## CLIENTE {0}", cliente.ID));

            cliente.EnderecoList = ado.ListarEndereco(cliente.ID);
            cliente = CarregarEnderecoCadastro(cliente);

            LogUtil.Debug(string.Format("##ClienteBO.CarregarEnderecos.SUCCESS## CLIENTE {0}, END_COUNT {1}", cliente.ID, cliente.EnderecoList.Count));

            return cliente;
        }

        public tCliente CarregarEnderecoCadastro(tCliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.CEPCliente) || string.IsNullOrEmpty(cliente.NumeroCliente))
            {
                return cliente;
            }

            var clienteEndereco = cliente.EnderecoList.FirstOrDefault(t => t.CEP != null && t.Numero != null && t.CEP.Trim() == cliente.CEPCliente.Trim() && t.Numero.Trim() == cliente.NumeroCliente.Trim());
            if (clienteEndereco != null)
            {
                return cliente;
            }

            clienteEndereco = new tClienteEndereco();
            clienteEndereco.Bairro = cliente.BairroCliente;
            clienteEndereco.CEP = cliente.CEPCliente;
            clienteEndereco.Cidade = cliente.CidadeCliente;
            clienteEndereco.ClienteID = cliente.ID;
            clienteEndereco.Complemento = cliente.ComplementoCliente;
            clienteEndereco.CPF = cliente.CPF;
            clienteEndereco.Endereco = cliente.EnderecoCliente;
            clienteEndereco.EnderecoPrincipal = "T";
            clienteEndereco.EnderecoTipoID = 1;
            clienteEndereco.Estado = cliente.EstadoCliente;
            clienteEndereco.Nome = cliente.Nome;
            clienteEndereco.Numero = cliente.NumeroCliente;
            clienteEndereco.RG = cliente.RG;
            clienteEndereco.StatusConsulta = cliente.StatusConsulta;
            InserirEndereco(clienteEndereco);
            cliente.EnderecoList.Add(clienteEndereco);

            return cliente;
        }

        public tClienteEndereco ConsultarEndereco(int clienteEnderecoID)
        {
            return ado.ConsultarEndereco(clienteEnderecoID);
        }

        public tClienteEndereco ConsultarEndereco(string cep, string endereco, string numero, string complemento, int clienteID)
        {
            return ado.ConsultarEndereco(cep, endereco, numero, complemento, clienteID);
        }

        public bool VerificarCPF(string cpf)
        {
            Login login = _loginADO.ConsultarCPF(cpf);
            if (login == null)
            {
                tCliente cliente = ado.ConsultarCPF(cpf);
                return (cliente != null);
            }
            return true;
        }

        public bool VerificarCNPJ(string cnpj)
        {
            throw new NotImplementedException();
        }

        public bool VerificarEmail(string email)
        {
            Login login = _loginADO.ConsultarEmail(email);
            if (login == null)
            {
                tCliente cliente = ado.ConsultarEmail(email);
                return (cliente != null);
            }
            return true;
        }

        public bool VerificarEmailCPF(string email, string cpf)
        {
            Login login = _loginADO.ConsultarEmailCPF(email, cpf);
            if (login == null)
            {
                tCliente cliente = ado.ConsultarEmailCPF(email, cpf);
                return (cliente != null);
            }
            return true;
        }

        public List<tContatoTipo> ListarContatoTipo()
        {
            return ado.ListarContatoTipo();
        }

        public static string Criptografar(string str)
        {
            string hashedPassword = IRLib.Criptografia.Crypto.Criptografar(str, ConfiguracaoAppUtil.Get(enumConfiguracaoBO.chaveCriptografiaLogin));
            return hashedPassword;
        }

        public static bool VerfificarCriptografia(string strCriptografada, string str)
        {
            return (strCriptografada.Equals(Criptografar(str)));
        }

        public static string Decriptografar(string strCriptografada)
        {
            string unHashedPassword = IRLib.Criptografia.Crypto.Decriptografar(strCriptografada, ConfiguracaoAppUtil.Get(enumConfiguracaoBO.chaveCriptografiaLogin));
            return unHashedPassword;
        }

        public List<tCartao> ListarCartoes(List<int> ids)
        {
            return ado.ListarCartoes(ids);
        }

        public int InserirEndereco(tClienteEndereco clienteEndereco)
        {
            return ado.InserirEndereco(clienteEndereco);
        }

        #region [ Bileto API ]

        private HttpResponseMessage Post<T>(HttpClient client, string requestUri, T value, bool biletoKey = false, string biletoToken = null)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);

            if (biletoKey)
            {
                client.DefaultRequestHeaders.Add("x-bileto-key", ((Site)this.siteId).ToString());
            }

            if (!string.IsNullOrEmpty(biletoToken))
            {
                client.DefaultRequestHeaders.Add("x-auth-token", biletoToken);
            }

            var response = client.PostAsJsonAsync<T>(requestUri, value).Result;
            return response;
        }

        private HttpResponseMessage Get(string _requestUri, HttpClient client, bool biletoKey = false, string biletoToken = null)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);

            if (biletoKey)
            {
                client.DefaultRequestHeaders.Add("x-bileto-key", ((Site)this.siteId).ToString());
            }

            if (!string.IsNullOrEmpty(biletoToken))
            {
                client.DefaultRequestHeaders.Add("x-auth-token", biletoToken);
            }

            var response = client.GetAsync(_requestUri).Result;
            return response;
        }

        public RetornoModel<Login, enumClienteException> BiletoSignIn(string _username, string _password, int usuarioId, int? apiUsuarioId = null)
        {
            var retorno = new RetornoModel<Login, enumClienteException>();

            using (var client = new HttpClient())
            {
                var account = new AccountSignIn
                {
                    owner = _username.ToLower(),
                    credential = _password,
                    authenticationPlatform = (int)AuthenticationPlatform.EMAIL_PASSWORD
                };

                var requestUri = biletoEndpoint + "/account/sign_in";
                HttpResponseMessage response;

                try
                {
                    response = this.Post(client, requestUri, account, true);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoSignIn.ERROR## EMAIL: {0}, MSG: {1}, ERROR: {2}", _username, "Bileto offline", ex.Message), ex);
                    retorno.Tipo = enumClienteException.biletoOffline;
                    retorno.Sucesso = false;
                    retorno.Mensagem = "";
                    return retorno;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //successful
                    LogUtil.Info(string.Format("##ClienteBO.BiletoSignIn.SUCCESS## EMAIL: '{0}' STATUS_CODE: '{1}'", _username, response.StatusCode));
                    var biletoToken = response.Headers.GetValues("x-auth-token").First();

                    retorno.Sucesso = true;
                    retorno.Mensagem = "OK";

                    var login = this.ConsultarUsername(_username, false);

                    //se login for nulo (tem cadastro no Bileto e não tem na IR), deve ser cadastrado local
                    if (login == null)
                    {
                        requestUri = biletoEndpoint + "/account";
                        response = this.Get(requestUri, client, true, biletoToken);

                        login = (Login)response.Content.ReadAsAsync<Account>().Result;
                        login.Senha = login.Cliente.Senha = _password;
                        login.SiteID = login.Cliente.SiteID = this.siteId;
                        login.SyncStatus = (char)SyncStatus.SUCCESS;
                        login.SyncDate = DateTime.Now;

                        this.Cadastrar(login, usuarioId);
                    }
                    else
                    {
                        //Marcar esse login pra ser atualizado no Bileto pelo Robô 
                        var updateFlag = this.AtualizarLoginNaBileto(login.ID);
                        LogUtil.Info(string.Format("##ClienteBO.BiletoSignIn.AtualizarLoginNaBileto## USER: {0}, FLEGADO: {1}", _username, updateFlag));
                    }

                    if (apiUsuarioId.HasValue)
                    {
                        login.UltimoAcessoAsDateTime = DateTime.Now;

                        if (login.ID > 0)
                        {
                            _loginADO.AtualizarUltimoAcesso(login, Convert.ToInt32(apiUsuarioId));
                        }
                    }

                    retorno.Retorno = login;
                    retorno.Retorno.BiletoToken = biletoToken;

                    return retorno;
                }

                if (IsServerError(response.StatusCode))
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoSignIn.ERROR## EMAIL: {0}, STATUS_CODE: {1}, MSG: {2}", _username, response.StatusCode, "Bileto offline"));
                    retorno.Tipo = enumClienteException.biletoOffline;
                    retorno.Sucesso = false;
                    retorno.Mensagem = "";
                    return retorno;
                }

                var errorInfo = response.Content.ReadAsAsync<ErrorInfo>().Result ?? new ErrorInfo();
                LogUtil.Info(string.Format("##ClienteBO.BiletoSignIn.ERROR## EMAIL: '{0}' STATUS_CODE: '{1}', MSG: '{2}', ERRORS: '{3}'", _username, response.StatusCode, errorInfo.message, string.Join(",", errorInfo.errors)));

                retorno.Tipo = enumClienteException.senhaInvalida;
                retorno.Sucesso = false;
                retorno.Mensagem = string.Join(",", errorInfo.errors);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    retorno.Mensagem = "Usuário ou senha inválidos.";
                }

                return retorno;
            }
        }

        public RetornoModel<Login, enumClienteException> BiletoFacebookSignIn(dynamic _facebookInfo, string _facebookToken)
        {
            if (string.IsNullOrEmpty(_facebookInfo.id))
            {
                return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Não foi possível efetuar login com o Facebook", Retorno = null, Tipo = enumClienteException.facebookConection };
            }

            var retorno = new RetornoModel<Login, enumClienteException>();

            using (var client = new HttpClient())
            {
                var account = new AccountSignIn
                {
                    owner = _facebookInfo.id,
                    credential = _facebookToken,
                    authenticationPlatform = (int)AuthenticationPlatform.FACEBOOK
                };

                var requestUri = biletoEndpoint + "/account/sign_in";
                HttpResponseMessage response;

                try
                {
                    response = this.Post(client, requestUri, account, true);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoFacebookSignIn.ERROR## FACEBOOKID: {0}, MSG: {1}, ERROR: {2}", _facebookInfo.id, "Bileto offline", ex.Message), ex);
                    retorno.Tipo = enumClienteException.biletoOffline;
                    retorno.Sucesso = false;
                    retorno.Mensagem = "";
                    return retorno;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //successful
                    LogUtil.Info(string.Format("##ClienteBO.BiletoFacebookSignIn.SUCCESS## FACEBOOKID: '{0}' STATUS_CODE: '{1}'", _facebookInfo.id, response.StatusCode));
                    var biletoToken = response.Headers.GetValues("x-auth-token").First();

                    var login = this.ConsultarFacebook(_facebookInfo.id, false);

                    if (login != null)
                    {
                        login.FaceBookUserID = _facebookInfo.id;
                        login.FaceBookUserToken = _facebookToken;
                        login.FaceBookUserInfos = _facebookInfo.ToString();
                        login.UltimoAcessoAsDateTime = DateTime.Now;

                        _loginADO.Salvar(login);

                        //Marcar esse login pra ser atualizado no Bileto pelo Robô 
                        var updateFlag = this.AtualizarLoginNaBileto(login.ID);
                        LogUtil.Info(string.Format("##ClienteBO.BiletoFacebookSignIn.AtualizarLoginNaBileto## FACEBOOK_ID: {0}, FLEGADO: {1}", _facebookInfo.id, updateFlag));

                        retorno = VerificarLogin(login, false);

                        retorno.Mensagem = "OK";
                        retorno.Retorno.BiletoToken = biletoToken;
                    }
                    else
                    {
                        retorno.Sucesso = false;
                        retorno.Mensagem = "Cliente não encontrado";
                        retorno.Retorno = null;
                        retorno.Tipo = enumClienteException.usuarioNaoEncontrado;
                    }

                    return retorno;
                }

                if (IsServerError(response.StatusCode))
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoFacebookSignIn.ERROR## FACEBOOK_ID: {0}, STATUS_CODE: {1}, MSG: {2}", _facebookInfo.id, response.StatusCode, "Bileto offline"));
                    retorno.Tipo = enumClienteException.biletoOffline;
                    retorno.Sucesso = false;
                    retorno.Mensagem = "";
                    return retorno;
                }

                var errorInfo = response.Content.ReadAsAsync<ErrorInfo>().Result ?? new ErrorInfo();
                LogUtil.Info(string.Format("##ClienteBO.BiletoFacebookSignIn.ERROR## FACEBOOK_ID: '{0}' STATUS_CODE: '{1}', MSG: '{2}', ERRORS: '{3}'", _facebookInfo.id, response.StatusCode, errorInfo.message, string.Join(",", errorInfo.errors)));

                retorno.Sucesso = false;
                retorno.Mensagem = string.Join(",", errorInfo.errors);

                return retorno;
            }
        }

        public RetornoModel<Login, enumClienteException> BiletoSignUp(Login _login, int _usuarioId)
        {
            var retorno = new RetornoModel<Login, enumClienteException>();

            if (_login.Cliente == null)
            {
                _login.Cliente = new tCliente();
            }
            if (!string.IsNullOrEmpty(_login.Senha))
            {
                _login.Cliente.Senha = _login.Senha;
            }
            _login.SiteID = _login.Cliente.SiteID = this.siteId;

            using (var client = new HttpClient())
            {
                var accountInfo = (AccountInfo)_login;

                HttpResponseMessage response;

                try
                {
                    response = this.Post(client, biletoEndpoint + "/admin/account/import", accountInfo);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoSignUp.ERROR## EMAIL: {0}, MSG: {1}, ERROR: {2}", _login.Email, "Bileto offline", ex.Message), ex);
                    retorno.Tipo = enumClienteException.biletoOffline;
                    retorno.Sucesso = false;
                    retorno.Mensagem = "";
                    return retorno;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //successful
                    LogUtil.Info(string.Format("##ClienteBO.BiletoSignUp.SUCCESS## EMAIL: '{0}' ", _login.Email));

                    var login = (Login)response.Content.ReadAsAsync<ExportResponse>().Result;
                    login.Senha = login.Cliente.Senha = _login.Senha;
                    login.SiteID = login.Cliente.SiteID = this.siteId;
                    login.SyncStatus = (char)SyncStatus.SUCCESS;
                    login.SyncDate = DateTime.Now;

                    login.CreatedAPIUsuarioId = _login.CreatedAPIUsuarioId;
                    login.LoginAPIUsuarioId = _login.LoginAPIUsuarioId;

                    if (!string.IsNullOrEmpty(login.Cliente.CNPJ))
                    {
                        login.Cliente.Nome = _login.Cliente.Nome.ToUpper();
                        login.Cliente.CPF = _login.CPF.DigitsOnly();
                        login.CPF = _login.CPF.DigitsOnly();
                        login.Cliente.RazaoSocial = _login.Cliente.RazaoSocial;
                        login.Cliente.NomeFantasia = _login.Cliente.NomeFantasia;
                    }

                    this.Cadastrar(login, _usuarioId);

                    retorno.Sucesso = true;
                    retorno.Retorno = login;
                    retorno.Mensagem = "OK";

                    return retorno;
                }

                if (IsServerError(response.StatusCode))
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoSignUp.ERROR## EMAIL: {0}, STATUS_CODE: {1}, MSG: {2}", _login.Email, response.StatusCode, "Bileto offline"));
                    retorno.Tipo = enumClienteException.biletoOffline;
                    retorno.Sucesso = false;
                    retorno.Mensagem = "";
                    return retorno;
                }

                var errorInfo = response.Content.ReadAsAsync<ErrorInfo>().Result ?? new ErrorInfo();
                LogUtil.Info(string.Format("##ClienteBO.BiletoSignUp.ERROR## EMAIL: '{0}' STATUS_CODE: '{1}', MSG: '{2}', ERRORS: '{3}'", _login.Email, response.StatusCode, errorInfo.message, string.Join(",", errorInfo.errors)));

                retorno.Sucesso = false;
                retorno.Mensagem = string.Join(",", errorInfo.errors);

                var duplicatedMsgs = new[] { "DUPLICATED_CREDENTIAL", "DUPLICATED_EMAIL" };
                if (duplicatedMsgs.Contains(retorno.Mensagem))
                {
                    retorno.Mensagem = "Email já cadastrado.";
                }

                return retorno;
            }
        }

        public RetornoModel<Login, enumClienteException> BiletoFacebookSignUp(Login loginInfo, int _usuarioId)
        {

            var retorno = new RetornoModel<Login, enumClienteException>();

            using (var client = new HttpClient())
            {
                //try to register the user
                var requestUri = biletoEndpoint + "/account/sign_up";
                var accountInfo = (AccountSignUp)loginInfo;
                HttpResponseMessage response;

                try
                {
                    response = this.Post(client, requestUri, accountInfo, true);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoFacebookSignUp.ERROR## EMAIL: {0}, MSG: {1}, ERROR: {2}", loginInfo.Email, "Bileto offline", ex.Message), ex);
                    retorno.Tipo = enumClienteException.biletoOffline;
                    retorno.Sucesso = false;
                    retorno.Mensagem = "";
                    return retorno;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //successful
                    LogUtil.Info(string.Format("##ClienteBO.BiletoFacebookSignUp.SUCCESS## EMAIL: '{0}' ", loginInfo.Email));

                    //signin the user to get the "x-auth-token" to get the user details
                    requestUri = biletoEndpoint + "/account/sign_in";
                    var account = new AccountSignIn { owner = loginInfo.FaceBookUserID, credential = loginInfo.FaceBookUserToken, authenticationPlatform = (int)AuthenticationPlatform.FACEBOOK };
                    response = this.Post(client, requestUri, account, true);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //get details
                        var authToken = response.Headers.GetValues("x-auth-token").First();
                        requestUri = biletoEndpoint + "/account";
                        response = this.Get(requestUri, client, true, authToken);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var login_ = (Login)response.Content.ReadAsAsync<Account>().Result;

                            login_.SyncStatus = (char)SyncStatus.SUCCESS;
                            login_.SyncDate = DateTime.Now;
                            login_.FaceBookUserID = loginInfo.FaceBookUserID;
                            login_.FaceBookUserToken = loginInfo.FaceBookUserToken;
                            login_.SiteID = login_.Cliente.SiteID = this.siteId;

                            this.Cadastrar(login_, _usuarioId);

                            retorno.Sucesso = true;
                            retorno.Retorno = login_;
                            retorno.Mensagem = "OK";

                            return retorno;
                        }
                    }
                }

                if (IsServerError(response.StatusCode))
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoFacebookSignUp.ERROR## EMAIL: {0}, STATUS_CODE: {1}, MSG: {2}", loginInfo.Email, response.StatusCode, "Bileto offline"));
                    retorno.Tipo = enumClienteException.biletoOffline;
                    retorno.Sucesso = false;
                    retorno.Mensagem = "";
                    return retorno;
                }

                var errorInfo = response.Content.ReadAsAsync<ErrorInfo>().Result ?? new ErrorInfo();
                LogUtil.Info(string.Format("##ClienteBO.BiletoFacebookSignUp.ERROR## EMAIL: '{0}' STATUS_CODE: '{1}', MSG: '{2}', ERRORS: '{3}'", loginInfo.Email, response.StatusCode, errorInfo.message, string.Join(",", errorInfo.errors)));

                retorno.Sucesso = false;
                retorno.Mensagem = string.Join(",", errorInfo.errors);

                var duplicatedMsgs = new[] { "DUPLICATED_CREDENTIAL", "DUPLICATED_EMAIL" };
                if (duplicatedMsgs.Contains(retorno.Mensagem))
                {
                    retorno.Mensagem = "Cliente já cadastrado com esse facebook.";
                }

                return retorno;
            }
        }

        public RetornoModel<string> BiletoForgottenPassword(string _emailcpf, string _url)
        {
            var retorno = new RetornoModel<string>();

            using (var client = new HttpClient())
            {
                var forgottenPassword_ = new ForgottenPassword
                {
                    email = _emailcpf,
                    redirectURL = _url,
                    externalClient = "WEB"
                };

                var requestUri = biletoEndpoint + "/account/forgotten_password";
                HttpResponseMessage response;

                try
                {
                    response = this.Post(client, requestUri, forgottenPassword_, true);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoForgottenPassword.ERROR## EMAILCPF: {0}, MSG: {1}, ERROR: {2}", _emailcpf, "Bileto offline", ex.Message), ex);
                    retorno.Sucesso = false;
                    retorno.Mensagem = "biletoOffline";
                    return retorno;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //successful
                    LogUtil.Info(string.Format("##ClienteBO.BiletoForgottenPassword.SUCCESS## EMAILCPF: '{0}' STATUS_CODE: '{1}'", _emailcpf, response.StatusCode));

                    retorno.Sucesso = true;
                    retorno.Mensagem = "OK";

                    return retorno;
                }

                if (IsServerError(response.StatusCode))
                {
                    LogUtil.Error(string.Format("##ClienteBO.BiletoForgottenPassword.ERROR## EMAILCPF: {0}, STATUS_CODE: {1}, MSG: {2}", _emailcpf, response.StatusCode, "Bileto offline"));
                    retorno.Sucesso = false;
                    retorno.Mensagem = "biletoOffline";
                    return retorno;
                }

                var errorInfo = response.Content.ReadAsAsync<ErrorInfo>().Result ?? new ErrorInfo();
                LogUtil.Info(string.Format("##ClienteBO.BiletoForgottenPassword.ERROR## EMAILCPF: '{0}' STATUS_CODE: '{1}', MSG: '{2}', ERRORS: '{3}'", _emailcpf, response.StatusCode, errorInfo.message, string.Join(",", errorInfo.errors)));

                retorno.Sucesso = false;
                retorno.Mensagem = string.Join(",", errorInfo.errors);

                return retorno;
            }
        }

        public RetornoModel<Login, enumUsuarioException> ChangePassword(string _oldPassword, string _newPassword, string _token)
        {
            var retorno = new RetornoModel<Login, enumUsuarioException>();

            using (var client = new HttpClient())
            {
                var forgottenPassword_ = new ChangePassword
                {
                    currentPassword = _oldPassword,
                    newPassword = _newPassword,
                };

                var requestUri = biletoEndpoint + "/account/change_password";
                HttpResponseMessage response;

                try
                {
                    response = this.Post(client, requestUri, forgottenPassword_, true, _token);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##ClienteBO.ChangePassword.ERROR## BILETOTOKEN: {0}, MSG: {1}, ERROR: {2}", _token, "Bileto offline", ex.Message), ex);
                    retorno.Sucesso = false;
                    retorno.Mensagem = "biletoOffline";
                    return retorno;
                }

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //successful
                    LogUtil.Info(string.Format("##ClienteBO.ChangePassword.SUCCESS## BILETOTOKEN: '{0}' STATUS_CODE: '{1}'", _token, response.StatusCode));

                    retorno.Sucesso = true;
                    retorno.Mensagem = "OK";

                    return retorno;
                }

                if (IsServerError(response.StatusCode))
                {
                    LogUtil.Error(string.Format("##ClienteBO.ChangePassword.ERROR## BILETOTOKEN: {0}, STATUS_CODE: {1}, MSG: {2}", _token, response.StatusCode, "Bileto offline"));
                    retorno.Sucesso = false;
                    retorno.Mensagem = "biletoOffline";
                    return retorno;
                }

                var errorInfo = response.Content.ReadAsAsync<ErrorInfo>().Result ?? new ErrorInfo();
                LogUtil.Info(string.Format("##ClienteBO.ChangePassword.ERROR## BILETOTOKEN: '{0}' STATUS_CODE: '{1}', MSG: '{2}', ERRORS: '{3}'", _token, response.StatusCode, errorInfo.message, string.Join(",", errorInfo.errors)));

                retorno.Sucesso = false;
                retorno.Mensagem = string.Join(",", errorInfo.errors);

                if ((int)response.StatusCode == 401)
                {
                    retorno.Mensagem = "A Senha atual não confere.";
                }

                return retorno;
            }
        }

        #endregion

        #region [ Database operations]

        public bool AtualizarLoginNaBileto(int loginId)
        {
            return ado.AtualizarLoginNaBileto(loginId);
        }

        #endregion

        public static bool IsServerError(HttpStatusCode response)
        {
            return (int)response >= 500 && (int)response <= 599;
        }

        public bool BloquearCliente(int ClienteID)
        {
            var bloqueado = ado.BloquearCliente(ClienteID);
            return bloqueado;
        }

        public bool BloaquearLogin(int ClienteID)
        {
            var bloqueado = _loginADO.BloquearLogin(ClienteID);
            return bloqueado;
        }

        public bool DesativarAcessoCliente(int ClienteID)
        {
            var apiUser = new APIUsuarioBO();
           
            var clienteBloqueado = ado.BloquearCliente(ClienteID);
            var loginBloqueado = _loginADO.BloquearLogin(ClienteID);
            var tokenExpirado = apiUser.ExpirarToken(ClienteID);

            var desativado = clienteBloqueado && loginBloqueado && tokenExpirado;
            return desativado;
        }
    }
}