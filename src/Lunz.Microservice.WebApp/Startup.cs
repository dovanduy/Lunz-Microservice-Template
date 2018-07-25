using Lunz.Microservice.Metrics;
using Lunz.Microservice.WebApp.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;

namespace Lunz.Microservice.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            configuration.ConfigureApollo();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = services.ConfigureConfigurationManager(Configuration);

            services.ConfigureData(configuration);
            services.ConfigureRepositories();
            services.ConfigureDomainRepositories(configuration);
            services.ConfigureAutoMapper();
            services.ConfigureMediatR();
            services.ConfigureActivityStreams(configuration);
            services.ConfigureConsul(configuration);
            services.ConfigHealthCheckers();

            services.ConfigureMvc(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureCors();

            app.UseStaticFiles();

            app.UseSystemMetrics(Configuration);

            app.UseSwagger();

            app.UseMvc();
        }
    }
}
