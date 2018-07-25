using System;

namespace Lunz.Microservice.Core.Urls
{
    public class AppUrlProvider : IAppUrlProvider
    {
        private readonly IUrlSettings _settings;

        public AppUrlProvider(IUrlSettings settings)
        {
            _settings = settings;
        }

        public string OrderPageUrl(Guid orderId)
        {
            var template = _settings.OrderUrlTemplate;
            return string.Format(template, orderId);
        }
    }
}