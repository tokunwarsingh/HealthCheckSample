using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class HealthController : ApiController
    {
        private readonly IHealthCheck[]_healthChecks = null;
        public HealthController(IHealthCheck[] healthChecks)
        {
            _healthChecks = healthChecks;
        }

        public HealthController()
        {
            _healthChecks = WebApiApplication._container.ResolveAll<IHealthCheck>();
        }

        public async Task<IHttpActionResult> GetHealthInfo()
        {
            var stopwatch = new Stopwatch();
            var context = new HealthCheckContext();
            var result = new Models.HealthCheckResult();
            stopwatch.Start();
            var items = new List<HealthCheckResult>();
           
            await Task.WhenAll(_healthChecks.Select(async x => items.Add(await x.CheckHealthAsync(context))));
            stopwatch.Stop();
            
            result.Version = "1.0";
            result.HealthCheckDuration = stopwatch.Elapsed.TotalSeconds.ToString();
            items.ForEach(x => result.HealthChecks.Add(new Models.HealthCheckResult.HealthCheck()
            {
                Description = "",
                Status = x.Status.ToString(),
                Component = x.Description
            }));

            result.Status = result.HealthChecks.Exists(x=>x.Status.Equals(HealthStatus.Unhealthy.ToString()))? HealthStatus.Unhealthy.ToString(): HealthStatus.Healthy.ToString();
            return Ok(result);
        }

        #region deleted code
        /*
        public async Task<IHttpActionResult> GetHealthInfo()
        {
            var stopwatch = new Stopwatch();
            var context = new HealthCheckContext();
            var result = new Models.HealthCheckResult();
            stopwatch.Start();
            var items = new List<HealthCheckResult>();
            var instances = new List<IHealthCheck>();
            var providers = GetAllProviders();
            foreach (var provider in providers.GetRange(1, providers.Count-1))
            {
                var instance = (IHealthCheck)Activator.CreateInstance(provider);
                instances.Add(instance);
            }
            await Task.WhenAll(instances.Select(async x => items.Add(await x.CheckHealthAsync(context))));
            stopwatch.Stop();
            result.HealthCheckDuration = stopwatch.Elapsed.TotalSeconds.ToString();
           items.ForEach(x => result.HealthChecks.Add(new Models.HealthCheckResult.HealthCheck()
            {
                Description ="",                
                Status = x.Status.ToString(),
                Component = x.Description
            }));
            return Ok(result);
        }

        private List<Type> GetAllProviders()
        {
            return GetTypesDeriving<IHealthCheck>();
        }

        private static List<Type> GetTypesDeriving<T>()
        {
            return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from assemblyType in domainAssembly.GetTypes()
                    where typeof(T).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract
                    select assemblyType).ToList();
        }
        */
        #endregion
    }
}