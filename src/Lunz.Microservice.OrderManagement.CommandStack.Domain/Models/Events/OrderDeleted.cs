using System;
using System.Collections.Generic;
using System.Text;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events
{
    public class OrderDeleted : OrderDomainEventBase
    {
        public OrderDeleted(Guid aggregateId)
            : base(aggregateId) { }
    }
}
