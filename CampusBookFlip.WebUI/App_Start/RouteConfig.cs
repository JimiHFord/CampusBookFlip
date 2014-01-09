using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseDashedRouting;

namespace CampusBookFlip.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.LowercaseUrls = true;
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "ResetPassword",
                url: "account/reset-password/{id}",
                defaults: new { controller = "Account", action = "ResetPassword" }
            );
            routes.Add(new LowercaseDashedRoute(url: "{controller}/{action}/{id}",
                defaults: new RouteValueDictionary(
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }),
                routeHandler: new DashedRouteHandler()));
        }
    }
}