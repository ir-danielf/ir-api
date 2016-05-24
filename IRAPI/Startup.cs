using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IRAPI.Startup))]

namespace IRAPI
{
    public partial class Startup
    {
        /// <summary>
        /// Adiciona configuração na classe de inicio da configuração de autênticação
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {			
            ConfigureAuth(app);

			// Configuração para WebAPI.
			var config = new HttpConfiguration();
			WebApiConfig.Register(config);
			app.UseWebApi(config);   
        }
    }
}
