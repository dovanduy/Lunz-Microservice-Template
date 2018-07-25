using System;
using Lunz.Domain.Kernel.Repositories;

namespace Lunz.Microservice.OrderManagement.QueryStack.Models
{
    public class OrderItem : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
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
    }
}