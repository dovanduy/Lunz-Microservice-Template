using System;
using System.Threading;
using System.Threading.Tasks;
using Lunz.Data;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events;
using Lunz.Microservice.OrderManagement.QueryStack.Models;
using Lunz.Microservice.OrderManagement.QueryStack.Repositories;
using MediatR;

namespace Lunz.Microservice.OrderManagement.QueryStack.Denormalizers
{
    public class OrderDenormalizer : DenormalizerBase
        , INotificationHandler<OrderEntered>
        , INotificationHandler<OrderItemAdded>
        , INotificationHandler<OrderPaid>
        , INotificationHandler<OrderUpdated>
        , INotificationHandler<OrderDeleted>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderDenormalizer(IOrderRepository orderRepository
            , IOrderItemRepository orderItemRepository
            , IAmbientDatabaseLocator databaseLocator)
            : base(databaseLocator)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task Handle(OrderEntered message, CancellationToken cancellationToken)
        {
            await _orderRepository.AddAsync(new Order()
            {
                Id = message.AggregateId,
                Subject = message.Subject,
                Date = message.Date,
                HearFromId = message.HearFromId,
                Amount = message.Amount,
                Price = message.Price,
                Total = message.Total
            });
        }

        public async Task Handle(OrderItemAdded message, CancellationToken cancellationToken)
        {
            await _orderItemRepository.AddAsync(new OrderItem()
            {
                Id = message.ItemId,
                OrderId = message.AggregateId,
                ProductName = message.ProductName,
                Price = message.Price,
                Amount = message.Amount,
                Total = message.Total
            });
        }

        public async Task Handle(OrderPaid message, CancellationToken cancellationToken)
        {
            await _orderRepository.PaidAsync(message.AggregateId);
        }

        public async Task Handle(OrderUpdated message, CancellationToken cancellationToken)
        {
            await _orderRepository.UpdateAsync(message.AggregateId, new Order()
            {
                Id = message.AggregateId,
                Subject = message.Subject,
                Date = message.Date,
                HearFromId = message.HearFromId,
                Amount = message.Amount,
                Price = message.Price,
                Total = message.Total
            });
        }

        public async Task Handle(OrderDeleted message, CancellationToken cancellationToken)
        {
            await _orderRepository.DeleteAsync(message.AggregateId);
        }
    }
}