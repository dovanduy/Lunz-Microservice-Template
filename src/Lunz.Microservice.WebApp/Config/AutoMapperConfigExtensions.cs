using AutoMapper;
using Lunz.Microservice.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Lunz.Microservice.OrderManagement.Api.Orders;
using Lunz.Microservice.ReferenceData.Api.HearFroms;

namespace Lunz.Microservice.WebApp.Config
{
    public static class AutoMapperConfigExtensions
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            // INFO: 这里注册更多的 MappingProfile
            var assemblies = new[]
            {
                typeof(OrderProfile).Assembly,
                typeof(OrdersController).Assembly,
                typeof(HearFromsController).Assembly
            };

            services.AddAutoMapper(assemblies);
        }
    }
}