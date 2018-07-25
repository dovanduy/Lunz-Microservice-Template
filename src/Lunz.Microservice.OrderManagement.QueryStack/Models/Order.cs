using System;
using System.Collections;
using System.Collections.Generic;
using Lunz.Domain.Kernel.Repositories;

namespace Lunz.Microservice.OrderManagement.QueryStack.Models
{
    public class Order : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public Guid? HearFromId { get; set; }
        public bool Paid { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public Guid? CreatedById { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public Guid? DeletedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<OrderItem> Items { get; }

        public Order()
        {
            Items = new List<OrderItem>();
        }
    }
}
