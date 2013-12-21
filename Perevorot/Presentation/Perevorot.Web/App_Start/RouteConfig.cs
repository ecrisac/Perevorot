using System.Web.Mvc;
using System.Web.Routing;

namespace Perevorot.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{*favicon}", new {favicon = @"(.*/)?favicon.ico(/.*)?"});

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //TODO: Remove wrong route, the application should go by default to the Login Page.
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Login",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Get", id = UrlParameter.Optional }
                );
        }
    }
}