using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CampusBookFlip.Domain.Concrete;
using CampusBookFlip.Domain.Abstract;
using CampusBookFlip.WebUI.Infrastructure;
using System.Web.Http;
using Postal;

namespace CampusBookFlip.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
            RegisterAPI();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IRepository>().To<EFRepository>();
            ninjectKernel.Bind<IEmailService>().To<EmailService>();
        }

        private void RegisterAPI()
        {
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(ninjectKernel);
        }
    }
}