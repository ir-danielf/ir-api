using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using IRAPI.Providers;
using IRAPI.Models;
using System.Web.Http;
using System.Web.Http.Cors;
using IRCore.Util;
using IRCore.Util.Enumerator;

namespace IRAPI
{
    /// <summary>
    /// Inicia o processo de autênticação
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Opções de aut~enticação
        /// </summary>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /// <summary>
        /// ID publico do Cliente
        /// </summary>
        public static string PublicClientId { get; private set; }

        /// <summary>
        /// For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureAuth(IAppBuilder app)
        {

            // Habilita o cross domain
            app.Use(async (context, next) =>
            {
                IOwinRequest req = context.Request;
                IOwinResponse res = context.Response;
                if (req.Path.StartsWithSegments(new PathString("/token")))
                {
                    res.Headers.Set("Access-Control-Allow-Origin", "*");
                    res.Headers.Set("Access-Control-Allow-Methods", "*");
                    
                    if (req.Method == "OPTIONS")
                    {
                        res.Headers.Set("Access-Control-Allow-Headers", req.Headers["Access-Control-Request-Headers"]);
                        res.StatusCode = 200;
                        return;
                    }
                }
                await next();
            });

            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = (!ConfiguracaoAppUtil.GetAsBool(enumConfiguracaoGeral.httpsHabilitado)),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };
         
            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
