using System;
using System.Linq;
using System.Threading.Tasks;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Repositories;
using Lunz.Microservice.OrderManagement.Contracts.Commands;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.OrderManagement.CommandStack.Handlers
{
    public class UpdateOrderHandler : AsyncRequestHandler<UpdateOrder, UpdateOrderResponse>
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<UpdateOrderHandler> _logger;

        public UpdateOrderHandler(IOrderRepository orderRepository, IMediator mediator,
            ILogger<UpdateOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task<UpdateOrderResponse> Handle(UpdateOrder request)
        {
            var order = await _orderRepository.FindAsync(request.OrderId);
            if (order == null)
                throw new ApplicationException($"未找到 Id={request.OrderId} 的订单信息。");

            order.Subject = request.Subject;
            order.Date = request.Date;
            order.HearFromId = request.HearFromId;
            order.HearFromName = request.HearFromName;
            order.Amount = request.Amount;
            order.Price = request.Price;

            await _mediator.Publish(new OrderUpdated(order.Id, order.Subject, order.Date,
                order.HearFromId, order.Amount, order.Price, order.Total));

            await _orderRepository.UpdateAsync(order);

            return new UpdateOrderResponse(order.Id);
        }
    }
}
