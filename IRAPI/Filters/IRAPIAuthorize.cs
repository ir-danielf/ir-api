using IRAPI.Controllers;
using IRAPI.Models;
using IRCore.BusinessObject;
using IRCore.BusinessObject.Enumerator;
using IRCore.DataAccess.Model.Enumerator;
using IRCore.Util;
using IRCore.Util.Enumerator;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

public class IRAPIAuthorize : AuthorizeAttribute
{
    private enumAPIRele[] _apiRoles { get; set; }

    public IRAPIAuthorize() { }

    public IRAPIAuthorize(params enumAPIRele[] apiRoles)
    {
        _apiRoles = apiRoles;
    }

    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
        try
        {
            if ((ConfiguracaoAppUtil.GetAsBool(enumConfiguracaoGeral.httpsHabilitado)) && (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps))
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required"
                };
            }

            if (base.IsAuthorized(actionContext))
            {
                var controller = (MasterApiController)actionContext.ControllerContext.Controller;

                IPrincipal principal = actionContext.ControllerContext.RequestContext.Principal;
                if ((principal.Identity != null) && (principal.Identity.IsAuthenticated))
                {

                    using (var usuarioBO = new APIUsuarioBO())
                    {
                        if (controller.APIUsuarioToken == null)
                        {
                            controller.APIUsuarioToken = usuarioBO.ConsultarToken(actionContext.ControllerContext.Request.Headers.Authorization.Parameter);
                            if (controller.APIUsuarioToken == null)
                            {
                                controller.APIUsuarioToken = usuarioBO.ConsultarToken(actionContext.ControllerContext.Request.Headers.GetValues("ClientInfo").FirstOrDefault(), principal.Identity.Name);
                                if ((controller.APIUsuarioToken == null) || (controller.APIUsuarioToken.Token != null))
                                {
                                    return false;
                                }

                                controller.APIUsuarioToken.Token = actionContext.ControllerContext.Request.Headers.Authorization.Parameter;
                                controller.AtualizarToken = true;
                            }
                        }


                        if ((_apiRoles != null) && (_apiRoles.Length > 0) && (controller.APIUsuarioToken.Roles == null))
                        {
                            controller.APIUsuarioToken.Roles = usuarioBO.ConsultarPermissoes(controller.APIUsuarioToken.APIUsuario.ID);
                        }

                    }
                    
					controller.SessionModel = JsonConvert.DeserializeObject<SessionModel>(controller.APIUsuarioToken.DadosSession);
                    if ((!controller.APIUsuarioToken.APIUsuario.Ativo) || (!controller.APIUsuarioToken.Ativo) || (controller.APIUsuarioToken.DadosIndentificacao != actionContext.ControllerContext.Request.Headers.GetValues("ClientInfo").FirstOrDefault()))
                    {
                        return false;
                    }

                    // TODO: Tem algum problema, deve ser checado
                    switch (controller.APIUsuarioToken.APIUsuario.TipoAcessoAsEnum)
                    {
                        case enumAPITipoAcesso.webClient:

                            if (string.IsNullOrEmpty(controller.APIUsuarioToken.APIUsuario.DominiosOrigem))
                            {
                                return false;
                            }
                            if (controller.APIUsuarioToken.APIUsuario.DominiosOrigem != "*")
                            {
                                var dominiosWC = controller.APIUsuarioToken.APIUsuario.DominiosOrigem.Split(',').Select(t => t.Trim());

                                if (!dominiosWC.Contains(HttpContext.Current.Request.GetClientReferer()))
                                {
                                    return false;
                                }
                            }
                                

                            break;
                        case enumAPITipoAcesso.webServer:
                            if (string.IsNullOrEmpty(controller.APIUsuarioToken.APIUsuario.IPOrigem))
                            {
                                return false;
                            }
                            if (controller.APIUsuarioToken.APIUsuario.IPOrigem != "*")
                            {
                                var ips = controller.APIUsuarioToken.APIUsuario.IPOrigem.Split(',').Select(t => t.Trim());
                                if (!ips.Any(t => HttpContext.Current.Request.GetClientIPAddress().Contains(t)))
                                {
                                    if (HttpContext.Current.Request.UrlReferrer == null)
                                    {
                                        LogUtil.Error("Os IPs " + string.Join(", ", HttpContext.Current.Request.GetClientIPAddress().ToArray()) + " não são permitidos para acesso a API com o usuário " + controller.APIUsuarioToken.APIUsuario.Login);
                                        return false;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(controller.APIUsuarioToken.APIUsuario.DominiosOrigem))
                                        {
                                            LogUtil.Error("Os IPs " + string.Join(", ", HttpContext.Current.Request.GetClientIPAddress().ToArray()) + " não são permitidos para acesso a API com o usuário " + controller.APIUsuarioToken.APIUsuario.Login);
                                            return false;
                                        }
                                        if (controller.APIUsuarioToken.APIUsuario.DominiosOrigem != "*")
                                        {
                                            var dominiosWS = controller.APIUsuarioToken.APIUsuario.DominiosOrigem.Split(',').Select(t => t.Trim());

                                            if (!dominiosWS.Contains(HttpContext.Current.Request.GetClientReferer()))
                                            {
                                                LogUtil.Error("O Domínio " + HttpContext.Current.Request.GetClientReferer() + " não permitido para o acesso a API  com o usuário " + controller.APIUsuarioToken.APIUsuario.Login);
                                                return false;
                                            }

                                        }
                                    }
                                }
                            }
                            break;
                    }

                    if ((_apiRoles != null) && (_apiRoles.Length > 0))
                    {
                        if ((!controller.APIUsuarioToken.Roles.Select(t => t.ToLower()).Contains(enumAPIRele.master.Description().ToLower())) && (!_apiRoles.Any(t => controller.APIUsuarioToken.Roles.Any(y => t.Description().ToLower().Split(',').Contains(y.ToLower())))))
                        {
                            if (controller.ControllerContext.RouteData.Values.ContainsKey("action"))
                            {
                                LogUtil.Error("O Usuario da API " + controller.APIUsuarioToken.APIUsuario.Login + " não tem permissão para o acesso ao metodo " + controller.ControllerContext.RouteData.Values["action"] + " ");
                            }
                            else
                            {
                                LogUtil.Error("O Usuario da API " + controller.APIUsuarioToken.APIUsuario.Login + " não tem permissão para o acesso.");
                            }
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }
        catch(Exception ex)
        {
            LogUtil.Error(ex);
            return false;
        }            
    }
}