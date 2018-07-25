using App.Metrics.AspNetCore;
using App.Metrics.Logging;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lunz.Microservice.Metrics
{
    public class MetricsRouteTemplateResolver : IRouteNameResolver
    {
        private const string ApiVersionToken = "{version:apiversion}";
        private const string MsVersionpolicyIsAppliedToken = "MS_VersionPolicyIsApplied";
        private const string VersionRouteDataToken = "version";
        private readonly IRouteNameResolver _routeNameResolver;
        private static readonly ILog Logger = LogProvider.For<MetricsRouteTemplateResolver>();

        public MetricsRouteTemplateResolver()
            : this(new MetricsRouteResolver())
        {

        }

        private MetricsRouteTemplateResolver(IRouteNameResolver routeNameResolver)
        {
            _routeNameResolver = routeNameResolver ?? throw new ArgumentNullException(nameof(routeNameResolver));
        }

        public async Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData)
        {
            var templateRoute = await _routeNameResolver.ResolveMatchingTemplateRouteAsync(routeData).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(templateRoute))
            {

                return templateRoute;
            }

            if (!(routeData.Routers.FirstOrDefault(r => r.GetType().Name == nameof(MvcAttributeRouteHandler)) is MvcAttributeRouteHandler attributeRouteHandler) || !attributeRouteHandler.Actions.Any())
            {
                return string.Empty;
            }

            if (attributeRouteHandler.Actions.Length == 1)
            {
                var singleDescriptor = attributeRouteHandler.Actions.Single();

                return ExtractRouteTemplate(routeData, singleDescriptor);
            }

            foreach (var actionDescriptor in attributeRouteHandler.Actions)
            {
                if (actionDescriptor.Properties != null && actionDescriptor.Properties.ContainsKey(MsVersionpolicyIsAppliedToken))
                {
                    return ExtractRouteTemplate(routeData, actionDescriptor);
                }
            }

            var firstDescriptor = attributeRouteHandler.Actions.First();

            return ExtractRouteTemplate(routeData, firstDescriptor);
        }

        private static string ExtractRouteTemplate(RouteData routeData, ActionDescriptor actionDescriptor)
        {
            var routeTemplate = actionDescriptor.AttributeRouteInfo?.Template.ToLower() ?? string.Empty;

            if (actionDescriptor.Properties != null && actionDescriptor.Properties.ContainsKey(MsVersionpolicyIsAppliedToken))
            {
                routeTemplate = routeTemplate.Replace(ApiVersionToken, routeData.Values[VersionRouteDataToken].ToString());
            }


            return routeTemplate;
        }
    }
}



