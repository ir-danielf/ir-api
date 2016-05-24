using IRCore.BusinessObject.Enumerator;
using IRCore.Util;
using IRCore.Util.Enumerator;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCore.BusinessObject.Identity
{
    public class ClientePasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return ClienteBO.Criptografar(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (ClienteBO.VerfificarCriptografia(hashedPassword, providedPassword))
                return PasswordVerificationResult.Success;
            else return PasswordVerificationResult.Failed;
        }
    }
}
