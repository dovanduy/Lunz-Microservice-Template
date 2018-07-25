using System;
using Lunz.Configuration;
using Lunz.Data;
using Lunz.Microservice.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Lunz.Microservice.WebApp.Config
{
    public static class DataConfigExtensions
    {
        public static void ConfigureData(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddSingleton<IDatabaseFactory>(new DatabaseFactory(key =>
            {
                switch (key)
                {
                    case "OrderManagement":
                        return configuration.ConnectionStrings.OrderManagement;
                    case "ReferenceData":
                        return configuration.ConnectionStrings.OrderManagement;
                    default:
                        throw new NotImplementedException($"未实现对关键字 '{key}' 的处理。");
                }
            }))
            .AddSingleton<IDatabaseScopeFactory, DatabaseScopeFactory>()
            .AddSingleton<IAmbientDatabaseLocator, AmbientDatabaseLocator>();
        }
    }
}