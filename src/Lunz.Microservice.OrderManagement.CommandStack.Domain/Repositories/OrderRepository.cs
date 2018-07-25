using System;
using Lunz.Domain.Kernel.Models;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Repositories
{
    public class OrderRepository
        : RepositoryBase<Guid, Order>
        , IOrderRepository
    {
        public OrderRepository(OrderManagementDbContext dbContext, IEventDispatcher eventDispatcher)
            : base(dbContext.Orders, eventDispatcher)
        {
        }
    }
}