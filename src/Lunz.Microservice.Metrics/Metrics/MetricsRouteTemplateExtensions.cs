using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using System.Linq;

namespace Lunz.Microservice.Metrics
{
    public static class MetricsRouteTemplateExtensions
    {
        /// <summary>
        //  返回路由字符串
        /// </summary>
        /// <param name="templateRoute">路由参数</param>
        /// <param name="area">area</param>
        /// <param name="controller">controller</param>
        /// <param name="action">action</param>
        /// <param name="id">id</param>
        /// <returns></returns>
        public static string ToTemplateString(this Route templateRoute, string area, string controller, string action, string id)
        {
            return string.Join("/", templateRoute.ParsedTemplate.Segments.Select(s => s.ToTemplateSegmentString()))
                   .Replace("{area}", area)
                   .Replace("{controller}", controller)
                   .Replace("{action}", action).ToLower()
                   .Replace("{id?}", id).ToLower(); ;
        }
    }
}
