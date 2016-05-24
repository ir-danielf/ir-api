using IRCore.DataAccess.Model;
using Microsoft.AspNet.Identity;
using System;

namespace IRCore.BusinessObject.Identity.Models
{
    /// <summary>
    /// Class that implements the ASP.NET Identity
    /// IUser interface 
    /// </summary>
    public class ClienteIdentity : Login, IUser
    {
        /// <summary>
        /// Default constructor 
        /// </summary>
        public ClienteIdentity()
        {
        }

        /// <summary>
        /// Constructor that takes user name as argument
        /// </summary>
        /// <param name="userName"></param>
        public ClienteIdentity(string userName)
            : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// User ID
        /// </summary>
        public string Id {
            get { 
                return this.ClienteID.ToString(); 
            }
            set {   } 
        }

        /// <summary>
        /// User's name
        /// </summary>
        public string UserName {
            get { return (string.IsNullOrEmpty(Email)?CPF:Email); }
            set { this.Email = value; }
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
