using System;

namespace Lunz.Microservice.OrderManagement.Models.Api
{
    public class OrderItem : Core.Models.OrderManagement.OrderItem
    {
        public Guid Id { get; set; }
    }
}