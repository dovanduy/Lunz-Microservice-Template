using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Lunz.Microservice.Metrics
{
    public class MetricsRouteOptionsConfig : IConfigureOptions<MvcOptions>
    {
        /// <inheritdoc />
        public void Configure(MvcOptions options)
        {
            if (!options.Filters.OfType<MetricsRouteFilter>().Any())
            {
                options.Filters.Add(new MetricsRouteFilter(new MetricsRouteTemplateResolver()));
            }
        }
    }
}
