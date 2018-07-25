using System.Threading.Tasks;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events;
using Lunz.Microservice.OrderManagement.Contracts.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.OrderManagement.CommandStack.Handlers
{
    public class PayOrderHandler : AsyncRequestHandler<PayOrder, PayOrderResponse>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PayOrderHandler> _logger;

        public PayOrderHandler(IMediator mediator, ILogger<PayOrderHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task<PayOrderResponse> Handle(PayOrder request)
        {
            // TODO: 完成方法

            await _mediator.Publish(new OrderPaid(request.OrderId, request.Payment));

            return new PayOrderResponse()
            {
                OrderId = request.OrderId
            };
        }
    }
}