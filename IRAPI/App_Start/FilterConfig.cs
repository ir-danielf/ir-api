using System.Web;
using System.Web.Mvc;

namespace IRAPI
{
    /// <summary>
    /// Filtra Configuração da API
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registra os FIltros
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
