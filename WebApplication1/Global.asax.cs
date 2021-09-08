using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebApplication1.Dependency;
using WebApplication1.health;

namespace WebApplication1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IWindsorContainer _container;
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ConfigureWindsor(GlobalConfiguration.Configuration);
            //IWindsorContainer container = new WindsorContainer();
            //container.Install(FromAssembly.This());
        }

        public static void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;
        }

    }
}
