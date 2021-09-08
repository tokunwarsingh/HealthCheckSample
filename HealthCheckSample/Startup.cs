using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;


[assembly: OwinStartup(typeof(HealthCheckSample.Startup))]

namespace HealthCheckSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHealthChecks("/Health", options);
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
