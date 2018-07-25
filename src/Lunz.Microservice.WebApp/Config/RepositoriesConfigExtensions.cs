using Lunz.Microservice.OrderManagement.QueryStack.MySql.Repositories;
using Lunz.Microservice.OrderManagement.QueryStack.Repositories;
using Lunz.Microservice.ReferenceData.QueryStack.MySql.Repositories;
using Lunz.Microservice.ReferenceData.QueryStack.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lunz.Microservice.WebApp.Config
{
    public static class RepositoriesConfigExtensions
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IHearFromRepository, HearFromRepository>();
        }
    }
}