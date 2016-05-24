using IRCore.BusinessObject.Identity.Models;
using IRCore.DataAccess.ADO;
using IRCore.DataAccess.Model;
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
    public class ClienteUserStore : IUserStore<ClienteIdentity>, 
                            IUserPasswordStore<ClienteIdentity>
    {

        private ClienteBO bo;

        public ClienteUserStore(int siteId, ClienteBO clienteBO = null)
        {
            bo = clienteBO ?? new ClienteBO(siteId);
        }

        public ClienteUserStore(ClienteBO clienteBO = null)
        {
            bo = clienteBO ?? new ClienteBO(1);
        }

        /// <summary>
        /// Insert a new IdentityUser in the UserTable
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(ClienteIdentity user)
        {
            if (user == null) {
                throw new ArgumentNullException("user");
            }

            bo.Salvar(user.CopyTo(new Login()));
            
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns an IdentityUser instance based on a userId query 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<ClienteIdentity> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            ClienteIdentity result = bo.Consultar(Convert.ToInt32(userId)).CopyTo(new ClienteIdentity());
            
            return Task.FromResult<ClienteIdentity>(result);
        }

        /// <summary>
        /// Returns an IdentityUser instance based on a userName query 
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<ClienteIdentity> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            ClienteIdentity result = bo.ConsultarUsername(userName).CopyTo(new ClienteIdentity());

            return Task.FromResult<ClienteIdentity>(result);
        }

        /// <summary>
        /// Updates the UsersTable with the IdentityUser instance values
        /// </summary>
        /// <param name="user">IdentityUser to be updated</param>
        /// <returns></returns>
        public Task UpdateAsync(ClienteIdentity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var login = bo.Consultar(user.ID);
            login.CopyFrom(user);
            login.Cliente.CopyFrom(user);
            bo.Salvar(login);
            
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
        public Task DeleteAsync(ClienteIdentity user)
        {
            if (user != null)
            {
                var login = bo.Consultar(user.ID);
                login.AtivoAsBool = false;
                login.Cliente.AtivoAsBool = false;
                bo.Salvar(login);
            
            }

            return Task.FromResult<Object>(null);
        }

        /// <summary>
        /// Returns the PasswordHash for a given IdentityUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(ClienteIdentity user)
        {
            string passwordHash = user.Senha;
            return Task.FromResult<string>(passwordHash);
        }

        /// <summary>
        /// Verifies if user has password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(ClienteIdentity user)
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
        public Task SetPasswordHashAsync(ClienteIdentity user, string passwordHash)
        {
            user.Senha = passwordHash;

            return Task.FromResult<Object>(null);
        }

    }
}
