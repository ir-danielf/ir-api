using System.Web.Mvc;
using System.Web.Routing;

namespace IRCore.Util.AreaRegistration
{
    public abstract class MultiPortaisAreaRegistration : System.Web.Mvc.AreaRegistration
    {
        public abstract string PrincipalNamespace { get; }
        public abstract string ParameterAreaName { get; }
        public abstract object RouteDefault { get; }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            Route route = null;

            RouteValueDictionary routeValue = new RouteValueDictionary(RouteDefault);
            routeValue[ParameterAreaName] = AreaName;

            route = new Route("{lang}/" + AreaName + "/{controller}/{action}/{id}", new MvcRouteHandler()) { Defaults = routeValue };
            route.DataTokens = new RouteValueDictionary();
            route.DataTokens["Namespaces"] = new string[] { PrincipalNamespace + ".Areas." + AreaName + ".Controllers" };
            route.DataTokens["area"] = AreaName;
            route.Constraints = new RouteValueDictionary();
            route.Constraints["lang"] = @"[a-z]{2,3}(?:-[A-Z]{2,3})?";
            context.Routes.Add(AreaName + "_lang1", route);


            route = new Route("{lang}/" + AreaName + "/{controller}/{action}/{id}", new MvcRouteHandler()) { Defaults = routeValue };
            route.DataTokens = new RouteValueDictionary();
            route.DataTokens["Namespaces"] = new string[] { PrincipalNamespace + ".Controllers" };
            route.DataTokens["area"] = AreaName;
            route.Constraints = new RouteValueDictionary(); 
            route.Constraints["lang"] = @"[a-z]{2,3}(?:-[A-Z]{2,3})?";
            context.Routes.Add(AreaName + "_lang2", route);


            route = new Route(AreaName + "/{controller}/{action}/{id}", new MvcRouteHandler()){ Defaults = routeValue };
            route.DataTokens = new RouteValueDictionary();
            route.DataTokens["Namespaces"] = new string[] { PrincipalNamespace + ".Areas." + AreaName + ".Controllers" };
            route.DataTokens["area"] = AreaName;
            context.Routes.Add(AreaName + "_default1", route);


            route = new Route(AreaName + "/{controller}/{action}/{id}", new MvcRouteHandler()) { Defaults = routeValue };
            route.DataTokens = new RouteValueDictionary();
            route.DataTokens["Namespaces"] = new string[] { PrincipalNamespace + ".Controllers" };
            route.DataTokens["area"] = AreaName;
            context.Routes.Add(AreaName + "_default2", route);

        }
    }
}