using System;
using System.Threading.Tasks;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Repositories;
using Lunz.Microservice.OrderManagement.Contracts.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.OrderManagement.CommandStack.Handlers
{
    public class DeleteHandler : AsyncRequestHandler<DeleteOrder, DeleteOrderResponse>
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteHandler> _logger;

        public DeleteHandler(IOrderRepository orderRepository, IMediator mediator,
            ILogger<DeleteHandler> logger)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task<DeleteOrderResponse> Handle(DeleteOrder request)
        {
            var order = await _orderRepository.FindAsync(request.OrderId);
            if (order == null)
                throw new ApplicationException($"未找到 Id={request.OrderId} 的订单信息。");

            await _mediator.Publish(new OrderDeleted(request.OrderId));

            await _orderRepository.RemoveAsync(order);

            return new DeleteOrderResponse(request.OrderId);
        }
    }
}
