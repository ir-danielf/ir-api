using IRCore.BusinessObject.Identity;
using IRCore.DataAccess.Model;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System;
using IRCore.BusinessObject.Identity.Models;

namespace IRCore.BusinessObject
{

    public static class ClienteBOExtensions
    {

        public static ClaimsIdentity CriarIdentity(this ClienteBO clienteBO, Login login)
        {
            var _UserManager = new UserManager<ClienteIdentity>(new ClienteUserStore(login.SiteID, clienteBO));
            _UserManager.PasswordHasher = new ClientePasswordHasher();
            return _UserManager.CreateIdentity(login.CopyTo(new ClienteIdentity()), DefaultAuthenticationTypes.ApplicationCookie);
        }

    }
}