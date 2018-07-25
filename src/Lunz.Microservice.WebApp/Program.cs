using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using App.Metrics.AspNetCore;
using App.Metrics.Reporting.InfluxDB;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Lunz.Microservice.WebApp
{
    public class Program
    {
        public const string Version = "1.0.0";

        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.LoadConfiguration("NLog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Initialize app...");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                               .AddJsonFile(path: $"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                               .Build();

            var influxOptions = new MetricsReportingInfluxDbOptions();

            configuration.GetSection(nameof(MetricsReportingInfluxDbOptions)).Bind(influxOptions);

            return WebHost.CreateDefaultBuilder(args)
                    .ConfigureMetricsWithDefaults(builder =>
                    {
                        builder.Report.ToInfluxDb(influxOptions);
                    })
					.UseIISIntegration()
                    .UseMetrics() // Setup Metrics
                    .UseStartup<Startup>()
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                    .UseNLog()  // NLog: setup NLog for Dependency injection
                    .Build();
        }
    }
}
