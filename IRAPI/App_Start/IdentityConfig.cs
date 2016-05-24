using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using IRCore.BusinessObject.Identity.Models;
using IRCore.BusinessObject.Identity;

namespace IRAPI
{
    /// <summary>
    /// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application. 
    /// </summary>
    public class ApplicationUserManager : UserManager<APIUsuarioIdentity>
    {
        /// <summary>
        /// Contrutor da Classe que precisa receber um objeto para salvar usuário
        /// </summary>
        /// <param name="store"></param>
        public ApplicationUserManager(APIUsuarioUserStore store)
            : base(store)
        {
        }

        /// <summary>
        /// Cria um usuário
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new APIUsuarioUserStore());
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<APIUsuarioIdentity>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
            
        }
    }
}
