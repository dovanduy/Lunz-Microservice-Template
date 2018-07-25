using System;

namespace Lunz.Microservice.Core.Urls
{
    public interface IAppUrlProvider
    {
        string OrderPageUrl(Guid orderId);
    }
}