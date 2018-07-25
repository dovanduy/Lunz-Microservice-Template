using System;
using System.Collections.Generic;
using System.Text;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events
{
    public class OrderUpdated : OrderDomainEventBase
    {
        public OrderUpdated(Guid aggregateId, string subject, DateTime date, Guid? hearFromId,
            int amount, decimal price, decimal total)
            : base(aggregateId)
        {
            Subject = subject;
            Date = date;
            HearFromId = hearFromId;
            Amount = amount;
            Price = price;
            Total = total;
        }

        public string Subject { get; }
        public DateTime Date { get; }
        public Guid? HearFromId { get; }
        public int Amount { get; }
        public decimal Price { get; }
        public decimal Total { get; set; }
    }
}
