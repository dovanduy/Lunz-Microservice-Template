using Com.Ctrip.Framework.Apollo;
using Lunz.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lunz.Microservice.WebApp.Config
{
    public static class ConfigurationManagerExtensions
    {
        public static void ConfigureApollo(this IConfiguration configuration)
        {
            // 初始化 apollo 配置
            var builder = new ConfigurationBuilder()
                .AddApollo(configuration.GetSection("Apollo"))
                .AddDefault();
            var nps = configuration["Apollo:Namespaces"];
            if (!string.IsNullOrWhiteSpace(nps))
            {
                foreach (var np in nps.Split(','))
                {
                    builder.AddNamespace(np, np);
                }
            }
            builder.Build();
        }

        public static IConfigurationManager ConfigureConfigurationManager(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationManager = new ConfigurationManager(new ApolloConfigurationManager(),
                configuration["Apollo:Namespaces"]?.Split(','));
            services.AddSingleton<IConfigurationManager>(configurationManager);

            return configurationManager;
        }
    }
}