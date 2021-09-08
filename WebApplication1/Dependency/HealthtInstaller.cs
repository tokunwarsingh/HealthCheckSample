using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Web.Http;
using WebApplication1.health;

namespace WebApplication1.Dependency
{
    public class HealthtInstaller: IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {  
            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestylePerWebRequest());
            container.Register(Component.For<IHealthCheck>().ImplementedBy<AuthorizationServiceHealthCheck>());
            container.Register(Component.For<IHealthCheck>().ImplementedBy<InternalGetPayerAPIHealthCheck>());
        }
    }
}