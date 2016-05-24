using IRCore.DataAccess.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Identity.Models
{
    /// <summary>
    /// Class that implements the ASP.NET Identity
    /// IUser interface 
    /// </summary>
    public partial class APIUsuarioIdentity : APIUsuario, IUser
    {
        /// <summary>
        /// Default constructor 
        /// </summary>
        public APIUsuarioIdentity()
        {
        }

        /// <summary>
        /// Constructor that takes user name as argument
        /// </summary>
        /// <param name="userName"></param>
        public APIUsuarioIdentity(string userName)
            : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// User ID
        /// </summary>
        public string Id
        {
            get { return this.ID.ToString(); }
            set { this.ID = Convert.ToInt32(value); }
        }

        /// <summary>
        /// User's name
        /// </summary>
        public string UserName {
            get { return this.Login; }
            set { this.Login = value; }
        }

        /// <summary>
        /// User's password hash
        /// </summary>
        public string PasswordHash {
            get { return this.Senha; }
            set { this.Senha = value;  } 
        }

        /// <summary>
        /// User's security stamp
        /// </summary>
        public string SecurityStamp { get; set; }

    }
}
