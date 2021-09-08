using Castle.Core.Configuration;
using Castle.MicroKernel;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.health
{
    public class InternalGetPayerAPIHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var url = "https://localhost:44399/api/values";
            var apiName = "InternalPayerListAPI";
            try
            {
                return Utilities.CallAPI(url, apiName);
            }
            catch (Exception)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(apiName));
            }           
        }        
    }


    public class AuthorizationServiceHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var url = "https://localhost:44399/api/values";
            var apiName = "AuthorizationServiceHealthCheck";
            try
            {
                return Utilities.CallAPI(url, apiName);
            }
            catch (Exception)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(apiName));
            }          
        }
    }

    public static class Utilities
    {
        public static Task<HealthCheckResult> CallAPI(string url, string apiName)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                return Task.FromResult(HealthCheckResult.Healthy(apiName));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(apiName));
            }
        }

    }

    #region old codr
    public class HealthCheckServicesCollection
    {
        public IHealthCheck[] HealthCheckServices { get; private set; }

        public HealthCheckServicesCollection(IHealthCheck[] healthCheckCollections)
        {
            HealthCheckServices = healthCheckCollections;
        }
    }

    public class ArrayFacility : IFacility
    {
        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.Resolver.AddSubResolver(new ArrayResolver(kernel));
        }

        public void Terminate() { }
    }
    #endregion
}   