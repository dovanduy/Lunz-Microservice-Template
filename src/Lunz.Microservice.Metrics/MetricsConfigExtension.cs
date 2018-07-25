using App.Metrics;
using App.Metrics.Scheduling;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
namespace Lunz.Microservice.Metrics
{
    public static class MetricsConfigExtension
    {
        public static void UseSystemMetrics(this IApplicationBuilder app, IConfiguration configuration)
        {
            var metrics = app.ApplicationServices.GetService<IMetrics>();
            var healthChecksConfig = new HealthChecksConfig();

            configuration.GetSection(nameof(HealthChecksConfig)).Bind(healthChecksConfig);
            var healthRoot = new HealthRootBuilder(healthChecksConfig);
            var health = healthRoot.HealthBuilder.HealthBuild(metrics);

            var cpuUsage = metrics.Provider.Gauge.Instance(CpuRegistry.CpuUsageGauge);
            var memoryUsage = metrics.Provider.Gauge.Instance(MemoryRegistry.MemoryUsageGauge);
            var totalMemory = (double)MemoryBaseInfo.GetTotalMemory() / 1024;
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var availMemoryCounter = new PerformanceCounter("Memory", "Available MBytes");

            var healthCheckScheduler = new AppMetricsTaskScheduler(
               TimeSpan.FromSeconds(10),//测试10s
               async () =>
               {
                   cpuCounter.NextValue();
                   Thread.Sleep(1000);
                   cpuUsage.SetValue((int)cpuCounter.NextValue());

                   var availMemory = availMemoryCounter.NextValue() / 1024;
                   memoryUsage.SetValue((int)(100 - availMemory / totalMemory * 100));

                   var healthStatus = await health.HealthCheckRunner.ReadAsync();
                   using (var stream = new MemoryStream())
                   {
                       await health.DefaultOutputHealthFormatter.WriteAsync(stream, healthStatus);
                   }
               });
            healthCheckScheduler.Start();

        }
    }
}
