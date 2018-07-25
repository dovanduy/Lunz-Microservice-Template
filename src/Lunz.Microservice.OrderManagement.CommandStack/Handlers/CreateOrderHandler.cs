using System.Linq;
using System.Threading.Tasks;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Repositories;
using Lunz.Microservice.OrderManagement.Contracts.Commands;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.OrderManagement.CommandStack.Handlers
{
    public class CreateOrderHandler : AsyncRequestHandler<CreateOrder, CreateOrderResponse>
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(IOrderRepository orderRepository, IMediator mediator,
            ILogger<CreateOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task<CreateOrderResponse> Handle(CreateOrder request)
        {
            var order = await _orderRepository.FindAsync(request.OrderId);
            if (order != null)
                return new CreateOrderResponse(order.Id);

            order = Order.EnterOrder(request.OrderId, request.Subject, request.Date, request.HearFromId,
                request.HearFromName, request.Amount, request.Price);

            request.OrderItems.ForEach(x => order.AddItem(NewId.NextGuid(), x.ProductName, x.Amount, x.Price));

            await _orderRepository.AddAsync(order);

            return new CreateOrderResponse(order.Id);
        }
    }
}