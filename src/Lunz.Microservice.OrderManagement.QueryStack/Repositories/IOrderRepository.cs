using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lunz.Domain.Kernel.Repositories;
using Lunz.Microservice.OrderManagement.QueryStack.Models;

namespace Lunz.Microservice.OrderManagement.QueryStack.Repositories
{
    public interface IOrderRepository : IRepository<Guid, Order>
    {
        Task PaidAsync(Guid id);
        Task<(long Count, IEnumerable<Order> Data)> QueryAsync(
            Func<(string Sql, dynamic Parameters)> filter = null, int? pageIndex = null, int? pageSize = null,
            string orderBy = null, bool hasCountResult = false);
        Task<(long Count, IEnumerable<T> Data)> QueryAsync<T>(
            Func<(string Sql, dynamic Parameters)> filter = null, int? pageIndex = null, int? pageSize = null,
            string orderBy = null, bool hasCountResult = false);
    }
}