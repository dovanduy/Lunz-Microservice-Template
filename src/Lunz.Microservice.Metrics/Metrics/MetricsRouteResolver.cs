using App.Metrics.AspNetCore;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace Lunz.Microservice.Metrics
{
    public class MetricsRouteResolver : IRouteNameResolver
    {
        /// <summary>
        /// 初始化路由信息字符串
        /// </summary>
        /// <param name="routeData">routeData</param>
        /// <returns></returns>
        public Task<string> ResolveMatchingTemplateRouteAsync(RouteData routeData)
        {

            if (!(routeData.Routers.FirstOrDefault(r => r.GetType().Name == "Route") is Route templateRoute))
            {
                return Task.FromResult(string.Empty);
            }

            var controller = routeData.Values.FirstOrDefault(v => v.Key == "controller");
            var action = routeData.Values.FirstOrDefault(v => v.Key == "action");
            var area = routeData.Values.FirstOrDefault(v => v.Key == "area");
            var id = routeData.Values.FirstOrDefault(v => v.Key == "id");

            var result = templateRoute.ToTemplateString(area.Value as string, controller.Value as string, action.Value as string, id.Value as string);
            return Task.FromResult(result);
        }
    }
}