using IRCore.Util.Enumerator;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Identity
{
    public class UsuarioPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return UsuarioBO.Criptografar(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (UsuarioBO.VerfificarCriptografia(hashedPassword, providedPassword))
                return PasswordVerificationResult.Success;
            else return PasswordVerificationResult.Failed;
        }
    }
}
