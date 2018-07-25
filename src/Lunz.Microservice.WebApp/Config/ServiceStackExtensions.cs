using Funq;
using Lunz.Metrics.Extensions.Metrics;
using Lunz.Microservice.Common;
using Lunz.Microservice.OrderManagement.Models.Validators;
using Lunz.Microservice.OrderManagement.Services.Orders;
using Lunz.Microservice.ReferenceData.Services.HearFroms;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Validation;
using System;
using Lunz.Microservice.ReferenceData.Api.HearFroms;

namespace Lunz.Microservice.WebApp.Config
{
    public static class ServiceStackExtensions
    {
        public static void UseServiceStack(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseServiceStack(new AppHost
            {
                AppSettings = new NetCoreAppSettings(configuration)
            });
        }
    }

    public class AppHost : AppHostBaseMetricsExtension
    {
        public AppHost() : base("Lunz.Microservice.WebApp",
            // INFO: 这里注册更多的服务集（ServiceStack.Service）。
            typeof(OrdersService).Assembly,
            typeof(HearFromsService).Assembly)
        {
        }

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        protected override void ConfigureInternal(Container container)
        {
            Plugins.Add(new ValidationFeature());
            // INFO: 这里注册更多的验证器（Validator）。
            container.RegisterValidators(
                typeof(OrderValidator).Assembly
                , typeof(HearFromValidator).Assembly);

            SetConfig(new HostConfig
            {
                DefaultRedirectPath = "/metadata",
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false)
            });
        }

        protected override RouteAttribute[] GetRouteAttributesInternal(Type requestType, RouteAttribute[] routes)
        {
            return routes.RouteUrl();
        }
    }
}
