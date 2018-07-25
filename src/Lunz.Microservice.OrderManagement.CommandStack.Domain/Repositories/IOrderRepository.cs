using System;
using Lunz.Domain.Kernel.Models;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Guid, Order>
    {
    }
}