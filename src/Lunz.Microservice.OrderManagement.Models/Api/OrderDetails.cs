using System;
using System.Collections;
using System.Collections.Generic;

namespace Lunz.Microservice.OrderManagement.Models.Api
{
    public class OrderDetails : Core.Models.OrderManagement.OrderDetails
    {
        public Guid Id { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }

        public OrderDetails()
        {
            Items = new List<OrderItem>();
        }
    }
}
