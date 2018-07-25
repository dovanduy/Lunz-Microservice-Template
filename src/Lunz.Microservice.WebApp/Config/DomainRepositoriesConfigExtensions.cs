using Lunz.Configuration;
using Lunz.Data.MongoDb;
using Lunz.Microservice.OrderManagement.CommandStack.Domain;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lunz.Microservice.WebApp.Config
{
    public static class DomainRepositoriesConfigExtensions
    {
        public static void ConfigureDomainRepositories(this IServiceCollection services, IConfigurationManager configuration)
        {
            var orderManagementConfigration = new MongoDbConfiguration()
            {
                ConnectionString = configuration.AppSettings.OrderManagement.DomainConnectionString,
                Database = configuration.AppSettings.OrderManagement.DomainDatabase,
            };

            services.AddTransient(sp => new OrderManagementDbContext(orderManagementConfigration));
            services.AddTransient<IOrderRepository, OrderRepository>();
        }
    }
}