using System;
using System.Threading.Tasks;
using Lunz.Domain.Kernel.Repositories;
using Lunz.Microservice.OrderManagement.QueryStack.Models;

namespace Lunz.Microservice.OrderManagement.QueryStack.Repositories
{
    public interface IOrderItemRepository : IRepository<Guid, OrderItem>
    {
    }
}