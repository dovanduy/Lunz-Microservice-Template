using Lunz.Microservice.Health.Health;
using Lunz.Microservice.Health.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lunz.Microservice.WebApp.Config
{
    public static class HealthConfigExtensions
    {
		public static void ConfigHealthCheckers(this IServiceCollection services)
		{
			services.AddTransient<IHealthChecker, ConsulHealthChecker>();
			services.AddTransient<IHealthChecker, DBHealthChecker>();
		}
	}
}
