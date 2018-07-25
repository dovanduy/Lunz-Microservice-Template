using ServiceStack;
using System.Linq;

namespace Lunz.Microservice.Common
{
    public static class UrlExtensions
    {
        public static string Url(this IReturn requestDto, string httpMethod, string formatFallbackToPredefinedRoute = null)
        {
            return Url((object)requestDto, httpMethod, formatFallbackToPredefinedRoute);
        }

        public static string GetUrl(this object requestDto)
        {
            return requestDto.Url(HttpMethods.Get, formatFallbackToPredefinedRoute: "json");
        }

        public static string PostUrl(this object requestDto)
        {
            return requestDto.Url(HttpMethods.Post, formatFallbackToPredefinedRoute: "json");
        }

        public static string PutUrl(this object requestDto)
        {
            return requestDto.Url(HttpMethods.Put, formatFallbackToPredefinedRoute: "json");
        }

        public static string DeleteUrl(this object requestDto)
        {
            return requestDto.Url(HttpMethods.Delete, formatFallbackToPredefinedRoute: "json");
        }

        public static  string Url(this object requestDto, string httpMethod = "GET", string formatFallbackToPredefinedRoute = null)
        {
            var url = requestDto.ToUrl(httpMethod, formatFallbackToPredefinedRoute);
            return url.StartsWith("~/") ? url.Substring(1) : "/api/v1" + url;
        }

        public static RouteAttribute[] RouteUrl(this RouteAttribute[] routes)
        {
            // INFO: 在这里配置 API URL 根路径，如 /api/v1
            routes.Where(x => !x.Path.StartsWith("~/")).Each(x => x.Path = "/api/v1" + x.Path);
            routes.Where(x => x.Path.StartsWith("~/")).Each(x => x.Path = x.Path.Substring(1));
            return routes;
        }
    }
}
