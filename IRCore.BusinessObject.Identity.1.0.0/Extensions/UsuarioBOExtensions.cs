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
    public static class UsuarioBOExtensions
    {

        public static ClaimsIdentity CriarIdentity(this UsuarioBO usuarioBO, tUsuario usuario)
        {
            var userManager = new UserManager<UsuarioIdentity>(new UsuarioUserStore(usuarioBO));
            userManager.PasswordHasher = new UsuarioPasswordHasher();
            return userManager.CreateIdentity(usuario.CopyTo(new UsuarioIdentity()), DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}
