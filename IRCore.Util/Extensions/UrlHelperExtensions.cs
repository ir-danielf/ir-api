using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Web;
using System.Web.Routing;
using IRCore.Util.Enumerator;
using IRCore.Util;

namespace System.Web.Mvc
{
    
    public static class UrlHelperExtensions
    {

        public static string ActionAbsoluteUri(this UrlHelper Url, string actionName, string controllerName, string protocol, object routeValues = null)
        {
            if((protocol=="https")&&(!ConfiguracaoAppUtil.GetAsBool(enumConfiguracaoGeral.httpsHabilitado)))
            {
                protocol = "http";
            }
            return Url.Action(actionName, controllerName, routeValues, protocol);
        }


    }
}
