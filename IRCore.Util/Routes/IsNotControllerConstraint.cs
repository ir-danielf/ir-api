using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace IRCore.Util.Routes
{
    public class IsNotControllerConstraint : IRouteConstraint
    {
        private string[] NameSpaces;
        private Assembly ExecutingAssembly;

        public IsNotControllerConstraint(Assembly executingAssembly, string[] nameSpaces = null)
        {
            NameSpaces = nameSpaces;
            ExecutingAssembly = executingAssembly;
        }

 
        public bool Match
            (
                HttpContextBase httpContext,
                Route route,
                string parameterName,
                RouteValueDictionary values,
                RouteDirection routeDirection
            )
        {
            
            if((NameSpaces == null) && (route.DataTokens.ContainsKey("Namespaces")) && (route.DataTokens["Namespaces"]!=null))
            {
                NameSpaces = (string[])route.DataTokens["Namespaces"];
            }
            if((NameSpaces == null) || (NameSpaces.Length == 0))
            {
                return false;
            }
            else
            {
                var controllers = ExecutingAssembly.GetTypes()
                                .Where(t => t.IsClass && NameSpaces.Contains(t.Namespace))
                                .Select(t => t.Name.ToLower());
                return (!controllers.Contains(values[parameterName].ToString().ToLower() + "controller"));    
            }
            
            
        }
    }
}
