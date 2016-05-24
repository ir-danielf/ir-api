using IRCore.BusinessObject.Enumerator;
using IRCore.BusinessObject.Estrutura;
using IRCore.BusinessObject.Identity;
using IRCore.BusinessObject.Identity.Models;
using IRCore.BusinessObject.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.ADO.Estrutura;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IRCore.BusinessObject
{
    public static class APIUsuarioBOExtensions
    {
        public static ClaimsIdentity CriarIdentity(this APIUsuarioBO apiUsuarioBO, APIUsuario usuario, IUserTokenProvider<APIUsuarioIdentity, string> UserTokenProvider)
        {
            UserManager<APIUsuarioIdentity> userManager = new UserManager<APIUsuarioIdentity>(new APIUsuarioUserStore(apiUsuarioBO));
            userManager.UserTokenProvider = UserTokenProvider;
            var user = usuario.CopyTo(new APIUsuarioIdentity());
            var identity = userManager.CreateIdentity(user, OAuthDefaults.AuthenticationType);
            return identity;
        }
        public static async Task<ClaimsIdentity> CriarIdentityAsync(this APIUsuarioBO apiUsuarioBO, APIUsuario usuario, IUserTokenProvider<APIUsuarioIdentity, string> UserTokenProvider)
        {
            UserManager<APIUsuarioIdentity> userManager = new UserManager<APIUsuarioIdentity>(new APIUsuarioUserStore(apiUsuarioBO));
            userManager.UserTokenProvider = UserTokenProvider;
            var user = usuario.CopyTo(new APIUsuarioIdentity());
            var identity = await userManager.CreateIdentityAsync(user, OAuthDefaults.AuthenticationType);
            return identity;
        }
    }
}
