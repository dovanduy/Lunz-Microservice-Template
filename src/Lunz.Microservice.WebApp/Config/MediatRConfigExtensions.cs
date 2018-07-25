using Lunz.Microservice.ActivityStreams.Handlers;
using Lunz.Domain.Kernel.Models;
using Lunz.Microservice.Notifications.Handlers;
using Lunz.Microservice.OrderManagement.CommandStack.Handlers;
using Lunz.Microservice.OrderManagement.QueryStack.Denormalizers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Lunz.Microservice.WebApp.Config
{
    public static class MediatRConfigExtensions
    {
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            // INFO: 这里注册更多的 MediatR's Handler
            services.AddMediatR(
                typeof(PayOrderHandler).Assembly
                , typeof(OrderPaidHandler).Assembly
                , typeof(OrderDenormalizer).Assembly
                , typeof(OrderActivityStreamsHandler).Assembly);

            services.AddTransient<IEventDispatcher, MediatorEventDispatcher>();
        }
    }
}