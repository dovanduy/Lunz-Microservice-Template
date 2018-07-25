using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lunz.Microservice.OrderManagement.Contracts.Commands
{
    public class DeleteOrder : IRequest<DeleteOrderResponse>
    {
        public Guid OrderId { get; set; }
    }

    public class DeleteOrderResponse
    {
        public DeleteOrderResponse(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }
    }
}
