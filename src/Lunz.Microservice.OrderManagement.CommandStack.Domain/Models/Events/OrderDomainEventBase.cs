using System;
using Lunz.Domain.Kernel.Models;
using MediatR;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events
{
    public abstract class OrderDomainEventBase : SourcedEventBase<Guid>, INotification
    {
        protected OrderDomainEventBase(Guid aggregateId)
            : base(aggregateId)
        {
        }
    }
}