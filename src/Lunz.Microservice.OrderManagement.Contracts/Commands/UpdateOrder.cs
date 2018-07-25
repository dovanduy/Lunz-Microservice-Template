using Lunz.Microservice.Core.Models.OrderManagement;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lunz.Microservice.OrderManagement.Contracts.Commands
{
    public class UpdateOrder : OrderDetails, IRequest<UpdateOrderResponse>
    {
        public UpdateOrder(Guid orderId, string subject, DateTime date, Guid? hearFromId, string hearFromName,
            int amount, decimal price)
        {
            OrderId = orderId;
            Subject = subject;
            Date = date;
            HearFromId = hearFromId;
            HearFromName = hearFromName;
            Amount = amount;
            Price = price;
            UpdatedAt = DateTime.Now;
        }

        public Guid OrderId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateOrderResponse
    {
        public UpdateOrderResponse(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}
