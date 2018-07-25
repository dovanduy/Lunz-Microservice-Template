using Lunz.Microservice.OrderManagement.Api.Orders;
using Lunz.Microservice.ReferenceData.Api.HearFroms;
using Microsoft.AspNetCore.Builder;
using NJsonSchema;
using NSwag.AspNetCore;

namespace Lunz.Microservice.WebApp.Config
{
    public static class SwaggerConfigExtensions
    {
        public static void UseSwagger(this IApplicationBuilder app)
        {
            // INFO: 在这里配置更多的 Controller 程序集。
            var controllerAssemblies = new[]
            {
                typeof(OrdersController).Assembly,
                typeof(HearFromsController).Assembly
            };

            // Enable the Swagger UI middleware and the Swagger generator
            app.UseSwaggerUi3(controllerAssemblies, settings =>
            {
                settings.SwaggerUiRoute = "/docs";
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.PostProcess = document =>
                {
                    document.Info.Title = "Microservice Template";
                    document.Info.Version = Program.Version;
                };
            });

            app.UseSwaggerReDoc(controllerAssemblies, settings =>
            {
                settings.SwaggerUiRoute = "/redoc";
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.PostProcess = document =>
                {
                    document.Info.Title = "Microservice Template";
                    document.Info.Version = Program.Version;
                };
            });
        }
    }
}