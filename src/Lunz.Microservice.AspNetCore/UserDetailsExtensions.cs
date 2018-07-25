using Lunz.Microservice.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Lunz.Microservice.AspNetCore
{
    public static class UserDetailsExtensions
    {
        public static UserDetails UserDetails(this HttpRequest request)
        {
            var data = request.Headers["user-details"];

            if (string.IsNullOrWhiteSpace(data) || data.Count == 0)
                return null;

            var userDetails = JsonConvert.DeserializeObject<UserDetails>(data[0], new UrlEncodeConverter());
            return userDetails;
        }
    }
}