using System;
using Lunz.Domain.Kernel.Models;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Models
{
    public class OrderItem : EntityMappedWithExpressions<Guid, Order>
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Total {
            get {
                return Amount * Price;
            }
            private set { }
        }

        internal OrderItem(Order parent, Guid entityId)
            : base(parent, entityId)
        {
        }

        public override void InitializeEventHandlers()
        {
            // Do anything
        }
    }
}