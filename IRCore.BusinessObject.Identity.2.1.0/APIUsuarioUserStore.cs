using IRCore.BusinessObject.Identity.Models;
using IRCore.DataAccess.Model;
using IRCore.DataAccess.Model.Enumerator;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Identity
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity user store iterfaces
    /// </summary>
    public class APIUsuarioUserStore : IUserStore<APIUsuarioIdentity>, 
                             IUserPasswordStore<APIUsuarioIdentity>
    {
        

        public APIUsuarioBO APIUsuarioBO;

        public APIUsuarioUserStore(APIUsuarioBO apiUsuarioBO = null)
        {
            APIUsuarioBO = apiUsuarioBO;
        }
        
        /// <summary>
        /// Insert a new IdentityUser in the UserTable
        /// </summary>
        /// <param name="user"></param>F
        /// <returns></returns>
        public Task CreateAsync(APIUsuarioIdentity user)
        {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            APIUsuarioBO.Salvar(user.CopyTo(new APIUsuario()));
            
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns an IdentityUser instance based on a userId query 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<APIUsuarioIdentity> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }
            APIUsuarioIdentity result = null;
            var usuario = APIUsuarioBO.Consultar(Convert.ToInt32(userId));
            if (usuario != null)
            {
                result = usuario.CopyTo(new APIUsuarioIdentity());
            }

            return Task.FromResult<APIUsuarioIdentity>(result);
        }

        /// <summary>
        /// Returns an IdentityUser instance based on a userName query 
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<APIUsuarioIdentity> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            APIUsuarioIdentity result = null;
            var usuario = APIUsuarioBO.Consultar(userName);
            if (usuario!=null)
            {
                result = usuario.CopyTo(new APIUsuarioIdentity());
            }
                

            return Task.FromResult<APIUsuarioIdentity>(result);
        }

        /// <summary>
        /// Updates the UsersTable with the IdentityUser instance values
        /// </summary>
        /// <param name="user">IdentityUser to be updated</param>
        /// <returns></returns>
        public Task UpdateAsync(APIUsuarioIdentity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var usuario = APIUsuarioBO.Consultar(user.ID);
            usuario.CopyFrom(user);
            APIUsuarioBO.Salvar(usuario);
            
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(APIUsuarioIdentity user)
        {
            if (user != null)
            {
                var usuario = APIUsuarioBO.Consultar(user.ID);
                usuario.Ativo = false;
                APIUsuarioBO.Salvar(usuario);
            }

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Returns the PasswordHash for a given IdentityUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(APIUsuarioIdentity user)
        {
            string passwordHash = user.Senha;
            return Task.FromResult<string>(passwordHash);
        }

        /// <summary>
        /// Verifies if user has password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(APIUsuarioIdentity user)
        {
            var hasPassword = !string.IsNullOrEmpty(user.Senha);

            return Task.FromResult<bool>(hasPassword);
        }

        /// <summary>
        /// Sets the password hash for a given IdentityUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(APIUsuarioIdentity user, string passwordHash)
        {
            user.Senha = passwordHash;

            return Task.FromResult<Object>(null);
        }
    }
}
