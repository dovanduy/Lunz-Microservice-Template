using System;
using System.Collections;
using System.Collections.Generic;
using Lunz.Microservice.Core.Models;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events
{
    public class OrderEntered : OrderDomainEventBase
    {
        public OrderEntered(Guid aggregateId, string subject, DateTime date, Guid? hearFromId,
            int amount, decimal price, decimal total, UserDetails userDetails)
            : base(aggregateId)
        {
            Subject = subject;
            Date = date;
            HearFromId = hearFromId;
            Amount = amount;
            Price = price;
            Total = total;
            User = userDetails;
        }

        public UserDetails User { get; }
        public string Subject { get; }
        public DateTime Date { get; }
        public Guid? HearFromId { get; }
        public int Amount { get; }
        public decimal Price { get; }
        public decimal Total { get; set; }
    }
}