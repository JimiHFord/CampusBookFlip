using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;
using CampusBookFlip.WebUI.Infrastructure;
using System.Data.Entity;
using CampusBookFlip.WebUI.Models;
using CampusBookFlip.Domain.Concrete;
using TwitterBootstrapMVC;


namespace CampusBookFlip.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Constants.SeedAdmins();
            //Bootstrap.Configure();
            AreaRegistration.RegisterAllAreas();
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}