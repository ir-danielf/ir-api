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
    public class UsuarioUserStore : IUserStore<UsuarioIdentity>,
                             IUserPasswordStore<UsuarioIdentity>
    {
        

        private UsuarioBO bo;

        public UsuarioUserStore(UsuarioBO usuarioBO = null)
        {
            if (usuarioBO == null)
            {
                bo = new UsuarioBO();
            }
            else
            {
                bo = usuarioBO;
            }
        }
        
        /// <summary>
        /// Insert a new IdentityUser in the UserTable
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(UsuarioIdentity user)
        {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            bo.Salvar(user.CopyTo(new tUsuario()), user.ID);
            
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns an IdentityUser instance based on a userId query 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<UsuarioIdentity> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            UsuarioIdentity result = bo.Consultar(Convert.ToInt32(userId)).CopyTo(new UsuarioIdentity());
            
            return Task.FromResult<UsuarioIdentity>(result);
        }

        /// <summary>
        /// Returns an IdentityUser instance based on a userName query 
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<UsuarioIdentity> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            UsuarioIdentity result = bo.Consultar(userName).CopyTo(new UsuarioIdentity());

            return Task.FromResult<UsuarioIdentity>(result);
        }

        /// <summary>
        /// Updates the UsersTable with the IdentityUser instance values
        /// </summary>
        /// <param name="user">IdentityUser to be updated</param>
        /// <returns></returns>
        public Task UpdateAsync(UsuarioIdentity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var usuario = bo.Consultar(user.ID);
            usuario.CopyFrom(user);
            bo.Salvar(usuario, user.ID);
            
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
        public Task DeleteAsync(UsuarioIdentity user)
        {
            if (user != null)
            {
                var usuario = bo.Consultar(user.ID);
                usuario.StatusAsEnum = enumUsuarioStatus.bloqueado;
                bo.Salvar(usuario, usuario.ID);
            }

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Returns the PasswordHash for a given IdentityUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(UsuarioIdentity user)
        {
            string passwordHash = user.Senha;
            return Task.FromResult<string>(passwordHash);
        }

        /// <summary>
        /// Verifies if user has password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(UsuarioIdentity user)
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
        public Task SetPasswordHashAsync(UsuarioIdentity user, string passwordHash)
        {
            user.Senha = passwordHash;

            return Task.FromResult<Object>(null);
        }
    }
}
