using System;
using System.Collections;
using System.Collections.Generic;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events
{
    public class OrderItemAdded : OrderDomainEventBase
    {
        public OrderItemAdded(Guid aggregateId, Guid itemId, string productName,
            int amount, decimal price, decimal total)
            : base(aggregateId)
        {
            ItemId = itemId;
            ProductName = productName;
            Amount = amount;
            Price = price;
            Total = total;
        }

        public Guid ItemId { get; }
        public string ProductName { get; }
        public int Amount { get; }
        public decimal Price { get; }
        public decimal Total { get; set; }
    }
}