using App.Metrics.AspNetCore;
using App.Metrics.Extensions.DependencyInjection.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Lunz.Microservice.Metrics
{
    public class MetricsRouteFilter : IAsyncResourceFilter
    {
        private readonly IRouteNameResolver _routeNameResolver;
        private ILogger _logger;

        public MetricsRouteFilter(IRouteNameResolver routeNameResolver)
        {
            _routeNameResolver = routeNameResolver ?? throw new ArgumentNullException(nameof(routeNameResolver));
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // Verify if AppMetrics was configured
            AppMetricsServicesHelper.ThrowIfMetricsNotRegistered(context.HttpContext.RequestServices);

            EnsureServices(context.HttpContext);

            var templateRoute = await _routeNameResolver.ResolveMatchingTemplateRouteAsync(context.RouteData);
            if (context.RouteData.Values != null && context.RouteData.Values.Count > 0)
            {
                string logRouteTemplate = templateRoute;
                foreach (var item in context.RouteData.Values)
                {
                    Regex r = new Regex("{" + item.Key + "[\\s\\S]*}");
                    logRouteTemplate = r.Replace(logRouteTemplate, item.Value.ToString());
                }
                if (context.HttpContext.Request.Body.CanRead && context.HttpContext.Request.ContentLength.HasValue)
                {
                    var buffer = new byte[Convert.ToInt32(context.HttpContext.Request.ContentLength)];
                    await context.HttpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                    var bodyString = Encoding.UTF8.GetString(buffer).Replace("\n", string.Empty);
                    if (!string.IsNullOrEmpty(bodyString))
                        logRouteTemplate = string.Format("{0}------body:{1}", logRouteTemplate, bodyString);

                    // https://stackoverflow.com/questions/21805362/rewind-request-body-stream
                    context.HttpContext.Request.Body = new MemoryStream(buffer);
                }
                if (context.HttpContext.Request.QueryString.HasValue)
                    _logger.LogInformation(string.Format("{0}:{1}{2}", context.HttpContext.Request.Method, logRouteTemplate, HttpUtility.UrlDecode(context.HttpContext.Request.QueryString.ToUriComponent())));
                else
                    _logger.LogInformation(string.Format("{0}:{1}", context.HttpContext.Request.Method, logRouteTemplate));
            }


            if (!string.IsNullOrEmpty(templateRoute))
            {
                context.HttpContext.AddMetricsCurrentRouteName(templateRoute);
            }

            await next.Invoke();
        }

        private void EnsureServices(HttpContext context)
        {
            if (_logger != null)
            {
                return;
            }

            var factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            _logger = factory.CreateLogger<MetricsResourceFilter>();
        }
    }
}

