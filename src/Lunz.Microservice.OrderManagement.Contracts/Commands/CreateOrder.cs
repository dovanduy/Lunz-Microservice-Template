using System;
using System.Collections.Generic;
using Lunz.Microservice.Core.Models.OrderManagement;
using MediatR;

namespace Lunz.Microservice.OrderManagement.Contracts.Commands
{
    public class CreateOrder : OrderDetails, IRequest<CreateOrderResponse>
    {
        public CreateOrder(Guid orderId, string subject, DateTime date, Guid? hearFromId, string hearFromName,
            int amount, decimal price, List<OrderItem> items)
        {
            OrderId = orderId;
            Subject = subject;
            Date = date;
            HearFromId = hearFromId;
            HearFromName = hearFromName;
            Amount = amount;
            Price = price;
            CreatedAt = DateTime.Now;
            OrderItems = items;
        }

        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItem> OrderItems { get; }
    }

    public class CreateOrderResponse
    {
        public CreateOrderResponse(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}