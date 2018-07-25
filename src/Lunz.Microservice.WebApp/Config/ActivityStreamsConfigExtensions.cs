using Lunz.Configuration;
using Lunz.Data.MongoDb;
using Lunz.Microservice.ActivityStreams;
using Lunz.Microservice.ActivityStreams.Services;
using Lunz.Microservice.Core.Urls;
using Microsoft.Extensions.DependencyInjection;

namespace Lunz.Microservice.WebApp.Config
{
    public static class ActivityStreamsConfigExtensions
    {
        public static void ConfigureActivityStreams(this IServiceCollection services, IConfigurationManager configuration)
        {
            var orderManagementConfigration = new MongoDbConfiguration()
            {
                ConnectionString = configuration.AppSettings.OrderManagement.DomainConnectionString,
                Database = configuration.AppSettings.OrderManagement.DomainDatabase,
            };
            
            // INFO: 这里注册更多的 ActivityStreamsDbContext
            services.AddTransient<IUrlSettings, UrlSettings>();
            services.AddTransient<IAppUrlProvider, AppUrlProvider>();
            services.AddTransient<HtmlContext>();
            services.AddTransient(sp => new OrderActivityStreamsDbContext(orderManagementConfigration));
            services.AddTransient<OrderHtmlContext>();
            services.AddTransient<IOrderActivityStreamsService, OrderActivityStreamsService>();
        }
    }
}