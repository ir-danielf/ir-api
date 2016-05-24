using IRAPI.Enumerator;
using IRAPI.Models;
using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IRCore.BusinessObject.Models.AccountKitModel;
using IRCore.DataAccess.ADO.Models;
using IRLib;
using Newtonsoft.Json;
using FormaPagamento = IRCore.DataAccess.Model.FormaPagamento;
using Login = IRCore.DataAccess.Model.Login;
using RestSharp;

namespace IRAPI.Controllers
{
    /// <summary>
    /// Controller responsável por autenticação e gerenciamento do usuário
    /// </summary>
    [IRAPIAuthorize(enumAPIRele.cliente, enumAPIRele.venda)]
    public class ClienteController : MasterApiController
    {
        readonly int canalInternetId = Convert.ToInt32(ConfigurationManager.AppSettings["CanalInternet"]);

        /// <summary>
        /// Método da API que autentica o Cliente com base no username e password informado. 
        /// URL de acesso: clientes/auth
        /// Método de acesso: POST
        /// </summary>
        /// <param name="clienteAuthRequest"></param>
        /// <returns>Objeto RetonoModel tendo as inforações de Sucesso,objeto Login e mensagem informando o erro, quando houver</returns>
        [Route("clientes/auth")]
        [HttpPost]
        public RetornoModel<Login, enumClienteException> PostAuth([FromBody]ClienteAuthRequestModel clienteAuthRequest)
        {
            RetornoModel<Login, enumClienteException> retorno;

            if (clienteAuthRequest == null || string.IsNullOrEmpty(clienteAuthRequest.username) || string.IsNullOrEmpty(clienteAuthRequest.password))
            {

                LogUtil.Info(string.Format("##Post.AuthCliente.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Parâmetros não informados"));

                retorno = new RetornoModel<Login, enumClienteException>
                {
                    Mensagem = "Objeto Auth não encontrado ou mal formatado",
                    Sucesso = false
                };
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));

            }

            LogUtil.Info(string.Format("##Post.AuthCliente## SESSION {0}, USER {1}", this.SessionModel.SessionID, clienteAuthRequest.username));

            if (ValidarCaptcha(Request.GetRequestContext().Principal.Identity.Name, clienteAuthRequest.respCaptcha))
            {

                using (var ado = new MasterADOBase())
                {
                    var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                    var carrinhoBO = new CarrinhoBO(ado);

                    try
                    {
                        var apiUser = new APIUsuarioBO();

                        var usuarioToken = apiUser.ConsultarToken(Request.Headers.Authorization.Parameter) ??
                                           apiUser.ConsultarToken(Request.Headers.GetValues("ClientInfo").FirstOrDefault(), Request.GetRequestContext().Principal.Identity.Name);

                        var biletoEnabled = ConfiguracaoAppUtil.GetAsBool("BILETO_ENABLED");

                        if (biletoEnabled && clienteAuthRequest.username.Contains("@"))
                        {
                            retorno = clienteBO.BiletoSignIn(clienteAuthRequest.username, clienteAuthRequest.password, this.SessionModel.UsuarioID, usuarioToken.APIUsuarioID);

                            if (!retorno.Sucesso && retorno.Tipo == enumClienteException.biletoOffline)
                            {
                                retorno = clienteBO.Logar(clienteAuthRequest.username, clienteAuthRequest.password, usuarioToken.APIUsuarioID);
                            }
                        }
                        else
                        {
                            LogUtil.Info(string.Format("##Post.AuthCliente.ByPassBileto## SESSION {0}, USER {1}, MSG: {2}", this.SessionModel.SessionID, clienteAuthRequest.username, "Login por CPF, bypass no Bileto"));
                            retorno = clienteBO.Logar(clienteAuthRequest.username, clienteAuthRequest.password, usuarioToken.APIUsuarioID);
                        }

                        if (retorno.Sucesso)
                        {
                            this.SessionModel.biletoToken = retorno.Retorno.BiletoToken;
                            this.SessionModel.biletoUuid = retorno.Retorno.BiletoUuid;

                            //Salvar endereco temp
                            SalvarEnderecoTemp(retorno.Retorno.Cliente);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##Post.AuthCliente.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                        retorno = new RetornoModel<Login, enumClienteException> { Mensagem = ex.Message, Sucesso = false };
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }

                    if (!retorno.Sucesso)
                    {
                        LogUtil.Info(string.Format("##Post.AuthCliente.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }

                    try
                    {
                        // Guarda o cliente logado na sessao
                        SessionModel.ClienteID = retorno.Retorno.ClienteID;
                        SessionModel.SiteID = retorno.Retorno.SiteID;

                        // Vincula os itens do carrinho ao cliente que logou
                        carrinhoBO.VincularCliente(SessionModel.ClienteID, SessionModel.SessionID);

                        LogUtil.Info(string.Format("##Post.AuthCliente.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

                        return ParseRetorno(retorno);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##Post.AuthCliente.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                        retorno = new RetornoModel<Login, enumClienteException>
                        {
                            Mensagem = ex.Message,
                            Sucesso = false
                        };
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                }
            }

            LogUtil.Info(string.Format("##Post.AuthCliente.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

            NewRelicIgnoreTransaction();

            retorno = new RetornoModel<Login, enumClienteException> { Mensagem = "Captcha Inválido", Sucesso = false };
            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
        }


        private bool ValidarCaptcha(string apiUser, string captcha)
        {
            try
            {
                //Em caso de não haver configuração para o API_USER retornamos true como default, pois para esses casos a validação de captcha não é obrigatório
                if (!ConfiguracaoAppUtil.GetAsList<string>("CaptchaObrigatorio").Any(c => c.Equals(apiUser)))
                    return true;

                string urlApi = ConfigurationManager.AppSettings["urlGoogleCaptcha"] ?? "https://www.google.com/recaptcha/api/siteverify";
                string secret = ConfigurationManager.AppSettings["secretGoogleCaptcha"] ?? "6LejohsTAAAAAMC1TuJ5BDQpv0rlkNaswoinL5aJ";

                var captchaApi = new RestClient(urlApi);
                var request = new RestRequest(Method.POST);
                request.AddParameter("secret", secret);
                request.AddParameter("response", captcha);
                var response = captchaApi.Execute(request);
                dynamic ret = SimpleJson.DeserializeObject(response.Content);

                return ret["success"];
            }
            catch (Exception ex)
            {
                //Em caso de erro na validação do captcha, retornamos true como default para não impedir a autenticação dos usuário
                LogUtil.Error(string.Format("##Post.AuthCliente.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);
                return true;
            }
        }

        /// <summary>
        /// Método da API que Remove a autenticacao do Cliente
        /// URL de acesso: clientes/auth
        /// Método de acesso: Delete
        /// </summary>
        /// <returns>Objeto RetonoModel tendo as inforações de Sucesso,objeto Login e mensagem informando o erro, quando houver</returns>
        [Route("clientes/auth")]
        [HttpDelete]
        public RetornoModel DeleteAuth()
        {
            LogUtil.Info(string.Format("##Delete.Auth## SESSION {0}", this.SessionModel.SessionID));

            using (CarrinhoBO carrinhoBO = new CarrinhoBO())
            {
                var carrinhoItens = carrinhoBO.Listar(SessionModel.SessionID, SessionModel.ClienteID, enumCarrinhoStatus.reservado);

                carrinhoBO.LimparReserva(SessionModel.SessionID, enumIngressoStatus.disponivel, carrinhoItens);

                SessionModel.ClienteID = 0;
                SessionModel.EntregaControleID = 0;
                SessionModel.ClienteEnderecoID = 0;
                SessionModel.PDVID = 0;

                return ParseRetorno(new RetornoModel()
                {
                    Sucesso = true,
                    Mensagem = "OK"
                });
            }
        }

        /// <summary>
        /// Método da API que retorna url que realiza a requisição dos dados do usuário no facebook
        /// URL de acesso: clientes/auth/facebook/uri
        /// Método de acesso: Post
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Objeto RetonoModel tendo as inforações de Sucesso, string Url e mensagem informando o erro, quando houver</returns>
        [Route("clientes/auth/facebook/uri")]
        [HttpPost]
        public RetornoModel<string, enumClienteException> GetFacebookUri([FromBody]ClienteAuthFacebookReturnUrl obj)
        {
            LogUtil.Info(string.Format("##Get.AuthFacebookURI## SESSION {0}, URL {1}", this.SessionModel.SessionID, obj.urlRet));

            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);

                var retorno = clienteBO.FacebookUrl(obj.urlRet);

                return ParseRetorno(retorno);
            }
        }

        /// <summary>
        /// Método da API que Loga a partir do Facebook
        /// URL de acesso: clientes/auth/facebook
        /// Método de acesso: Post
        /// </summary>
        /// <param name="facebookInfo"></param>
        /// <returns>Objeto RetonoModel tendo as inforações de Sucesso,objeto Login e mensagem informando o erro, quando houver</returns>
        [Route("clientes/auth/facebook")]
        [HttpPost]
        public RetornoModel<Login, enumClienteException> PostFacebookAuth([FromBody]ClienteAuthFacebookRequestModel facebookInfo)
        {
            LogUtil.Info(string.Format("##Post.AuthFacebook## SESSION {0}, TOKEN {1}, CODE {2}", this.SessionModel.SessionID, facebookInfo.facebookAccessToken, facebookInfo.facebookAccessCode));

            using (var ado = new MasterADOBase())
            {
                var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                var carrinhoBO = new CarrinhoBO(ado);

                var retornoFb = string.IsNullOrEmpty(facebookInfo.facebookAccessToken) ? clienteBO.FacebookGetUsuario(facebookInfo.facebookAccessCode, facebookInfo.urlRet) : clienteBO.FacebookGetUsuario(facebookInfo.facebookAccessToken); // Se já possuir token já realizar o get do usuário.
                if (retornoFb.Sucesso)
                {
                    RetornoModel<Login, enumClienteException> retorno;

                    var biletoEnabled = ConfiguracaoAppUtil.GetAsBool("BILETO_ENABLED");

                    if (biletoEnabled)
                    {
                        LogUtil.Info(string.Format("##Post.AuthFacebook.BiletoAtivo## SESSION {0}, FACEBOOKID {1}, MSG: {2}", this.SessionModel.SessionID, facebookInfo.facebookAccessCode, "Autenticando via facebook no Bileto."));
                        retorno = clienteBO.BiletoFacebookSignIn(retornoFb.Retorno, facebookInfo.facebookAccessToken);

                        if (!retorno.Sucesso && retorno.Tipo == enumClienteException.biletoOffline)
                        {
                            retorno = clienteBO.FacebookLogar(retornoFb.Retorno);
                        }
                    }
                    else
                    {
                        LogUtil.Info(string.Format("##Post.AuthFacebook.ByPassBileto## SESSION {0}, FACEBOOKID {1}, MSG: {2}", this.SessionModel.SessionID, facebookInfo.facebookAccessCode, "Bileto inativo, logando local."));
                        retorno = clienteBO.FacebookLogar(retornoFb.Retorno);
                    }

                    if (retorno.Sucesso)
                    {
                        //Salvar endereco temp
                        SalvarEnderecoTemp(retorno.Retorno.Cliente);

                        // Guarda o cliente logado na sessao
                        SessionModel.ClienteID = retorno.Retorno.ClienteID;
                        SessionModel.SiteID = retorno.Retorno.SiteID;

                        // Vincula os itens do carrinho ao cliente que logou
                        carrinhoBO.VincularCliente(SessionModel.ClienteID, SessionModel.SessionID);

                        LogUtil.Info(string.Format("##Post.AuthFacebook.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

                        return ParseRetorno(retorno);
                    }

                    dynamic me = retornoFb.Retorno;
                    if (!string.IsNullOrEmpty(me.email))
                    {
                        Login loginCadastrado = clienteBO.ConsultarEmail(me.email); //Achar o cliente via email
                        if (loginCadastrado == null) //--Email não possui cadastro
                        {
                            LogUtil.Info(string.Format("##Post.AuthFacebook.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "não possui cadastro // preencha os campos"));

                            retorno.Mensagem = "Você ainda não possui cadastro, para continuar preencha os campos.";
                            retorno.Retorno = null;
                            retorno.Tipo = enumClienteException.usuarioNaoEncontrado;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)420, ParseRetorno(retorno)));
                        }

                        LogUtil.Info(string.Format("##Post.AuthFacebook.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "já possui cadastro, por favor realize o login"));

                        retorno.Mensagem = "Você já possui cadastro, por favor realize o login.";
                        retorno.Retorno = null;
                        retorno.Tipo = enumClienteException.usuarioJaCadastradoComEmail;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }

                    LogUtil.Info(string.Format("##Post.AuthFacebook.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "não deu acesso ao email // não possui email cadastrado no facebook"));

                    retorno.Mensagem = "Você não deu acesso ao email ou não possui email cadastrado no facebook";
                    retorno.Retorno = null;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)420, ParseRetorno(retorno)));
                }
                else
                {
                    LogUtil.Info(string.Format("##Post.AuthFacebook.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Erro ao tentar acesso ao facebook"));

                    var retorno = new RetornoModel<Login, enumClienteException>
                    {
                        Mensagem = "Erro ao tentar acesso ao facebook.",
                        Tipo = enumClienteException.facebookConection
                    };
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized, ParseRetorno(retorno)));
                }
            }
        }

        /// <summary>
        /// Login através do Account Kit
        /// </summary>
        /// <returns>Objeto RetonoModel com as informações de sucesso, objeto login e mensagem</returns>
        [Route("clientes/auth/accountkit")]
        [HttpPost]
        public RetornoModel<Login, enumClienteException> PostAccountKitAuth([FromBody]ClienteAuthAccountKitRequestModel authAccountKit)
        {
            LogUtil.Info(string.Format("##Post.AuthAccountKit## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<Login, enumClienteException> retorno;

            if (!string.IsNullOrEmpty(authAccountKit.accessToken) || !string.IsNullOrEmpty(authAccountKit.code))
            {
                using (var ado = new MasterADOBase())
                {
                    var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                    var carrinhoBO = new CarrinhoBO(ado);

                    var chaveCanaisValidacaoCSRF = ConfigurationManager.AppSettings["accountKitCanaisAntiCSRF"] ?? string.Empty;
                    var canaisValidacaoCSRF = chaveCanaisValidacaoCSRF.Split(',').Select(Int32.Parse).ToList();

                    if (!canaisValidacaoCSRF.Contains(SessionModel.CanalID) || (!string.IsNullOrEmpty(SessionModel.Guid) && SessionModel.Guid.Equals(authAccountKit.guid)))
                    {
                        var accessToken = authAccountKit.accessToken;

                        if (!string.IsNullOrEmpty(authAccountKit.code) && string.IsNullOrEmpty(accessToken))
                        {
                            var respostaAccessTokenAccountKit = clienteBO.GetAccessTokenAccountKit(authAccountKit.code);

                            if (respostaAccessTokenAccountKit.Sucesso)
                            {
                                accessToken = respostaAccessTokenAccountKit.Retorno.accessToken;
                            }
                        }

                        var respostaUsuarioAccountKit = clienteBO.GetUsuarioAccountKit(accessToken);

                        if (respostaUsuarioAccountKit.Sucesso)
                        {
                            SessionModel.Guid = string.Empty;

                            retorno = clienteBO.LogarAccountKit(respostaUsuarioAccountKit.Retorno);

                            if (retorno.Sucesso)
                            {
                                var login = retorno.Retorno;

                                login.AccountKitAccessToken = accessToken;

                                var cliente = retorno.Retorno.Cliente;

                                LogUtil.Info(string.Format("##Post.AuthAccountKit.SalvarEnderecoTemp## SESSION {0}, TOKEN {1}, CLIENTE ID {2}", this.SessionModel.SessionID, authAccountKit.accessToken, login.ClienteID));

                                // Salva o endereco temporário
                                SalvarEnderecoTemp(cliente);

                                // Salva o cliente logado na sessão
                                SessionModel.ClienteID = login.ClienteID;
                                SessionModel.SiteID = login.SiteID;

                                LogUtil.Info(string.Format("##Post.AuthAccountKit.VincularCliente## SESSION {0}, TOKEN {1}, CLIENTE ID {2}", this.SessionModel.SessionID, authAccountKit.accessToken, login.ClienteID));

                                // Vincula os itens do carrinho ao cliente logado
                                carrinhoBO.VincularCliente(SessionModel.ClienteID, SessionModel.SessionID);

                                LogUtil.Info(string.Format("##Post.AuthAccountKit.SUCCESS## SESSION {0}, TOKEN {1}, CLIENTE ID {2}", this.SessionModel.SessionID, authAccountKit.accessToken, SessionModel.ClienteID));

                                return ParseRetorno(retorno);
                            }
                            else
                            {
                                LogUtil.Info(string.Format("##Post.AuthAccountKit.LogarAccountKit## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "O cliente não possui cadastro"));

                                var usuarioAccountKit = respostaUsuarioAccountKit.Retorno;

                                string ddd = string.Empty, celular = string.Empty;

                                if (usuarioAccountKit.phone.nationalNumber.Length > 2)
                                {
                                    ddd = usuarioAccountKit.phone.nationalNumber.Substring(0, 2);
                                    celular = usuarioAccountKit.phone.nationalNumber.Substring(2);
                                }

                                retorno.Mensagem = "Você ainda não possui cadastro";
                                retorno.Retorno = new Login()
                                {
                                    AccountKitAccessToken = accessToken,
                                    Cliente = new tCliente() { DDDCelular = ddd, Celular = celular, AtivoAsBool = false, RecebeEmailAsBool = false, StatusAtualAsEnum = enumClienteStatus.bloqueado },
                                    StatusAtualAsEnum = enumClienteStatus.bloqueado,
                                    AtivoAsBool = false
                                };
                                retorno.Tipo = enumClienteException.usuarioNaoEncontrado;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized, ParseRetorno(retorno)));
                            }
                        }
                    }
                }
            }

            LogUtil.Info(string.Format("##Post.AuthAccountKit## SESSION {0}, MSG {1}, ACCESS TOKEN {2}, CODE {3}, GUID {4}", this.SessionModel.SessionID, "Access token, code ou guid inválidos", authAccountKit.accessToken, authAccountKit.code, authAccountKit.guid));

            retorno = new RetornoModel<Login, enumClienteException>();
            retorno.Mensagem = "Não foi possível se conectar ao Account Kit";
            retorno.Tipo = enumClienteException.accountKitConection;
            NewRelicIgnoreTransaction();
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
        }

        /// <summary>
        /// Método da API que cadastra a partir do Facebook
        /// URL de acesso: clientes/auth/facebook
        /// Método de acesso: Post
        /// </summary>
        /// <param name="facebookInfo"></param>
        /// <returns>Objeto RetonoModel com as informações de sucesso, objeto login e mensagem</returns>
        [Route("clientes/auth/facebook/cadastro")]
        [HttpPost]
        public RetornoModel<Login, enumClienteException> PostFacebookCadastroAuth([FromBody] ClienteAuthFacebookCadasrtoRequestModel facebookInfo)
        {

            if (facebookInfo == null || string.IsNullOrEmpty(facebookInfo.email) || string.IsNullOrEmpty(facebookInfo.cpf) || !facebookInfo.email.IsEmail())
            {
                //Objeto inválido
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(new RetornoModel<Login, enumClienteException>
                {
                    Sucesso = false,
                    Tipo = enumClienteException.emailInvalido,
                    Mensagem = "E-mail ou CPF inválido."
                })));
            }

            using (var ado = new MasterADOBase())
            {
                var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);

                var retornoFb = string.IsNullOrEmpty(facebookInfo.facebookAccessToken)
                    ? clienteBO.FacebookGetUsuario(facebookInfo.facebookAccessCode, facebookInfo.urlRet)
                    : clienteBO.FacebookGetUsuario(facebookInfo.facebookAccessToken);

                // Se já possuir token já realizar o get do usuário.
                if (retornoFb.Sucesso)
                {
                    RetornoModel<Login, enumClienteException> retorno;

                    Login login = clienteBO.ConsultarFacebook(retornoFb.Retorno.id, false);
                    if (login != null)
                    {
                        //Cliente já existe com esse facebookuserid
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(new RetornoModel<Login, enumClienteException> { Sucesso = false, Tipo = enumClienteException.usuarioJaCadastradoComFacebookUserID, Mensagem = "Já existe uma conta cadastrada com esse facebook." })));
                    }

                    var biletoEnabled = ConfiguracaoAppUtil.GetAsBool("BILETO_ENABLED");

                    facebookInfo.nome = facebookInfo.nome ?? retornoFb.Retorno.name;
                    login = (Login)facebookInfo;
                    login.Cliente.SiteID = login.SiteID = clienteBO.ado.SiteId;
                    login.FaceBookUserID = retornoFb.Retorno.id;
                    login.FaceBookUserToken = facebookInfo.facebookAccessToken;

                    if (biletoEnabled)
                    {
                        LogUtil.Info(string.Format("##Post.CadastroFacebook.BiletoAtivo## SESSION {0}, EMAIL {1}, MSG: {2}", this.SessionModel.SessionID, login.Email, "Cadastrando no Bileto"));
                        retorno = clienteBO.BiletoFacebookSignUp(login, SessionModel.UsuarioID);

                        if (!retorno.Sucesso && retorno.Tipo == enumClienteException.biletoOffline)
                        {
                            retorno = clienteBO.Cadastrar(login, SessionModel.UsuarioID);
                        }
                    }
                    else
                    {
                        LogUtil.Info(string.Format("##Post.CadastroFacebook.ByPassBileto## SESSION {0}, EMAIL {1}, MSG: {2}", this.SessionModel.SessionID, login.Email, "Bileto inativo, cadastrando local"));
                        retorno = clienteBO.Cadastrar(login, SessionModel.UsuarioID);
                    }

                    if (retorno.Sucesso)
                    {
                        //Salvar endereco temp
                        SalvarEnderecoTemp(retorno.Retorno.Cliente, false);

                        // Guarda o cliente logado na sessao
                        SessionModel.ClienteID = retorno.Retorno.ClienteID;
                        SessionModel.SiteID = retorno.Retorno.SiteID;

                        // Vincula os itens do carrinho ao cliente que logou
                        var carrinhoBO = new CarrinhoBO(ado);
                        carrinhoBO.VincularCliente(SessionModel.ClienteID, SessionModel.SessionID);

                        return ParseRetorno(retorno);
                    }

                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(new RetornoModel<Login, enumClienteException>
                {
                    Sucesso = false,
                    Tipo = enumClienteException.facebookConection,
                    Mensagem = "Não foi possivel cadastrar, tente novamente."
                })));
            }
        }

        /// <summary>
        /// Cadastro através do Account Kit
        /// </summary>
        /// <param name="cadastroAccountKit"></param>
        /// <returns>Objeto RetonoModel com as informações de sucesso, objeto login e mensagem</returns>
        [Route("clientes/cadastro/accountkit")]
        [HttpPost]
        public RetornoModel<Login, enumClienteException> PostAccountKitCadastro([FromBody]ClienteCadastroAccountKitRequestModel cadastroAccountKit)
        {
            LogUtil.Info(string.Format("##Post.CadastroAccountKit## SESSION {0}", this.SessionModel.SessionID));

            if (string.IsNullOrEmpty(cadastroAccountKit.accessToken) || string.IsNullOrEmpty(cadastroAccountKit.nome) || string.IsNullOrEmpty(cadastroAccountKit.email) || string.IsNullOrEmpty(cadastroAccountKit.cpf))
            {
                LogUtil.Debug(string.Format("##Post.CadastroAccountKit## SESSION {0}, TOKEN {1}, NOME {2}, E-MAIL {3}, CPF {4}, MSG {5}", this.SessionModel.SessionID, cadastroAccountKit.accessToken, cadastroAccountKit.nome, cadastroAccountKit.email, cadastroAccountKit.cpf, "Access token, Nome, E-mail ou CPF inválidos"));

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Access token, Nome, E-mail ou CPF inválidos" })));
            }

            using (var ado = new MasterADOBase())
            {
                var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                var carrinhoBO = new CarrinhoBO(ado);

                // Recupera os dados do usuário do Account Kit
                var respostaAccountKit = clienteBO.GetUsuarioAccountKit(cadastroAccountKit.accessToken);

                if (!respostaAccountKit.Sucesso)
                {
                    LogUtil.Debug(string.Format("##Post.CadastroAccountKit## SESSION {0} TOKEN {1}, MSG {2}", this.SessionModel.SessionID, cadastroAccountKit.accessToken, respostaAccountKit.Mensagem));

                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(new RetornoModel<Login, enumClienteException>() { Sucesso = false, Tipo = respostaAccountKit.Tipo, Mensagem = respostaAccountKit.Mensagem })));
                }

                var usuarioAccountKit = respostaAccountKit.Retorno;

                var login = clienteBO.ConsultarAccountKit(usuarioAccountKit.id, false);

                if (login != null)
                {
                    LogUtil.Debug(string.Format("##Post.CadastroAccountKit## SESSION {0}, TOKEN {1}, ACCOUNT KIT ID {2}, MSG {3}", this.SessionModel.SessionID, cadastroAccountKit.accessToken, usuarioAccountKit.id, "Cliente já cadastrado com Account Kit"));

                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(new RetornoModel<Login, enumClienteException>() { Sucesso = false, Tipo = enumClienteException.usuarioCadastradoAccountKit, Mensagem = "Cliente já cadastrado com Account Kit" })));
                }

                LogUtil.Debug(string.Format("##Post.CadastroAccountKit## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Cliente não encontrado. Criando novo cadastro"));

                // Faz um novo cadastro com os dados do Account Kit

                string ddd = string.Empty, telefone = string.Empty;

                if (usuarioAccountKit.phone.nationalNumber.Length > 2)
                {
                    ddd = usuarioAccountKit.phone.nationalNumber.Substring(0, 2);
                    telefone = usuarioAccountKit.phone.nationalNumber.Substring(2);
                }

                login = new Login
                {
                    AccountKitId = usuarioAccountKit.id,
                    Email = cadastroAccountKit.email,
                    CPF = cadastroAccountKit.cpf,
                    Senha = string.Empty,
                    SiteID = clienteBO.ado.SiteId,
                    Cliente = new tCliente
                    {
                        Nome = cadastroAccountKit.nome,
                        Email = cadastroAccountKit.email,
                        CPF = cadastroAccountKit.cpf,
                        DDDTelefone = cadastroAccountKit.dddTelefone ?? ddd,
                        Telefone = cadastroAccountKit.telefone ?? telefone,
                        DDDCelular = ddd,
                        Celular = telefone,
                        SiteID = clienteBO.ado.SiteId,
                        RecebeEmailAsBool = cadastroAccountKit.recebeEmail
                    }
                };

                var respostaCadastro = clienteBO.Cadastrar(login);

                if (!respostaCadastro.Sucesso)
                {
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(respostaCadastro)));
                }

                LogUtil.Debug(string.Format("##Post.AuthAccountKit.SalvarEnderecoTemp## SESSION {0}, TOKEN {1}, CLIENTE ID {2}", this.SessionModel.SessionID, cadastroAccountKit.accessToken, login.ClienteID));

                // Salva o endereco temporário
                SalvarEnderecoTemp(respostaCadastro.Retorno.Cliente, false);

                // Salva o cliente logado na sessão
                SessionModel.ClienteID = respostaCadastro.Retorno.ClienteID;
                SessionModel.SiteID = respostaCadastro.Retorno.SiteID;

                LogUtil.Debug(string.Format("##Post.AuthAccountKit.VincularCliente## SESSION {0}, TOKEN {1}, CLIENTE ID {2}", this.SessionModel.SessionID, cadastroAccountKit.accessToken, SessionModel.ClienteID));

                // Vincula os itens do carrinho ao cliente logado
                carrinhoBO.VincularCliente(SessionModel.ClienteID, SessionModel.SessionID);

                LogUtil.Info(string.Format("##Post.CadastroAccountKit.SUCCESS## SESSION {0}, TOKEN {1}, , CLIENTE ID {2}", this.SessionModel.SessionID, cadastroAccountKit.accessToken, SessionModel.ClienteID));

                return ParseRetorno(respostaCadastro);
            }
        }

        /// <summary>
        /// Método da API que Vincula usuario a partir do Facebook
        /// URL de acesso: clientes/auth/facebook/vinculo
        /// Método de acesso: Post
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Objeto RetonoModel tendo as inforações de Sucesso,objeto Login e mensagem informando o erro, quando houver</returns>
        [Route("clientes/auth/facebook/vinculo")]
        [HttpPost]
        public RetornoModel<Login, enumClienteException> PostFacebookVinculo([FromBody]ClienteAuthFacebookVinculoRequestModel rq)
        {
            RetornoModel<Login, enumClienteException> retorno = null;
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarrinhoBO carrinhoBO = new CarrinhoBO(ado);
                retorno = clienteBO.FacebookVincular(rq.loginTemp, rq.password, rq.tipoException);

                if (!retorno.Sucesso)
                {
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized, ParseRetorno(retorno)));
                }
                else
                {
                    // Guarda o cliente logado na sessao
                    SessionModel.ClienteID = retorno.Retorno.ClienteID;
                    SessionModel.SiteID = retorno.Retorno.SiteID;

                    // Vincula os itens do carrinho ao cliente que logou
                    carrinhoBO.VincularCliente(SessionModel.ClienteID, SessionModel.SessionID);
                }
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que carrega o usuário vinculado a um token de alterar senha
        /// URL de acesso: clientes/senha/{token}
        /// Método de acesso: Post
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Objeto RetonoModel tendo as inforações de Sucesso,objeto Login e mensagem informando o erro, quando houver</returns>
        [Route("clientes/senha/{token}")]
        [HttpGet]
        public RetornoModel<Login> GetSenhaToken(string token)
        {
            RetornoModel<Login> retorno;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                    retorno = clienteBO.ConsultarToken(token, false);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                retorno = new RetornoModel<Login>();
                retorno.Mensagem = ex.Message;
                retorno.Sucesso = false;
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (!retorno.Sucesso)
            {
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Altera senha baseada no token
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="trocaSenha">Nova senha</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("clientes/senha/{token}")]
        [HttpPut]
        public RetornoModel<Login, enumClienteException> PutSenha(string token, [FromBody]ClienteSenhaModel trocaSenha)
        {
            RetornoModel<Login, enumClienteException> retorno;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                    retorno = clienteBO.MudarSenha(token, trocaSenha.senha);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                retorno = new RetornoModel<Login, enumClienteException> { Mensagem = ex.Message, Sucesso = false };
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
            }

            if (!retorno.Sucesso)
            {
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que efetua um cadastro de um cliente com base nos dados informados.
        /// URL de acesso: clientesV2 (depois alterar para clientes)
        /// Método de Acesso: POST
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Objeto RetornoModel tendo as informações de Sucesso,objeto Login e mensagem informando o erro,quando houver</returns>
        [Route("clientes")]
        [HttpPost]
        public RetornoModel<Login, enumClienteException> Post([FromBody]Login login)
        {
            var retorno = new RetornoModel<Login, enumClienteException>();

            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Senha) || string.IsNullOrEmpty(login.CPF))
            {
                LogUtil.Info(string.Format("##Post.CadastroCliente.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Objeto Login não instânciado"));

                NewRelicIgnoreTransaction();
                retorno.Mensagem = "Não foi possivel efetuar o cadastro, tente novamente.";
                retorno.Sucesso = false;
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Post.CadastroCliente## SESSION {0}, LOGIN {1}", this.SessionModel.SessionID, JsonConvert.SerializeObject(login)));

            if (ValidarCaptcha(Request.GetRequestContext().Principal.Identity.Name, login.respCaptcha))
            {

                using (var ado = new MasterADOBase())
                {
                    var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);

                    try
                    {

                        var loginByCpf = clienteBO.ConsultarCPF(login.CPF);
                        if (loginByCpf != null)
                        {
                            LogUtil.Info(string.Format("##Post.CadastroCliente.ERROR## CLIENTEID {0}, EMAIL {1}, CPF {2}, MSG {3}", loginByCpf.ClienteID, loginByCpf.Email, loginByCpf.CPF, "Já existe um usuário cadastrado com este CPF. Por favor clique em esqueci minha senha ou realize o login."));

                            return new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Já existe um usuário cadastrado com este CPF. Por favor clique em esqueci minha senha ou realize o login.", Retorno = login, Tipo = enumClienteException.usuarioJaCadastradoComCPF };
                        }

                        var apiUser = new APIUsuarioBO();

                        var usuarioToken = apiUser.ConsultarToken(Request.Headers.Authorization.Parameter) ??
                                           apiUser.ConsultarToken(Request.Headers.GetValues("ClientInfo").FirstOrDefault(), Request.GetRequestContext().Principal.Identity.Name);

                        //Ajustar APIUsusario no login
                        login.CreatedAPIUsuarioId = usuarioToken.APIUsuarioID;
                        login.LoginAPIUsuarioId = usuarioToken.APIUsuarioID;

                        var biletoEnabled = ConfiguracaoAppUtil.GetAsBool("BILETO_ENABLED");

                        if (biletoEnabled)
                        {
                            LogUtil.Info(string.Format("##Post.CadastroCliente.BiletoAtivo## SESSION {0}, EMAIL {1}, MSG`: {2}", this.SessionModel.SessionID, login.Email, "Cadastrando no Bileto"));
                            retorno = clienteBO.BiletoSignUp(login, this.SessionModel.UsuarioID);

                            if (!retorno.Sucesso && retorno.Tipo == enumClienteException.biletoOffline)
                            {
                                retorno = clienteBO.Cadastrar(login, SessionModel.UsuarioID);
                            }
                        }
                        else
                        {
                            LogUtil.Info(string.Format("##Post.CadastroCliente.ByPassBileto## SESSION {0}, EMAIL {1}, MSG`: {2}", this.SessionModel.SessionID, login.Email, "Bileto inativo, cadastrando local"));
                            retorno = clienteBO.Cadastrar(login, SessionModel.UsuarioID);
                        }

                        if (retorno.Sucesso)
                        {
                            //Salvar endereco temp
                            SalvarEnderecoTemp(retorno.Retorno.Cliente, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##Post.CadastroCliente.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                        //LogUtil.Error(ex);
                        retorno = new RetornoModel<Login, enumClienteException> { Mensagem = ex.Message, Sucesso = false };
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }

                    if (!retorno.Sucesso)
                    {
                        LogUtil.Info(string.Format("##Post.CadastroCliente.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }

                    // Guarda o cliente logado na sessao
                    SessionModel.ClienteID = retorno.Retorno.ClienteID;
                    SessionModel.SiteID = retorno.Retorno.SiteID;

                    // Vincula os itens do carrinho ao cliente que logou
                    var carrinhoBO = new CarrinhoBO(ado);
                    carrinhoBO.VincularCliente(SessionModel.ClienteID, SessionModel.SessionID);

                    LogUtil.Info(string.Format("##Post.CadastroCliente.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

                    return ParseRetorno(retorno);
                }
            }


            LogUtil.Info(string.Format("##Post.AuthCliente.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Sucesso = false"));

            NewRelicIgnoreTransaction();

            retorno = new RetornoModel<Login, enumClienteException> { Mensagem = "Captcha Inválido", Sucesso = false };
            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
        }

        /// <summary>
        /// Método que atualiza os dados cadastrais do usuário logado
        /// </summary>
        /// <param name="loginInfo">Informações atualizadas</param>
        /// <returns></returns>
        [Route("clientes/logado")]
        [HttpPut]
        public RetornoModel<Login, enumClienteException> PutLogado([FromBody]Login loginInfo)
        {
            RetornoModel<Login, enumClienteException> retorno;
            using (var ado = new MasterADOBase())
            {
                var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);

                CarregarCliente(clienteBO);

                if (loginInfo != null)
                {
                    LogUtil.Info(string.Format("##Put.AtualizaUsuario## SESSION {0}, EMAIL {1}, CPF {2}", this.SessionModel.SessionID, loginInfo.Email, loginInfo.CPF));

                    if (SessionModel.Login == null)
                    {
                        LogUtil.Error(string.Format("##Put.AtualizaUsuario.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Não existe um cliente logado"));

                        retorno = new RetornoModel<Login, enumClienteException>() { Sucesso = false, Mensagem = "Não existe um cliente logado", Retorno = null, Tipo = enumClienteException.usuarioNaoEncontrado };
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }

                    Login login = SessionModel.Login;
                    login.Email = !string.IsNullOrEmpty(loginInfo.Email) ? loginInfo.Email : login.Email;
                    login.FaceBookUserID = login.FaceBookUserID;
                    login.FaceBookUserToken = login.FaceBookUserToken;
                    login.FaceBookUserInfos = login.FaceBookUserInfos;
                    login.CPF = !string.IsNullOrEmpty(loginInfo.CPF) ? loginInfo.CPF.Replace(new[] { ".", "-" }, "") : login.CPF.Replace(new[] { ".", "-" }, "");
                    login.SiteID = login.SiteID;
                    login.SyncStatus = (char)SyncStatus.EXPORT_LATER;
                    login.BiletoUuid = this.SessionModel.Login.BiletoUuid;

                    if (loginInfo.Cliente != null)
                    {
                        login.Cliente.Senha = !string.IsNullOrEmpty(loginInfo.Cliente.Senha) ? loginInfo.Cliente.Senha : login.Cliente.Senha;
                        login.Cliente.RG = !string.IsNullOrEmpty(loginInfo.Cliente.RG) ? loginInfo.Cliente.RG : login.Cliente.RG;
                        login.Cliente.ContatoTipoID = loginInfo.Cliente.ContatoTipoID == null ? loginInfo.Cliente.ContatoTipoID : login.Cliente.ContatoTipoID;
                        login.Cliente.Pais = !string.IsNullOrEmpty(loginInfo.Cliente.Pais) ? loginInfo.Cliente.Pais : login.Cliente.Pais;
                        login.Cliente.Nome = !string.IsNullOrEmpty(loginInfo.Cliente.Nome) ? loginInfo.Cliente.Nome : login.Cliente.Nome;
                        login.Cliente.DDDCelular = !string.IsNullOrEmpty(loginInfo.Cliente.DDDCelular) ? loginInfo.Cliente.DDDCelular : login.Cliente.DDDCelular;
                        login.Cliente.Celular = !string.IsNullOrEmpty(loginInfo.Cliente.Celular) ? loginInfo.Cliente.Celular : login.Cliente.Celular;
                        login.Cliente.DDDTelefone = !string.IsNullOrEmpty(loginInfo.Cliente.DDDTelefone) ? loginInfo.Cliente.DDDTelefone : login.Cliente.DDDTelefone;
                        login.Cliente.Telefone = !string.IsNullOrEmpty(loginInfo.Cliente.Telefone) ? loginInfo.Cliente.Telefone : login.Cliente.Telefone;
                        login.Cliente.DDDTelefoneComercial = !string.IsNullOrEmpty(loginInfo.Cliente.DDDTelefoneComercial) ? loginInfo.Cliente.DDDTelefoneComercial : login.Cliente.DDDTelefoneComercial;
                        login.Cliente.TelefoneComercial = !string.IsNullOrEmpty(loginInfo.Cliente.DDDTelefoneComercial) ? loginInfo.Cliente.TelefoneComercial : login.Cliente.TelefoneComercial;
                        login.Cliente.RecebeEmailAsBool = loginInfo.Cliente.RecebeEmailAsBool;
                        login.Cliente.DataNascimento = !string.IsNullOrEmpty(loginInfo.Cliente.DataNascimento) ? loginInfo.Cliente.DataNascimento : login.Cliente.DataNascimento;
                        login.Cliente.Sexo = !string.IsNullOrEmpty(loginInfo.Cliente.Sexo) ? loginInfo.Cliente.Sexo : login.Cliente.Sexo;
                        login.Cliente.CidadeCliente = !string.IsNullOrEmpty(loginInfo.Cliente.CidadeCliente) ? loginInfo.Cliente.CidadeCliente : login.Cliente.CidadeCliente;
                        login.Cliente.EstadoCliente = !string.IsNullOrEmpty(loginInfo.Cliente.EstadoCliente) ? loginInfo.Cliente.EstadoCliente : login.Cliente.EstadoCliente;
                        login.Cliente.EnderecoCliente = !string.IsNullOrEmpty(loginInfo.Cliente.EnderecoCliente) ? loginInfo.Cliente.EnderecoCliente : login.Cliente.EnderecoCliente;
                        login.Cliente.NumeroCliente = !string.IsNullOrEmpty(loginInfo.Cliente.NumeroCliente) ? loginInfo.Cliente.NumeroCliente : login.Cliente.NumeroCliente;
                        login.Cliente.SiteID = loginInfo.Cliente.SiteID != 0 ? loginInfo.Cliente.SiteID : login.Cliente.SiteID;

                        if (!string.IsNullOrEmpty(loginInfo.Cliente.CEPCliente))
                            login.Cliente.CEPCliente = loginInfo.Cliente.CEPCliente.Replace("-", "");
                        login.Cliente.ComplementoCliente = !string.IsNullOrEmpty(loginInfo.Cliente.ComplementoCliente) ? loginInfo.Cliente.ComplementoCliente : login.Cliente.ComplementoCliente;
                        login.Cliente.BairroCliente = !string.IsNullOrEmpty(loginInfo.Cliente.BairroCliente) ? loginInfo.Cliente.BairroCliente : login.Cliente.BairroCliente;
                    }

                    bool salvarConcluido;
                    try
                    {
                        salvarConcluido = clienteBO.Salvar(login, SessionModel.UsuarioID);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(string.Format("##Put.AtualizaUsuario.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                        //LogUtil.Error(ex);
                        retorno = new RetornoModel<Login, enumClienteException> { Mensagem = ex.Message, Sucesso = false };
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }

                    if (salvarConcluido)
                    {
                        LogUtil.Info(string.Format("##Put.AtualizaUsuario.SUCCESS## SESSION {0}", this.SessionModel.SessionID));

                        retorno = new RetornoModel<Login, enumClienteException>
                        {
                            Mensagem = "OK",
                            Retorno = login,
                            Sucesso = true
                        };
                    }
                    else
                    {
                        LogUtil.Info(string.Format("##Put.AtualizaUsuario.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Não foi possível completar a solicitação"));

                        retorno = new RetornoModel<Login, enumClienteException>
                        {
                            Mensagem = "Não foi possível completar a solicitação",
                            Sucesso = false
                        };
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                    retorno.Sucesso = salvarConcluido;
                }
                else
                {
                    LogUtil.Info(string.Format("##Put.AtualizaUsuario.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Parâmetros não informados"));

                    retorno = new RetornoModel<Login, enumClienteException>
                    {
                        Mensagem = "Objeto Login não encontrado",
                        Sucesso = false
                    };
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                }
            }
            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Método da API que retorna os dados de Login do cliente logado.
        /// URL de acesso: clientes/logado
        /// Método de Acesso: GET
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Objeto RetornoModel tendo as informações de Sucesso,objeto Login e mensagem informando o erro,quando houver</returns>
        [IRAPIAuthorize(enumAPIRele.cliente, enumAPIRele.venda, enumAPIRele.osesp)]
        [Route("clientes/logado")]
        [HttpGet]
        public RetornoModel<Login> GetLogado()
        {
            LogUtil.Info(string.Format("##Get.UsuarioLogado## SESSION {0}", this.SessionModel.SessionID));

            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                RetornoModel<Login> retorno = new RetornoModel<Login>();
                Login login = SessionModel.Login;

                if (login != null)
                {
                    #region Cliente possui endereço completo.
                    login.Cliente.PossuiEndereco = !(string.IsNullOrEmpty(login.Cliente.EnderecoCliente) ||
                        string.IsNullOrEmpty(login.Cliente.BairroCliente) ||
                        string.IsNullOrEmpty(login.Cliente.CEPCliente) ||
                        string.IsNullOrEmpty(login.Cliente.CidadeCliente) ||
                        string.IsNullOrEmpty(login.Cliente.ComplementoCliente) ||
                        string.IsNullOrEmpty(login.Cliente.NumeroCliente) ||
                        string.IsNullOrEmpty(login.Cliente.DDDCelular) ||
                        string.IsNullOrEmpty(login.Cliente.Celular) ||
                        string.IsNullOrEmpty(login.Cliente.EstadoCliente)
                    );
                    #endregion

                    retorno.Sucesso = true;
                    retorno.Retorno = login;
                    retorno.Mensagem = "OK";
                    return ParseRetorno(retorno);
                }

                retorno.Mensagem = "Não encontrado um cliente com este id";
                retorno.Sucesso = false;
                retorno.Retorno = null;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
        }

        /// <summary>
        /// Método da API que retorna os enderecos do usuário logado.
        /// URL de acesso: clientes/{id}
        /// Método de Acesso: GET
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Objeto RetornoModel tendo as informações de Sucesso,objeto Login e mensagem informando o erro,quando houver</returns>
        [Route("clientes/logado/enderecos")]
        [HttpGet]
        public RetornoModel<List<tClienteEndereco>> GetLogadoEnderecos()
        {
            LogUtil.Info(string.Format("##Get.EndereçosUsuarioLogado## SESSION {0}", this.SessionModel.SessionID));

            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO, true);
            }
            RetornoModel<List<tClienteEndereco>> retorno = new RetornoModel<List<tClienteEndereco>>();
            Login login = SessionModel.Login;
            if (login != null)
            {
                if (login.Cliente.EnderecoList.Count == 0)
                {
                    retorno.Mensagem = "Nehum endereço encontrado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                }
                else
                {
                    retorno.Mensagem = "OK";
                    retorno.Sucesso = true;
                    retorno.Retorno = login.Cliente.EnderecoList;
                }
            }
            else
            {
                retorno.Mensagem = "Não encontrado um cliente logado";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }

        [Route("clientes/logado/enderecos")]
        [HttpPost]
        public RetornoModel<tClienteEndereco> PostEndereco([FromBody]tClienteEndereco obj)
        {
            LogUtil.Info(string.Format("##Post.AddEndereçoUsuarioLogado## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<tClienteEndereco> retorno = new RetornoModel<tClienteEndereco>();
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                Login login = SessionModel.Login;

                if (login != null)
                {
                    tCliente cliente = login.Cliente;
                    if (cliente != null)
                    {
                        if (obj != null)
                        {
                            if (!string.IsNullOrEmpty(obj.Estado) && obj.Estado.Length > 2)
                            {
                                retorno.Mensagem = "Estado deve possuir apenas dois carcteres";
                                retorno.Sucesso = false;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                            }

                            obj.ClienteID = cliente.ID;
                            obj.CPF = cliente.CPF;
                            obj.RG = cliente.RG;
                            obj.Nome = cliente.Nome;
                            obj.CEP = obj.CEP.Replace(new string[] { "-", "." }, "");
                            try
                            {
                                retorno = clienteBO.SalvarEndereco(obj, cliente, SessionModel.UsuarioID);
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Error(ex);
                                retorno.Mensagem = ex.Message;
                                retorno.Sucesso = false;
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                            }
                            if (!retorno.Sucesso)
                            {
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                            }
                        }
                        else
                        {
                            retorno.Mensagem = "Objeto endereço não encontrado";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Não encontrado um cliente com este id";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Não encontrado um cliente com este id";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            return ParseRetorno(retorno);
        }

        [Route("clientes/logado/enderecos/{enderecoID}")]
        [HttpPut]
        public RetornoModel<tClienteEndereco> PutEndereco(int enderecoID, [FromBody]tClienteEndereco obj)
        {
            LogUtil.Info(string.Format("##Put.EndereçoUsuarioLogado## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<tClienteEndereco> retorno = new RetornoModel<tClienteEndereco>();
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                Login login = SessionModel.Login;
                tCliente cliente = null;

                if (login != null)
                {
                    cliente = login.Cliente;
                    if (cliente != null)
                    {
                        tClienteEndereco clienteEndereco = null;
                        try
                        {
                            clienteEndereco = clienteBO.ConsultarEndereco(enderecoID);
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error(ex);
                            retorno.Mensagem = ex.Message;
                            retorno.Sucesso = false;
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                        }
                        if (clienteEndereco == null || clienteEndereco.ClienteID != SessionModel.ClienteID)
                        {
                            retorno.Retorno = clienteEndereco;
                            retorno.Sucesso = false;
                            retorno.Mensagem = "Não encontrado um endereço com este ID vinculado ao cliente";
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                        else
                        {
                            if (obj != null)
                            {
                                if (!string.IsNullOrEmpty(obj.Estado) && obj.Estado.Length > 2)
                                {
                                    retorno.Mensagem = "Estado deve possuir apenas dois carcteres";
                                    retorno.Sucesso = false;
                                    NewRelicIgnoreTransaction();
                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                                }
                                clienteEndereco.CEP = obj.CEP.Replace(new string[] { "-", "." }, "");
                                clienteEndereco.Endereco = obj.Endereco;
                                clienteEndereco.Numero = obj.Numero;
                                clienteEndereco.Cidade = obj.Cidade;
                                clienteEndereco.Estado = obj.Estado;
                                clienteEndereco.Complemento = obj.Complemento;
                                clienteEndereco.Bairro = obj.Bairro;
                                clienteEndereco.Nome = cliente.Nome;
                                clienteEndereco.CPF = cliente.CPF;
                                clienteEndereco.RG = cliente.RG;
                                clienteEndereco.ClienteID = cliente.ID;
                                clienteEndereco.StatusConsulta = obj.StatusConsulta;
                                clienteEndereco.EnderecoTipoID = obj.EnderecoTipoID;
                                clienteEndereco.EnderecoPrincipal = obj.EnderecoPrincipal;
                                clienteEndereco.ID = enderecoID;
                                try
                                {
                                    retorno = clienteBO.SalvarEndereco(clienteEndereco, cliente, SessionModel.UsuarioID);
                                }
                                catch (Exception ex)
                                {
                                    LogUtil.Error(ex);
                                    retorno.Mensagem = ex.Message;
                                    retorno.Sucesso = false;
                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                                }
                            }
                            else
                            {
                                retorno.Mensagem = "Objeto endereço não encontrado";
                                retorno.Sucesso = false;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                            }
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Nenhum usuário logado";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            return ParseRetorno(retorno);
        }

        [Route("clientes/logado/enderecos/{enderecoID}")]
        [HttpDelete]
        public RetornoModel DeleteEndereco(int enderecoID)
        {
            LogUtil.Info(string.Format("##Delete.EndereçoUsuarioLogado## SESSION {0}, END {1}", this.SessionModel.SessionID, enderecoID));

            RetornoModel retorno = new RetornoModel();
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                Login login = SessionModel.Login;
                tCliente cliente = null;
                if (login != null)
                {
                    cliente = login.Cliente;
                    if (cliente != null)
                    {
                        tClienteEndereco clienteEndereco = null;
                        try
                        {
                            clienteEndereco = clienteBO.ConsultarEndereco(enderecoID);
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error(ex);
                            retorno.Mensagem = ex.Message;
                            retorno.Sucesso = false;
                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                        }
                        if (clienteEndereco == null || clienteEndereco.ClienteID != SessionModel.ClienteID)
                        {
                            retorno.Sucesso = false;
                            retorno.Mensagem = "Não encontrado um endereço com este ID vinculado ao cliente";
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                        else
                        {
                            try
                            {
                                clienteBO.Remover(clienteEndereco, SessionModel.UsuarioID);
                                retorno.Mensagem = "OK";
                                retorno.Sucesso = true;
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Error(ex);
                                retorno.Sucesso = false;
                                retorno.Mensagem = ex.Message;
                                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                            }
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Nenhum usuário logado";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            return ParseRetorno(retorno);
        }

        ///// <summary>
        ///// Retorna as compras do usuário logado.
        ///// </summary>
        ///// <param name="pg"></param>
        ///// <param name="qtdpg"></param>
        ///// <returns></returns>
        [Route("clientes/logado/compras")]
        [HttpGet]
        public RetornoModel<PagedListModel<CompraModel>> GetComprasLogado(int pg = 1, int qtdpg = 1)
        {
            LogUtil.Info(string.Format("##Get.Compras## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<PagedListModel<CompraModel>> retorno = new RetornoModel<PagedListModel<CompraModel>>();
            IPagedList<tVendaBilheteria> compras = null;
            List<CompraModel> listCompra = new List<CompraModel>();
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    try
                    {
                        VendaBilheteriaBO vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        FormaPagamentoBO formaPagamentoBO = new FormaPagamentoBO(ado);

                        compras = vendaBilheteriaBO.ListarCliente(pg, qtdpg, SessionModel.Login);
                        for (int i = 0; i < compras.Count; i++)
                        {
                            var item = compras[i];
                            CompraModel compra = new CompraModel();
                            if (item.tIngresso != null && item.tIngresso.Any())
                            {
                                decimal valorTaxaConveniencia = (item.TaxaConvenienciaValorTotal ?? 0) / item.tIngresso.Count();

                                var bo = new CompraBOModel(SessionModel.GetSiteId(), ado);

                                //Cria lista de inteiros com um único ID que será passado ao método ListarInEventos, que utilizará esse ID para retornar lista de EntregaControle
                                var lista = new List<int> { item.tIngresso.FirstOrDefault().EventoID.Value };

                                var entregas = bo.entregaBO.ListarInEventos(lista);

                                /*
                                 * Caso o canal utilizado para efetuar essa chamada da API seja canal mobile e um ou mais eventos que corresponderem aos ingressos do usuário logado
                                   tiver como método de entrega a modalidade Mobile Ticket (independente do usuário ter comprado por ela), o código de barras é zerado no retorno da API.
                                 * Isso é necessário para que o app não gere o QR code de um evento que aceite eTicket, porém não aceite mTicket (robô GeradorETicket que gera código de barras
                                   para ingressos comprados via eTicket).
                                 */
                                if (new ConfigGerenciador().getMobileCanais().Contains(SessionModel.CanalID) && !entregas.Exists(e => e.EntregaID == 31))
                                {
                                    item.tIngresso.FirstOrDefault().CodigoBarra = String.Empty;
                                }

                                //Converte a lista de Ingressos em uma lista de Carrinhos
                                compra.CarrinhoItens = item.tIngresso.Select(x => x.toCarrinho(valorTaxaConveniencia)).ToList();
                            }

                            compra.ClienteEnderecoID = item.ClienteEnderecoID ?? 0;
                            compra.EntregaComEndereco = (item.ClienteEnderecoID ?? 0) > 0;
                            compra.EntregaControleID = item.EntregaControleID ?? 0;

                            //Pagamento
                            compra.Pagamentos = new List<CompraPagamentoModel>();

                            //Pega todas formas de pagamento
                            List<int> formasPagamentoIDs = item.tVendaBilheteriaFormaPagamento.Select(x => x.FormaPagamentoID ?? 0).ToList();
                            if (formasPagamentoIDs.Count > 0)
                            {
                                List<FormaPagamento> formasPagamento = formaPagamentoBO.Listar(formasPagamentoIDs);

                                //retornas as formas de pagamento desta lista.
                                foreach (var formaPagamento in item.tVendaBilheteriaFormaPagamento.Where(x => x.ValeIngressoID == null || x.ValeIngressoID == 0))
                                {
                                    CompraPagamentoModel pagamento = new CompraPagamentoModel();
                                    pagamento.Valor = formaPagamento.Valor ?? 0;
                                    pagamento.formaPagamento = formasPagamento.FirstOrDefault(x => x.IR_FormaPagamentoID == formaPagamento.FormaPagamentoID);
                                    if (formaPagamento.tCartao != null)
                                    {
                                        pagamento.NomeCartao = formaPagamento.tCartao.NomeCartao;
                                        pagamento.NumeroCartao = formaPagamento.tCartao.NroCartao;
                                    }

                                    compra.Pagamentos.Add(pagamento);
                                }
                            }

                            compra.PDVID = item.PdvID ?? 0;
                            //Totais
                            compra.Total = new CompraTotalModel();
                            compra.Total.ValorDesconto = item.tVendaBilheteriaFormaPagamento.Where(x => x.ValeIngressoID != null && x.ValeIngressoID > 0).Sum(x => x.Valor ?? 0);
                            compra.Total.ValorTaxaConveniencia = item.TaxaConvenienciaValorTotal ?? 0;
                            compra.Total.ValorTaxaEntrega = item.TaxaEntregaValor ?? 0;
                            compra.Total.ValorTotal = item.ValorTotal ?? 0;
                            compra.Total.ValorIngressos = compra.Total.ValorTotal - compra.Total.ValorTaxaEntrega - compra.Total.ValorTaxaConveniencia;
                            compra.ValorTotalSeguro = item.ValorSeguro ?? 0;

                            compra.ValeIngressos = item.tVendaBilheteriaFormaPagamento.Where(x => x.tValeIngresso != null).Select(x => x.tValeIngresso).ToList();

                            //Limpa os itens da venda bilheteria que não devem ser retornados
                            item.tVendaBilheteriaFormaPagamento = new List<tVendaBilheteriaFormaPagamento>();
                            item.tIngresso = new List<tIngresso>();

                            compra.VendaBilheteria = item;
                            listCompra.Add(compra);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
                if (compras != null)
                {
                    retorno.Retorno = new PagedListModel<CompraModel>();
                    retorno.Retorno.CopyFrom(compras);
                    retorno.Retorno.Itens = listCompra;
                    retorno.Sucesso = true;
                    retorno.Mensagem = "OK";
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.Mensagem = "Nenhuma compra realizada";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                }
            }

            return ParseRetorno(retorno);
        }

        /// <summary>
        /// Retorna as compras do usuário logado.
        /// </summary>
        /// <param name="pg">Página</param>
        /// <param name="qtdpg">Quantidade de página</param>
        /// <returns>Lista de compras</returns>
        /// <exception cref="HttpResponseException"></exception>
        [Route("clientes/logado/meus_ingressos")]
        [HttpGet]
        public RetornoModel<PagedListModel<MeuIngresso>> GetMeusIngressos(int pg = 1, int qtdpg = 1)
        {
            LogUtil.Info(string.Format("##Get.MeusIngressos## SESSION {0}", this.SessionModel.SessionID));

            var retorno = new RetornoModel<PagedListModel<MeuIngresso>>();
            IPagedList<MeuIngresso> result;

            using (var ado = new MasterADOBase())
            {
                //Get Login.
                LogUtil.Debug(string.Format("##Get.Compras_2.GetLogin## SESSION {0}", this.SessionModel.SessionID));
                var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);

                if (SessionModel.Login == null)
                {
                    LogUtil.Info(string.Format("##Get.MeusIngressos.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Nenhum usuário logado"));

                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }

                try
                {
                    //Get meusingressos.
                    LogUtil.Debug(string.Format("##Get.MeusIngressos## SESSION {0}, CLIENTE {1}", this.SessionModel.SessionID, SessionModel.Login.ClienteID));

                    var vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                    result = vendaBilheteriaBO.GetMeusIngressos(pg, qtdpg, SessionModel.Login.ClienteID, SessionModel.CanalID);
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Get.MeusIngressos.EXCEPTION## SESSION {0}, MSG {1}", this.SessionModel.SessionID, ex.Message), ex);

                    retorno.Mensagem = ex.Message;
                    retorno.Sucesso = false;
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }
            }

            //Set retorno.
            if (result != null && result.Count > 0)
            {
                LogUtil.Debug(string.Format("##Get.MeusIngressos.SUCCESS## SESSION {0}, CLIENTE {1}", this.SessionModel.SessionID, SessionModel.Login.ClienteID));

                retorno.Retorno = new PagedListModel<MeuIngresso>().CopyFrom(result);
                retorno.Retorno.Itens = result.ToList();
                retorno.Sucesso = true;
                retorno.Mensagem = "OK";
            }
            else
            {
                LogUtil.Debug(string.Format("##Get.MeusIngressos.NOTFOUND## SESSION {0}, CLIENTE {1}", this.SessionModel.SessionID, SessionModel.Login.ClienteID));

                retorno.Sucesso = false;
                retorno.Mensagem = "Nenhum ingresso encontrado.";
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }

            return ParseRetorno(retorno);
        }

        ///// <summary>
        ///// Retorna as compras do usuário logado.
        ///// </summary>
        ///// <param name="pg"></param>
        ///// <param name="qtdpg"></param>
        ///// <returns></returns>
        [Route("clientes/logado/compras2")]
        [HttpGet]
        public RetornoModel<PagedListModel<CompraModel>> GetComprasLogado2(int pg = 1, int qtdpg = 1)
        {
            LogUtil.Info(string.Format("##Get.Compras2## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<PagedListModel<CompraModel>> retorno = new RetornoModel<PagedListModel<CompraModel>>();
            IPagedList<tVendaBilheteria> compras = null;
            List<CompraModel> listCompra = new List<CompraModel>();
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    try
                    {
                        VendaBilheteriaBO vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        FormaPagamentoBO formaPagamentoBO = new FormaPagamentoBO(ado);

                        compras = vendaBilheteriaBO.ListarCliente(pg, qtdpg, SessionModel.Login);
                        for (int i = 0; i < compras.Count; i++)
                        {
                            var item = compras[i];
                            CompraModel compra = new CompraModel();
                            if (item.tIngresso != null && item.tIngresso.Count() > 0)
                            {
                                decimal valorTaxaConveniencia = (item.TaxaConvenienciaValorTotal ?? 0) / item.tIngresso.Count();
                                //Converte a lista de Ingressos em uma lista de Carrinhos
                                compra.CarrinhoItens = item.tIngresso.Select(x => x.toCarrinho(valorTaxaConveniencia)).ToList();
                            }
                            else
                            {
                                CarrinhoBO carrinhoBO = new CarrinhoBO(ado);
                                compra.CarrinhoItens = carrinhoBO.ListarVendaBilheteria(item.ID);
                            }
                            compra.ClienteEnderecoID = item.ClienteEnderecoID ?? 0;
                            compra.EntregaComEndereco = (item.ClienteEnderecoID ?? 0) > 0;
                            compra.EntregaControleID = item.EntregaControleID ?? 0;

                            //Pagamento
                            compra.Pagamentos = new List<CompraPagamentoModel>();

                            //Pega todas formas de pagamento
                            List<int> formasPagamentoIDs = item.tVendaBilheteriaFormaPagamento.Select(x => x.FormaPagamentoID ?? 0).ToList();
                            if (formasPagamentoIDs.Count > 0)
                            {
                                List<FormaPagamento> formasPagamento = formaPagamentoBO.Listar(formasPagamentoIDs);

                                //retornas as formas de pagamento desta lista.
                                foreach (var formaPagamento in item.tVendaBilheteriaFormaPagamento.Where(x => x.ValeIngressoID == null || x.ValeIngressoID == 0))
                                {
                                    CompraPagamentoModel pagamento = new CompraPagamentoModel();
                                    pagamento.Valor = formaPagamento.Valor ?? 0;
                                    pagamento.formaPagamento = formasPagamento.FirstOrDefault(x => x.IR_FormaPagamentoID == formaPagamento.FormaPagamentoID);
                                    tCartao cartao = formaPagamento.tCartao;
                                    if (cartao != null)
                                    {
                                        pagamento.NomeCartao = cartao.NomeCartao;
                                        pagamento.NumeroCartao = cartao.NroCartao;
                                    }

                                    compra.Pagamentos.Add(pagamento);
                                }
                            }

                            compra.PDVID = item.PdvID ?? 0;
                            //Totais
                            compra.Total = new CompraTotalModel();
                            compra.Total.ValorDesconto = item.tVendaBilheteriaFormaPagamento.Where(x => x.ValeIngressoID != null && x.ValeIngressoID > 0).Sum(x => x.Valor ?? 0);
                            compra.Total.ValorTaxaConveniencia = item.TaxaConvenienciaValorTotal ?? 0;
                            compra.Total.ValorTaxaEntrega = item.TaxaEntregaValor ?? 0;
                            compra.Total.ValorTotal = item.ValorTotal ?? 0;
                            compra.Total.ValorIngressos = compra.Total.ValorTotal - compra.Total.ValorTaxaEntrega - compra.Total.ValorTaxaConveniencia;
                            compra.ValorTotalSeguro = item.ValorSeguro ?? 0;

                            compra.ValeIngressos = item.tVendaBilheteriaFormaPagamento.Where(x => x.tValeIngresso != null).Select(x => x.tValeIngresso).ToList();

                            //Limpa os itens da venda bilheteria que não devem ser retornados
                            item.tVendaBilheteriaFormaPagamento = new List<tVendaBilheteriaFormaPagamento>();
                            item.tIngresso = new List<tIngresso>();

                            compra.VendaBilheteria = item;
                            listCompra.Add(compra);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
                if (compras != null)
                {
                    retorno.Retorno = new PagedListModel<CompraModel>();
                    retorno.Retorno.CopyFrom(compras);
                    retorno.Retorno.Itens = listCompra;
                    retorno.Sucesso = true;
                    retorno.Mensagem = "OK";
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.Mensagem = "Nenhuma compra realizada";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                }
            }
            return ParseRetorno(retorno);
        }

        [Route("clientes/logado/compras/{senha}/agregados")]
        [HttpGet]
        public RetornoModel<List<ClienteAgregadoModel>> GetAgregados(string senha)
        {
            LogUtil.Info(string.Format("##Get.Agregados## SESSION {0}, SENHA {1}", this.SessionModel.SessionID, senha));

            RetornoModel<List<ClienteAgregadoModel>> retorno = new RetornoModel<List<ClienteAgregadoModel>>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    VendaBilheteriaBO vendaBilheteriaBO = null;
                    tVendaBilheteria vb = null;
                    try
                    {
                        vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        vb = vendaBilheteriaBO.Consultar(senha);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Sucesso = false;
                        retorno.Mensagem = ex.Message;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }

                    if (vb != null)
                    {
                        if (vb.ClienteID == SessionModel.ClienteID)
                        {
                            List<AgregadoModel> listaAgregados = null;
                            try
                            {
                                listaAgregados = vendaBilheteriaBO.ConsultarAgregados(vb.ID);
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Error(ex);
                                retorno.Sucesso = false;
                                retorno.Mensagem = ex.Message;
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                            }
                            retorno.Retorno = listaAgregados.Select(x => new ClienteAgregadoModel(x)).ToList();
                        }
                        else
                        {
                            retorno.Mensagem = "Compra não pertence ao usuário atual";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Compra inexistente";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            return retorno;
        }

        [Route("clientes/logado/senhas_compra/{senha}/agregado")]
        [HttpPut]
        public RetornoModel PutAgregado(string senha, [FromBody]ClienteAgregadoModel agregado)
        {
            LogUtil.Info(string.Format("##Put.Agregado## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel retorno = new RetornoModel();
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    VendaBilheteriaBO vendaBilheteriaBO = null;
                    tVendaBilheteria vb = null;
                    try
                    {
                        vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        vb = vendaBilheteriaBO.Consultar(senha);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Sucesso = false;
                        retorno.Mensagem = ex.Message;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                    if (vb != null)
                    {
                        if (vb.ClienteID == SessionModel.ClienteID)
                        {
                            if (!string.IsNullOrWhiteSpace(agregado.Nome) && !string.IsNullOrWhiteSpace(agregado.CPF) && !string.IsNullOrWhiteSpace(agregado.Email) && !string.IsNullOrWhiteSpace(agregado.Telefone))
                            {
                                try
                                {
                                    retorno.Sucesso = true;
                                    retorno.Mensagem = vendaBilheteriaBO.PutAgregado(vb.ID, agregado.Nome, agregado.CPF, agregado.Email, agregado.Email);
                                }
                                catch (Exception ex)
                                {
                                    LogUtil.Error(ex);
                                    retorno.Sucesso = false;
                                    retorno.Mensagem = ex.Message;
                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                                }
                            }
                            else
                            {
                                retorno.Mensagem = "Objeto não encontrado ou em formato inválido";
                                retorno.Sucesso = false;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
                            }
                        }
                        else
                        {
                            retorno.Mensagem = "Compra não pertence ao usuário atual";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Compra inexistente";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            return retorno;
        }

        [Route("clientes/logado/senhas_compra/{senha}/agregado")]
        [HttpDelete]
        public RetornoModel DeleteAgregado(string senha)
        {
            RetornoModel retorno = new RetornoModel();
            using (var ado = new MasterADOBase())
            {

                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    VendaBilheteriaBO vendaBilheteriaBO = null;
                    tVendaBilheteria vb = null;
                    try
                    {
                        vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        vb = vendaBilheteriaBO.Consultar(senha);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Sucesso = false;
                        retorno.Mensagem = ex.Message;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                    if (vb != null)
                    {
                        if (vb.ClienteID == SessionModel.ClienteID)
                        {
                            try
                            {
                                retorno.Sucesso = true;
                                retorno.Mensagem = vendaBilheteriaBO.DeleteAgregado(vb.ID);
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Error(ex);
                                retorno.Sucesso = false;
                                retorno.Mensagem = ex.Message;
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                            }
                        }
                        else
                        {
                            retorno.Mensagem = "Compra não pertence ao usuário atual";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Compra inexistente";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            return retorno;
        }

        [Route("clientes/logado/senhas_compra")]
        [HttpGet]
        public RetornoModel<PagedListModel<SenhaCompraModel>> GetSenhasCompra(int pg = 1, int qtdpg = 1)
        {
            RetornoModel<PagedListModel<SenhaCompraModel>> retorno = new RetornoModel<PagedListModel<SenhaCompraModel>>();
            IPagedList<SenhaCompraModel> compras = null;
            List<SenhaCompraModel> listCompra = new List<SenhaCompraModel>();
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    try
                    {
                        VendaBilheteriaBO vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        compras = vendaBilheteriaBO.ConsultarSenhasCompra(pg, qtdpg, SessionModel.Login);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
                if (compras != null)
                {
                    retorno.Retorno = new PagedListModel<SenhaCompraModel>();
                    retorno.Retorno.CopyFrom(compras);
                    retorno.Retorno.Itens = compras.ToList();
                    retorno.Sucesso = true;
                    retorno.Mensagem = "OK";
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.Mensagem = "Nenhuma compra realizada";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                }
            }
            return ParseRetorno(retorno);
        }

        [Route("clientes/logado/senhas_compra/{senha}")]
        [HttpGet]
        public RetornoModel<SenhaCompraDetalhe> GetSenhaCompraDetalhe(string senha)
        {
            RetornoModel<SenhaCompraDetalhe> retorno = new RetornoModel<SenhaCompraDetalhe>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            using (var ado = new MasterADOBase())
            {

                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    VendaBilheteriaBO vendaBilheteriaBO = null;
                    tVendaBilheteria vb = null;
                    try
                    {
                        vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        vb = vendaBilheteriaBO.Consultar(senha);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Sucesso = false;
                        retorno.Mensagem = ex.Message;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                    if (vb != null)
                    {
                        if (vb.ClienteID == SessionModel.ClienteID)
                        {
                            if (vb.Status != "C")
                            {
                                try
                                {
                                    retorno.Retorno = vendaBilheteriaBO.ConsultarSenhasCompraDetalhe(vb.ID);
                                }
                                catch (Exception ex)
                                {
                                    LogUtil.Error(ex);
                                    retorno.Sucesso = false;
                                    retorno.Mensagem = ex.Message;
                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                                }
                            }
                            else
                            {
                                retorno.Mensagem = "Compra inexistente";
                                retorno.Sucesso = false;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                            }
                        }
                        else
                        {
                            retorno.Mensagem = "Compra não pertence ao usuário atual";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Compra inexistente";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            return retorno;
        }

        [Route("clientes/logado/senhas_compra/{senha}/informacoes_cancelamento")]
        [HttpGet]
        public RetornoModel<SenhaCompraDetalheCancelamento> GetInformacoesCancelamento(string senha)
        {
            RetornoModel<SenhaCompraDetalheCancelamento> retorno = new RetornoModel<SenhaCompraDetalheCancelamento>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";

            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    VendaBilheteriaBO vendaBilheteriaBO = null;
                    tVendaBilheteria vb = null;
                    try
                    {
                        vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        vb = vendaBilheteriaBO.Consultar(senha);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Sucesso = false;
                        retorno.Mensagem = ex.Message;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                    if (vb != null)
                    {
                        if (vb.ClienteID == SessionModel.ClienteID)
                        {
                            if (vb.Status != "C")
                            {
                                try
                                {
                                    SenhaCompraDetalheCancelamento detalhe = vendaBilheteriaBO.ConsultarSenhasCompraDetalheCancelamento(vb.ID, canalInternetId);
                                    if (detalhe.Pagamentos != null)
                                    {
                                        if (detalhe.FormaDevolucaoDisponivel == "C")
                                        {
                                            detalhe.Pagamentos[0].CPFTitular = SessionModel.Login.Cliente.CPF;
                                            if (string.IsNullOrEmpty(detalhe.Pagamentos[0].NomeTitular))
                                                detalhe.Pagamentos[0].NomeTitular = SessionModel.Login.Cliente.Nome;
                                        }
                                    }
                                    retorno.Retorno = detalhe;
                                }
                                catch (Exception ex)
                                {
                                    LogUtil.Error(ex);
                                    retorno.Sucesso = false;
                                    retorno.Mensagem = ex.Message;
                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                                }
                            }

                            else
                            {
                                retorno.Mensagem = "Compra inexistente";
                                retorno.Sucesso = false;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                            }
                        }
                        else
                        {
                            retorno.Mensagem = "Compra não pertence ao usuário atual";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Compra inexistente";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }

            return retorno;
        }

        ///// <summary>
        ///// Retorna as compras do usuário logado.
        ///// </summary>
        ///// <param name="pg"></param>
        ///// <param name="qtdpg"></param>
        ///// <returns></returns>
        [Route("clientes/logado/senhas_cancelamento")]
        [HttpGet]
        public RetornoModel<PagedListModel<SenhaCancelamentoModel>> GetSenhasCancelamento(int pg = 1, int qtdpg = 1)
        {
            RetornoModel<PagedListModel<SenhaCancelamentoModel>> retorno = new RetornoModel<PagedListModel<SenhaCancelamentoModel>>();
            IPagedList<SenhaCancelamentoModel> compras = null;
            List<SenhaCancelamentoModel> listCompra = new List<SenhaCancelamentoModel>();
            using (var ado = new MasterADOBase())
            {
                ClienteBO clienteBO = new ClienteBO(ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    try
                    {
                        VendaBilheteriaBO vendaBilheteriaBO = new VendaBilheteriaBO(ado);

                        compras = vendaBilheteriaBO.ConsultarSenhasCancelamento(pg, qtdpg, SessionModel.Login);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Mensagem = ex.Message;
                        retorno.Sucesso = false;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
                if (compras != null)
                {
                    retorno.Retorno = new PagedListModel<SenhaCancelamentoModel>();
                    retorno.Retorno.CopyFrom(compras);
                    retorno.Retorno.Itens = compras.ToList();
                    retorno.Sucesso = true;
                    retorno.Mensagem = "OK";
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.Mensagem = "Nenhuma compra realizada";
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                }
            }
            return ParseRetorno(retorno);
        }

        [Route("clientes/logado/senhas_cancelamento")]
        [HttpPost]
        public RetornoModel<string, enumTipoRetornoCancelamento> PostCancelamentoOnline([FromBody]CancelamentoAPIModel requestModel)
        {
            RetornoModel<string, enumTipoRetornoCancelamento> retorno = new RetornoModel<string, enumTipoRetornoCancelamento>();
            if (requestModel != null)
            {
                using (var ado = new MasterADOBase())
                {
                    ClienteBO clienteBO = new ClienteBO(ado);
                    CarregarCliente(clienteBO);
                    if (SessionModel.Login != null)
                    {
                        if (requestModel.SenhaCompra != null)
                        {
                            VendaBilheteriaBO vendaBilheteriaBO = null;
                            tVendaBilheteria vendaBilheteria = null;
                            try
                            {
                                vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                                vendaBilheteria = vendaBilheteriaBO.Consultar(requestModel.SenhaCompra);
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Error(ex);
                                retorno.Sucesso = false;
                                retorno.Mensagem = ex.Message;
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                            }
                            if (vendaBilheteria != null && vendaBilheteria.Status != "C")
                            {
                                if (SessionModel.ClienteID == vendaBilheteria.ClienteID)
                                {
                                    SenhaCompraDetalheCancelamento senhaCompraDetalheCancelamento = vendaBilheteriaBO.ConsultarSenhasCompraDetalheCancelamento(vendaBilheteria.ID, canalInternetId);

                                    if (requestModel.CancelaSeguro)
                                    {
                                        if (requestModel.Ingressos.Count == 0)
                                        {
                                            retorno.Mensagem = "Seguro dessa compra não pode ser cancelado.";
                                            retorno.Tipo = enumTipoRetornoCancelamento.cancelSeguroIndisponivel;
                                            retorno.Sucesso = false;
                                            NewRelicIgnoreTransaction();
                                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                        }
                                    }

                                    if (requestModel.CancelaEntrega)
                                    {
                                        if (!senhaCompraDetalheCancelamento.PodeCancelarEntrega)
                                        {
                                            retorno.Mensagem = "Entrega dessa compra não pode ser cancelada pois um ou mais ingressos já foram impressos.";
                                            retorno.Tipo = enumTipoRetornoCancelamento.cancelEntregaIndisponivel;
                                            retorno.Sucesso = false;
                                            NewRelicIgnoreTransaction();
                                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                        }
                                    }

                                    if (senhaCompraDetalheCancelamento.FormaDevolucaoDisponivel != requestModel.FormaDevolucao)
                                    {
                                        retorno.Mensagem = "Forma de devolução indisponível para essa compra.";
                                        retorno.Tipo = enumTipoRetornoCancelamento.formaDevolucaoIndisponivel;
                                        retorno.Sucesso = false;
                                        NewRelicIgnoreTransaction();
                                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                    }

                                    if (requestModel.Ingressos.Count == 0 && !requestModel.CancelaEntrega)
                                    {
                                        retorno.Mensagem = "Nenhum ingresso selecionado para cancelamento.";
                                        retorno.Tipo = enumTipoRetornoCancelamento.nenhumIngressoSelecionado;
                                        retorno.Sucesso = false;
                                        NewRelicIgnoreTransaction();
                                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                    }

                                    IEnumerable<IngressoDetalheCancelamento> ingressosCancelar = senhaCompraDetalheCancelamento.Ingressos.Where(I => requestModel.Ingressos.Contains(I.ID));
                                    IEnumerable<IngressoDetalheCancelamento> ingressosPacote = ingressosCancelar.Where(I => !String.IsNullOrEmpty(I.PacoteGrupo));

                                    if (ingressosPacote != null && ingressosPacote.Count() > 0)
                                    {
                                        List<string> pacotesFechados = ingressosPacote.Where(I => !I.PacotePermitirCancelamentoAvulso).Select(I => I.PacoteGrupo).Distinct().ToList();
                                        foreach (string grupo in pacotesFechados)
                                        {
                                            int ingressoPacoteCancelar = ingressosPacote.Where(I => I.PacoteGrupo == grupo).Count();
                                            int ingressoPacoteCompra = senhaCompraDetalheCancelamento.Ingressos.Where(I => I.PacoteGrupo == grupo).Count();
                                            if (ingressoPacoteCancelar != ingressoPacoteCompra)
                                            {
                                                retorno.Mensagem = "Todos os ingressos do pacote " + ingressosPacote.FirstOrDefault(I => I.PacoteGrupo == grupo).PacoteNome + " devem ser cancelados juntos.";
                                                retorno.Tipo = enumTipoRetornoCancelamento.cancelamentoPacoteCompleto;
                                                retorno.Sucesso = false;
                                                NewRelicIgnoreTransaction();
                                                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                            }
                                        }
                                    }

                                    if (ingressosCancelar.Any(x => x.TemDevolucao && !x.Cancelado))
                                    {
                                        if (!requestModel.CienteDevolucao)
                                        {
                                            retorno.Mensagem = "É obrigatório a ciência da devolução de um ou mais ingressos para efetivar o cancelamento.";
                                            retorno.Tipo = enumTipoRetornoCancelamento.devolucaoIngressosExigida;
                                            retorno.Sucesso = false;
                                            NewRelicIgnoreTransaction();
                                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                        }
                                    }

                                    foreach (int ingressoID in requestModel.Ingressos)
                                    {
                                        IngressoDetalheCancelamento ingresso = senhaCompraDetalheCancelamento.Ingressos.FirstOrDefault(x => x.ID == ingressoID);
                                        if (ingresso != null)
                                        {
                                            if (ingresso.Cancelado)
                                            {
                                                if (ingresso.StatusCancelamentoCodigo == "P")
                                                {
                                                    retorno.Mensagem = "O ingresso " + ingresso.Codigo + @" já está com o processo de cancelamento em andamento e tem devolução do ingresso pendente.
                                                                    Esta devolução pode ser realizada em um Ponto de Venda em sua cidade ou enviando o mesmo via Correios para o endereço 'Rua Escorpião, 91 - Alphaville Conde Comercial 1 - Bairro Alphaville - Barueri - São Paulo - CEP 29829-232'.
                                                                    A devolução do valor do seu ingresso está condicionada ao recebimento deste. Em caso de dúvidas, entre em contato com nosso atendimento 4003-2051.";
                                                    retorno.Tipo = enumTipoRetornoCancelamento.ingressoCanceladoPendente;
                                                    retorno.Sucesso = false;
                                                    NewRelicIgnoreTransaction();
                                                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                                }
                                                else
                                                {
                                                    retorno.Mensagem = "Ingresso " + ingresso.Codigo + " já cancelado.";
                                                    retorno.Tipo = enumTipoRetornoCancelamento.ingressoCancelado;
                                                    retorno.Sucesso = false;
                                                    NewRelicIgnoreTransaction();
                                                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            retorno.Mensagem = "Ingresso não pertence a essa compra";
                                            retorno.Tipo = enumTipoRetornoCancelamento.ingressoInexistente;
                                            retorno.Sucesso = false;
                                            NewRelicIgnoreTransaction();
                                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                                        }
                                    }

                                    switch (requestModel.FormaDevolucao)
                                    {
                                        case "D":
                                            string propriedadeNula = null;
                                            if (requestModel.DadosDeposito != null)
                                            {
                                                #region DadosDeposito check propriedades nulas
                                                if (String.IsNullOrEmpty(requestModel.DadosDeposito.Poupanca))
                                                {
                                                    propriedadeNula = "Poupanca";
                                                }
                                                else if (String.IsNullOrEmpty(requestModel.DadosDeposito.Banco))
                                                {
                                                    propriedadeNula = "Banco";
                                                }
                                                else if (String.IsNullOrEmpty(requestModel.DadosDeposito.Agencia))
                                                {
                                                    propriedadeNula = "Agencia";
                                                }
                                                else if (String.IsNullOrEmpty(requestModel.DadosDeposito.Conta))
                                                {
                                                    propriedadeNula = "Conta";
                                                }
                                                else if (String.IsNullOrEmpty(requestModel.DadosDeposito.DV))
                                                {
                                                    propriedadeNula = "Digito Verificador";
                                                }
                                                else if (String.IsNullOrEmpty(requestModel.DadosDeposito.CPF))
                                                {
                                                    propriedadeNula = "CPF";
                                                }
                                                else if (String.IsNullOrEmpty(requestModel.DadosDeposito.Nome))
                                                {
                                                    propriedadeNula = "Nome";
                                                }

                                                if (propriedadeNula != null)
                                                {
                                                    retorno.Mensagem = "Dados de depositos são obrigatórios para essa forma de devolução. Campo " + propriedadeNula + " está nulo";
                                                    retorno.Tipo = enumTipoRetornoCancelamento.dadosDepositoPropriedadeNula;
                                                    retorno.Sucesso = false;
                                                    NewRelicIgnoreTransaction();
                                                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                                }
                                                #endregion

                                                #region DadosDeposito check tamanho dos campos
                                                string msg = null;
                                                if (requestModel.DadosDeposito.Banco.Length > 50)
                                                {
                                                    msg = "O campo 'Banco' deve ter no máximo 50 caracteres";
                                                }
                                                else if (requestModel.DadosDeposito.Agencia.Length > 5)
                                                {
                                                    msg = "O campo 'Agencia' deve ter no máximo 5 caracteres";
                                                }
                                                else if (requestModel.DadosDeposito.Conta.Length > 10)
                                                {
                                                    msg = "O campo 'Conta' deve ter no máximo 50 caracteres";
                                                }
                                                else if (requestModel.DadosDeposito.DV.Length > 5)
                                                {
                                                    msg = "O campo 'Digito Verificador' deve ter no máximo 5 caractere";
                                                }
                                                else if (requestModel.DadosDeposito.CPF.Length > 14)
                                                {
                                                    msg = "O campo 'CPF' deve ter no máximo 14 caracteres";
                                                }
                                                else if (requestModel.DadosDeposito.Nome.Length > 50)
                                                {
                                                    msg = "O campo 'Nome' deve ter no máximo 50 caracteres";
                                                }

                                                if (msg != null)
                                                {
                                                    retorno.Mensagem = msg;
                                                    retorno.Tipo = enumTipoRetornoCancelamento.dadosDepositoPropriedadeNula;
                                                    retorno.Sucesso = false;
                                                    NewRelicIgnoreTransaction();
                                                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                retorno.Mensagem = "Dados de depositos são obrigatórios para essa forma de devolução.";
                                                retorno.Tipo = enumTipoRetornoCancelamento.dadosDepositoPropriedadeNula;
                                                retorno.Sucesso = false;
                                                NewRelicIgnoreTransaction();
                                                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                            }
                                            break;
                                        case "P":
                                        case "C":
                                            if (!requestModel.CartaoProprio)
                                            {
                                                string propriedadeNulaCartao = null;
                                                if (requestModel.DadosCartao != null)
                                                {
                                                    if (String.IsNullOrEmpty(requestModel.DadosCartao.TitularNome))
                                                    {
                                                        propriedadeNulaCartao = "Titular Cartão";
                                                    }
                                                    else if (String.IsNullOrEmpty(requestModel.DadosCartao.TitularCPF))
                                                    {
                                                        propriedadeNulaCartao = "Titular CPF";
                                                    }

                                                    if (propriedadeNulaCartao != null)
                                                    {
                                                        retorno.Mensagem = "Dados do cartão são obrigatórios para essa forma de devolução, pois o cartão não pertence ao cliente. Campo " + propriedadeNulaCartao + " está nulo.";
                                                        retorno.Tipo = enumTipoRetornoCancelamento.dadosDepositoPropriedadeNula;
                                                        retorno.Sucesso = false;
                                                        NewRelicIgnoreTransaction();
                                                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                                    }
                                                }
                                                else
                                                {
                                                    retorno.Mensagem = "Dados do cartão são obrigatórios para essa forma de devolução, pois o cartão não pertence ao cliente.";
                                                    retorno.Tipo = enumTipoRetornoCancelamento.dadosCartaoPropriedadeNula;
                                                    retorno.Sucesso = false;
                                                    NewRelicIgnoreTransaction();
                                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                                                }
                                            }
                                            break;
                                        default:
                                            retorno.Mensagem = "Forma de devolução inválida.";
                                            retorno.Tipo = enumTipoRetornoCancelamento.dadosCartaoPropriedadeNula;
                                            retorno.Sucesso = false;
                                            NewRelicIgnoreTransaction();
                                            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                                    }
                                    #region Cancelamento
                                    CaixaBO caixaBO = null;
                                    CanalBO canalBO = null;
                                    LojaBO lojaBO = null;
                                    tCaixa caixa = null;
                                    tCanal canal = null;
                                    try
                                    {
                                        caixaBO = new CaixaBO(ado);
                                        canalBO = new CanalBO(ado);
                                        lojaBO = new LojaBO(ado);
                                        canal = canalBO.Consultar(SessionModel.CanalID);
                                        caixa = caixaBO.ConsultarCaixaInternet(SessionModel.UsuarioID, lojaBO.Consultar(SessionModel.LojaID), canal).Retorno;
                                    }
                                    catch (Exception ex)
                                    {
                                        LogUtil.Error(ex);
                                        retorno.Sucesso = false;
                                        retorno.Mensagem = ex.Message;
                                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                                    }
                                    if (caixa == null)
                                    {
                                        retorno.Sucesso = false;
                                        retorno.Mensagem = "Nenhum Caixa disponível";
                                        NewRelicIgnoreTransaction();
                                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                                    }
                                    CancelamentoAPIRetornoModel retornoCancel = null;
                                    retornoCancel = vendaBilheteriaBO.SolicitarCancelamento(requestModel, senhaCompraDetalheCancelamento,
                                        SessionModel.ClienteID, SessionModel.UsuarioID, SessionModel.CanalID, SessionModel.LojaID,
                                        caixa.ID, canal.EmpresaID ?? 0, vendaBilheteria.ID, SessionModel.Login.Cliente.Email);
                                    retorno.Sucesso = retornoCancel.Sucesso;
                                    if (retorno.Sucesso)
                                    {
                                        retorno.Retorno = retornoCancel.SenhaCancelamento;
                                        retorno.Mensagem = @"Você pode consultar o status do seu cancelamento através do menu Minhas Compras ou através do nosso SAC.<br/>
                                                             Caso possua qualquer problema ou dúvida durante o processo, por favor, mantenha contato com o nosso SAC (Serviço de Atendimento ao Cliente) no telefone 4003-2051 ou pelo e-mail sac@ingressorapido.com.br.<br/>
                                                             Em ambos os contatos já tenha em mãos o CPF ou e-mail do titular da compra, somado ao número da senha de compra ou de cancelamento da compra.<br/>
                                                             Iremos analisar o ocorrido e retornaremos com um número de protocolo no qual iremos, informar o ocorrido e a forma de regularização. Caso seja evidente que a pendencia seja da Ingresso Rápido, iremos ressarci-lo da forma mais ágil possível. Não deixe de nos acionar!<br>";
                                        retorno.Tipo = (ingressosCancelar.Any(x => x.TemDevolucao && !x.Cancelado)) ? enumTipoRetornoCancelamento.devolucaoIngressos : enumTipoRetornoCancelamento.canceladoComSucesso;
                                    }
                                    else
                                    {
                                        retorno.Mensagem = "Não foi possível realizar o cancelamento de sua compra. " + retornoCancel.Erro;
                                        retorno.Tipo = enumTipoRetornoCancelamento.cancelamentoAbortado;
                                    }
                                    #endregion
                                }
                                else
                                {
                                    retorno.Mensagem = "Senha de compra inexistente";
                                    retorno.Tipo = enumTipoRetornoCancelamento.senhaInexistente;
                                    retorno.Sucesso = false;
                                    NewRelicIgnoreTransaction();
                                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                                }
                            }
                            else
                            {
                                retorno.Mensagem = "Senha de compra inexistente";
                                retorno.Tipo = enumTipoRetornoCancelamento.senhaInexistente;
                                retorno.Sucesso = false;
                                NewRelicIgnoreTransaction();
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
                            }
                        }
                        else
                        {
                            retorno.Mensagem = "Senha de compra está nula";
                            retorno.Tipo = enumTipoRetornoCancelamento.senhaNula;
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Nenhum usuário logado";
                        retorno.Tipo = enumTipoRetornoCancelamento.loginRequerido;
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
            }
            else
            {
                retorno.Mensagem = "Objeto de request obrigatório está nulo";
                retorno.Tipo = enumTipoRetornoCancelamento.requestInvalido;
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }
            return retorno;
        }

        [Route("clientes/logado/senhas_cancelamento/{senha}")]
        [HttpGet]
        public RetornoModel<SenhaCancelamentoDetalhe> GetSenhaCancelamentoDetalhe(string senha)
        {
            RetornoModel<SenhaCancelamentoDetalhe> retorno = new RetornoModel<SenhaCancelamentoDetalhe>();
            retorno.Sucesso = true;
            retorno.Mensagem = "OK";
            using (var ado = new MasterADOBase())
            {

                ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                CarregarCliente(clienteBO);
                if (SessionModel.Login != null)
                {
                    VendaBilheteriaBO vendaBilheteriaBO = null;
                    tVendaBilheteria vb = null;
                    try
                    {
                        vendaBilheteriaBO = new VendaBilheteriaBO(ado);
                        vb = vendaBilheteriaBO.Consultar(senha);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex);
                        retorno.Sucesso = false;
                        retorno.Mensagem = ex.Message;
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                    }
                    if (vb != null)
                    {
                        if (vb.ClienteID == SessionModel.ClienteID)
                        {
                            try
                            {
                                retorno.Retorno = vendaBilheteriaBO.ConsultarSenhasCancelamentoDetalhe(vb.ID);
                            }
                            catch (Exception ex)
                            {
                                LogUtil.Error(ex);
                                retorno.Sucesso = false;
                                retorno.Mensagem = ex.Message;
                                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                            }
                        }
                        else
                        {
                            retorno.Mensagem = "Compra não pertence ao usuário atual";
                            retorno.Sucesso = false;
                            NewRelicIgnoreTransaction();
                            throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                        }
                    }
                    else
                    {
                        retorno.Mensagem = "Compra inexistente";
                        retorno.Sucesso = false;
                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                    }
                }
                else
                {
                    retorno.Mensagem = "Nenhum usuário logado";
                    retorno.Sucesso = false;
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
            }
            return retorno;
        }

        /// <summary>
        /// Atualiza senha do uauário.
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        [Route("clientes/logado/senha")]
        [HttpPut]
        public RetornoModel<Login, enumUsuarioException> PutTrocaSenha([FromBody]ClienteTrocaSenhaRequestModel cliente)
        {
            LogUtil.Info(string.Format("##Put.TrocaSenha## SESSION {0}", this.SessionModel.SessionID));

            RetornoModel<Login, enumUsuarioException> retorno = null;
            try
            {
                using (var ado = new MasterADOBase())
                {
                    var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);

                    var biletoEnabled = ConfiguracaoAppUtil.GetAsBool("BILETO_ENABLED");
                    if (biletoEnabled)
                    {
                        LogUtil.Info(string.Format("##Put.TrocaSenha.BiletoAtivo## SESSION {0}, MSG: {1}", this.SessionModel.SessionID, "Trocando senha no Bileto"));
                        retorno = clienteBO.ChangePassword(cliente.oldPassword, cliente.newPassword, this.SessionModel.biletoToken);

                        if (!retorno.Sucesso && retorno.Mensagem == "biletoOffline")
                        {
                            CarregarCliente(clienteBO);
                            retorno = clienteBO.MudarSenha(SessionModel.Login, cliente.oldPassword, cliente.newPassword);
                        }
                    }
                    else
                    {
                        LogUtil.Info(string.Format("##Put.TrocaSenha.ByPassBileto## SESSION {0}, EMAIL {1}, MSG: {2}", this.SessionModel.SessionID, SessionModel.Login.Email, "Bileto inativo, trocando senha local"));

                        CarregarCliente(clienteBO);
                        retorno = clienteBO.MudarSenha(SessionModel.Login, cliente.oldPassword, cliente.newPassword);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
                retorno = new RetornoModel<Login, enumUsuarioException>();
                retorno.Sucesso = false;
                retorno.Mensagem = ex.Message;
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, ParseRetorno(retorno)));
            }

            if (!retorno.Sucesso)
            {
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }
            return ParseRetorno(retorno);
        }

        [Route("clientes/senha/envia_nova")]
        [HttpPost]
        public RetornoModel PostEnviarNovaSenha([FromBody] ClienteEnviarNovaSenhaRequestModel cliente)
        {
            RetornoModel retorno = new RetornoModel();
            if (cliente != null && !string.IsNullOrWhiteSpace(cliente.email) && !string.IsNullOrWhiteSpace(cliente.cpf))
            {
                try
                {
                    using (var ado = new MasterADOBase())
                    {
                        ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                        var retornoLogin = clienteBO.EnviarSenha(cliente.email, cliente.cpf.Replace(new string[] { ".", "-" }, ""));
                        retorno.Mensagem = retornoLogin.Mensagem;
                        retorno.Sucesso = retornoLogin.Sucesso;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                    retorno = new RetornoModel();
                    retorno.Mensagem = ex.Message;
                    retorno.Sucesso = false;
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }

                if (!retorno.Sucesso)
                {
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
                else
                {
                    return ParseRetorno(retorno);
                }
            }
            else
            {
                retorno.Mensagem = "Objeto não encontrado ou em formato inválido";
                retorno.Sucesso = false;
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
            }
        }

        [Route("clientes/senha/recuperar")]
        [HttpPost]
        public RetornoModel<string> PostRecuperarSenha([FromBody] ClienteRecuperarSenhaRequestModel recuperarSenha)
        {
            LogUtil.Info(string.Format("##Post.RecuperarSenha## SESSION {0}", this.SessionModel.SessionID));
            var retorno = new RetornoModel<string>();
            if (recuperarSenha != null && !string.IsNullOrWhiteSpace(recuperarSenha.emailcpf))
            {
                LogUtil.Info(string.Format("##Post.RecuperarSenha## SESSION {0}, USERNAME {1}", this.SessionModel.SessionID, recuperarSenha.emailcpf));

                try
                {
                    using (var ado = new MasterADOBase())
                    {
                        var clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                        var biletoEnabled = ConfiguracaoAppUtil.GetAsBool("BILETO_ENABLED");

                        if (biletoEnabled)
                        {
                            LogUtil.Info(string.Format("##Post.RecuperarSenha.BiletoAtivo## SESSION {0}, EMAILCPF {1}, MSG`: {2}", this.SessionModel.SessionID, recuperarSenha.emailcpf, "Recuperando senha no Bileto"));
                            retorno = clienteBO.BiletoForgottenPassword(recuperarSenha.emailcpf, recuperarSenha.url);

                            if (!retorno.Sucesso && retorno.Mensagem == "biletoOffline")
                            {
                                var login = clienteBO.ConsultarUsername(recuperarSenha.emailcpf, false);

                                if (login != null && login.Cliente != null && !string.IsNullOrWhiteSpace(login.Cliente.Email) && login.Cliente.Email.IsEmail())
                                {
                                    retorno = clienteBO.EnviarEmailRedefinirSenha(login.Cliente.Email, recuperarSenha.url);
                                }
                                else
                                {
                                    retorno.Mensagem = "Cliente não encontrado";
                                    retorno.Sucesso = false;
                                }
                            }
                        }
                        else
                        {
                            LogUtil.Info(string.Format("##Post.RecuperarSenha.ByPassBileto## SESSION {0}, EMAILCPF {1}, MSG`: {2}", this.SessionModel.SessionID, recuperarSenha.emailcpf, "Bileto inativo, recuperando senha local"));

                            var login = clienteBO.ConsultarUsername(recuperarSenha.emailcpf, false);

                            if (login != null && login.Cliente != null && !string.IsNullOrWhiteSpace(login.Cliente.Email) && login.Cliente.Email.IsEmail())
                            {
                                retorno = clienteBO.EnviarEmailRedefinirSenha(login.Cliente.Email, recuperarSenha.url);
                            }
                            else
                            {
                                retorno.Mensagem = "Cliente não encontrado";
                                retorno.Sucesso = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Post.RecuperarSenha.ERROR## SESSION {0}, USERNAME {1}, MSG {2}", this.SessionModel.SessionID, recuperarSenha.emailcpf, ex.Message));
                    retorno.Mensagem = "Para melhor atende-los,estamos em manutenção no momento.";
                    retorno.Sucesso = false;

                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }

                if (retorno.Sucesso)
                {
                    LogUtil.Info(string.Format("##Post.RecuperarSenha.Sucesso## SESSION {0}, USERNAME {1}", this.SessionModel.SessionID, recuperarSenha.emailcpf));

                    return ParseRetorno(retorno);
                }

                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
            }

            LogUtil.Info(string.Format("##Post.RecuperarSenha.ERROR## SESSION {0}, MSG {1}", this.SessionModel.SessionID, "Objeto não encontrado."));
            retorno.Mensagem = "Para melhor atende-los,estamos em manutenção no momento.";
            retorno.Sucesso = false;
            NewRelicIgnoreTransaction();
            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
        }

        [Route("clientes/senha/envia_link")]
        [HttpPost]
        public RetornoModel PostEnviarLink([FromBody]ClienteEnviarLinkaRequestModel cliente)
        {
            RetornoModel retorno = new RetornoModel();
            if (cliente != null && !string.IsNullOrWhiteSpace(cliente.email) && !string.IsNullOrWhiteSpace(cliente.url))
            {
                try
                {
                    using (var ado = new MasterADOBase())
                    {
                        ClienteBO clienteBO = new ClienteBO(SessionModel.GetSiteId(), ado);
                        var retornoLogin = (!string.IsNullOrWhiteSpace(cliente.nome) && !string.IsNullOrWhiteSpace(cliente.cpf))
                                                    ? clienteBO.EnviarEmailLinkRedefinirSenha(cliente.nome, cliente.cpf.Replace(new string[] { ".", "-" }, ""), cliente.email, cliente.url)
                                                    : clienteBO.EnviarEmailLinkRedefinirSenha(cliente.email, cliente.url);
                        retorno.Mensagem = retornoLogin.Mensagem;
                        retorno.Sucesso = retornoLogin.Sucesso;
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(ex);
                    retorno = new RetornoModel();
                    retorno.Mensagem = ex.Message;
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError, ParseRetorno(retorno)));
                }

                if (!retorno.Sucesso)
                {
                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, ParseRetorno(retorno)));
                }
                else
                {
                    return ParseRetorno(retorno);
                }
            }
            else
            {
                retorno.Mensagem = "Objeto não encontrado ou em formato inválido";
                NewRelicIgnoreTransaction();
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(retorno)));
            }
        }

        private void SalvarEnderecoTemp(tCliente cliente, bool verificarSeEnderecoExiste = true)
        {
            //Endereco temporário
            if (SessionModel.EnderecoTemp == null || cliente == null)
            {
                return;
            }

            LogUtil.Info(string.Format("##Salvar.EntregaTemporaria.Logado## SESSION {0}, CLIENTE {1}", this.SessionModel.SessionID, cliente.ID));

            var clienteBO = new ClienteBO();
            tClienteEndereco endereco = null;

            //Se verificar se endereço já existe
            if (verificarSeEnderecoExiste)
            {
                endereco = clienteBO.ConsultarEndereco(SessionModel.EnderecoTemp.CEP, SessionModel.EnderecoTemp.Endereco, SessionModel.EnderecoTemp.Numero, SessionModel.EnderecoTemp.Complemento, cliente.ID);
            }

            if (endereco != null) //Se já existe um endereco com os dados atribui o id para a SessionModel 
            {
                LogUtil.Info(string.Format("##Salvar.EntregaTemporaria.EnderecoEncontrado## SESSION {0}, CLIENTE {1}, ENDERECO {2}", this.SessionModel.SessionID, cliente.ID, endereco.ID));

                SessionModel.ClienteEnderecoID = endereco.ID;
                SessionModel.EnderecoTemp = null;
            }
            else //Senao Salva o endereco.
            {
                try
                {
                    endereco = new tClienteEndereco()
                    {
                        CEP = SessionModel.EnderecoTemp.CEP,
                        Endereco = SessionModel.EnderecoTemp.Endereco,
                        Numero = SessionModel.EnderecoTemp.Numero,
                        Cidade = SessionModel.EnderecoTemp.Cidade,
                        Estado = SessionModel.EnderecoTemp.Estado,
                        Complemento = SessionModel.EnderecoTemp.Complemento,
                        Bairro = SessionModel.EnderecoTemp.Bairro,
                        Nome = cliente.Nome,
                        CPF = cliente.CPF,
                        RG = cliente.RG,
                        ClienteID = cliente.ID,
                        EnderecoTipoID = 1,
                        EnderecoPrincipal = "F",
                        StatusConsulta = 0
                    };

                    LogUtil.Info(string.Format("##Salvar.EntregaTemporaria.NovoEndereco## SESSION {0}, CLIENTE {1}, ENDERECO {2}", this.SessionModel.SessionID, SessionModel.ClienteID, endereco.Endereco));

                    //Inserir endereço cliente
                    var enderecoId = clienteBO.InserirEndereco(endereco);
                    if (enderecoId > 0)
                    {
                        LogUtil.Info(string.Format("##Salvar.EntregaTemporaria.NovoEndereco.SUCCESS## SESSION {0}, CLIENTE {1}, ENTREGACONTROLE {2}, ENDERECO {3}", this.SessionModel.SessionID, this.SessionModel.ClienteID, this.SessionModel.EntregaControleID, enderecoId));

                        SessionModel.ClienteEnderecoID = enderecoId;
                        SessionModel.EnderecoTemp = null;
                    }
                    else
                    {
                        LogUtil.Info(string.Format("##Post.EntregaTemporaria.ERROR## SESSION {0}, CLIENTE {1}, ENTREGACONTROLE {2}, ENDERECO {3}, MSG {4}", this.SessionModel.SessionID, cliente.ID, this.SessionModel.EntregaControleID, this.SessionModel.EnderecoTemp.Endereco, "Endereço não cadastrado"));

                        SessionModel.EntregaControleID = enderecoId;
                        SessionModel.EnderecoTemp = null;

                        NewRelicIgnoreTransaction();
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(new RetornoModel { Mensagem = "Endereço não cadastrado, tente novamente.", Sucesso = false })));
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Error(string.Format("##Post.EntregaTemporaria.NovoEndereço.EXCEPTION## SESSION {0}, CLIENTE {1}, MSG {2}", this.SessionModel.SessionID, this.SessionModel.ClienteID, ex.Message), ex);

                    SessionModel.EntregaControleID = 0;
                    SessionModel.EnderecoTemp = null;

                    NewRelicIgnoreTransaction();
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ParseRetorno(new RetornoModel { Mensagem = "Não foi possivel cadastrar o endereço, tente novamente.", Sucesso = false })));
                }

            }
        }

    }
}