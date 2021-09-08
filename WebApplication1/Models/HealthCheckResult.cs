using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class HealthCheckResult
    {
        public HealthCheckResult()
        {
            HealthChecks = new List<HealthCheck>();
        }
        public string Status { get; set; }
        public List<HealthCheck> HealthChecks { get; set; }
        public string HealthCheckDuration { get; set; }

        public string Version { get; set; }

        public class HealthCheck
        {
            public string Status { get; set; }
            public string Component { get; set; }
            public string Description { get; set; }
        }
    }
}