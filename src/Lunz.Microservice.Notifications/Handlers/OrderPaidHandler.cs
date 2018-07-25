using System.Threading;
using System.Threading.Tasks;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.Notifications.Handlers
{
    public class OrderPaidHandler : INotificationHandler<OrderPaid>
    {
        private readonly ILogger<OrderPaidHandler> _logger;

        public OrderPaidHandler(ILogger<OrderPaidHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(OrderPaid message, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Order #{message.AggregateId} has been paid.");
        }
    }
}
