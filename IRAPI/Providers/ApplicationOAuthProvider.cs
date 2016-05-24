using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using IRAPI.Models;
using IRCore.DataAccess.Model;
using IRCore.BusinessObject;
using IRCore.DataAccess.Model.Enumerator;
using Newtonsoft.Json;
using IRCore.Util;
using System.Web;

namespace IRAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            try
            {
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                using (var usuarioBO = new APIUsuarioBO())
                {
                    APIUsuario usuario = usuarioBO.Consultar(context.UserName);

                    if (usuario == null)
                    {
						NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                        context.SetError("Mensagem", "Usuário ou senha incorretos");
                        return;
                    }
                    if (!usuario.Ativo)
                    {
						NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                        context.SetError("Mensagem", "Usuário está inativo");
                        return;
                    }
                    switch (usuario.TipoAcessoAsEnum)
                    {
                        case enumAPITipoAcesso.webClient:
                            if (string.IsNullOrEmpty(usuario.DominiosOrigem))
                            {
								NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                                context.SetError("Mensagem", "Usuário mal cadastrado");
                                return;
                            }
                            if (usuario.DominiosOrigem != "*")
                            {

                                var dominios = usuario.DominiosOrigem.Split(',').Select(t => t.Trim());
                                if (!dominios.Contains(HttpContext.Current.Request.GetClientReferer()))
                                {
									NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                                    context.SetError("Mensagem", "O Domínio " + HttpContext.Current.Request.GetClientReferer() + " não permitido para o acesso a API com este usuário");
                                    return;
                                }
                            }

                            if (usuario.Senha != context.Password)
                            {
								NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                                context.SetError("Mensagem", "Usuário ou senha incorretos");
                                return;
                            }
                            break;
                        case enumAPITipoAcesso.webServer:
                            if (string.IsNullOrEmpty(usuario.IPOrigem))
                            {
								NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                                context.SetError("Mensagem", "Usuário mal cadastrado");
                                return;
                            }
                            if (usuario.IPOrigem != "*")
                            {
                                // TODO: Tem algum problema, deve ser checado
                                var ips = usuario.IPOrigem.Split(',').Select(t => t.Trim());
                                if (!ips.Any(t => HttpContext.Current.Request.GetClientIPAddress().Contains(t)))
                                {
									NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                                    context.SetError("Mensagem", "Os IPs " + string.Join(", ", HttpContext.Current.Request.GetClientIPAddress().ToArray()) + " não são permitidos para acesso a API com este usuário");
                                    return;
                                }
                            }
                            if (userManager.PasswordHasher.VerifyHashedPassword(usuario.Senha, context.Password) == PasswordVerificationResult.Failed)
                            {
								NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                                context.SetError("Mensagem", "Usuário ou senha incorretos");
                                return;
                            }
                            break;
                        case enumAPITipoAcesso.app:
                            if (userManager.PasswordHasher.VerifyHashedPassword(usuario.Senha, context.Password) == PasswordVerificationResult.Failed)
                            {
								NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                                context.SetError("Mensagem", "Usuário ou senha incorretos");
                                return;
                            }
                            break;
                    }

                    var token = new APIUsuarioToken
                    {
                        APIUsuario = usuario,
                        APIUsuarioID = usuario.ID,
                        DadosIndentificacao = context.Request.Headers["ClientInfo"],
                        Ativo = true,
                        DadosSession = JsonConvert.SerializeObject(new DadosSessionModel
                        {
                            SessionID = Guid.NewGuid().ToString(),
                            CanalID = usuario.CanalID,
                            LojaID = usuario.LojaID,
                            UsuarioID = usuario.UsuarioID,
                            SiteID = usuario.SiteID,
                        }),
                        DataExpiracao = DateTime.Now.Add(TimeSpan.FromHours(usuario.HorasExpiracaoToken))
                    };

                    usuarioBO.Salvar(token, true);

                    context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromHours(usuario.HorasExpiracaoToken);
                    ClaimsIdentity oAuthIdentity = await usuarioBO.CriarIdentityAsync(usuario, userManager.UserTokenProvider);

                    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, CreateProperties(usuario));
                    context.Validated(ticket);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }


        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            try
            {
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                using (var usuarioBO = new APIUsuarioBO())
                {
                    APIUsuario usuario = usuarioBO.Consultar(context.Ticket.Properties.Dictionary["userName"]);

                    if (usuario == null)
                    {
						NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
                        context.SetError("Mensagem", "Usuário ou senha incorretos");
                        return;
                    }

                    context.Options.AccessTokenExpireTimeSpan = TimeSpan.FromHours(usuario.HorasExpiracaoToken);
                    ClaimsIdentity oAuthIdentity = await usuarioBO.CriarIdentityAsync(usuario, userManager.UserTokenProvider);

                    AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, CreateProperties(usuario));
                    context.Validated(ticket);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex);
            }

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(APIUsuario usuario)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", usuario.Login }
            };
            return new AuthenticationProperties(data);
        }
    }
}