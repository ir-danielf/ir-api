using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using IRAPI.Util;
using IRCore.Util;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression;
using Microsoft.AspNet.WebApi.MessageHandlers.Compression.Compressors;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IRAPI
{
    /// <summary>
    ///     Configuração da API
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        ///     Registra a configuração da api
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            LogUtil.Debug(string.Format("##WebApiConfig.Register##"));

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional }
                );

            // REMOVE TODOS (FORM, XML E JSON)
            config.Formatters.Clear();

            // ADICIONA APENAS O JSON
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            config.Formatters.JsonFormatter.SerializerSettings.Error += (sender, e) =>
            {
                LogUtil.Debug(string.Format("##WebApiConfig.Register.ERROR## MSG {0}", e.ErrorContext.Error));

                //LogUtil.Error("Erro na Geração do JSON");
                //LogUtil.Error(e.ErrorContext.Error);

                LogHttpUtil.LogRequest();

                e.ErrorContext.Handled = true;
            };

            config.MessageHandlers.Insert(0, new ServerCompressionHandler(new GZipCompressor(), new DeflateCompressor()));
        }
    }
}