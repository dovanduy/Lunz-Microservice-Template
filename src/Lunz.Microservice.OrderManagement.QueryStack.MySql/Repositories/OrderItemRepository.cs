using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lunz.Data;
using Lunz.Microservice.Data;
using Lunz.Microservice.OrderManagement.QueryStack.Models;
using Lunz.Microservice.OrderManagement.QueryStack.Repositories;
using Dapper;

namespace Lunz.Microservice.OrderManagement.QueryStack.MySql.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly IAmbientDatabaseLocator _databaseLocator;

        public OrderItemRepository(IAmbientDatabaseLocator databaseLocator)
        {
            _databaseLocator = databaseLocator;
        }

        public async Task<OrderItem> FindAsync(Guid id)
        {
            return await FindAsync<OrderItem>(id);
        }

        public async Task<T> FindAsync<T>(Guid id)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();

            // https://msdn.microsoft.com/zh-cn/magazine/mt703432.aspx
            return await db.QueryFirstOrDefaultAsync<T>(
                "SELECT Id, ProductName, Price, Amount, Total FROM OrderItems WHERE Id=@Id",
                new { Id = id });
        }

        public async Task<(long Count, IEnumerable<OrderItem> Data)> QueryAsync(
            Func<string> filter = null, int? pageIndex = null, int? pageSize = null, string[] orderBy = null, bool hasCountResult = false)
        {
            return await QueryAsync<OrderItem>(filter, pageIndex, pageSize, orderBy, hasCountResult);
        }

        public async Task<(long Count, IEnumerable<T> Data)> QueryAsync<T>(
            Func<string> filter = null, int? pageIndex = null, int? pageSize = null, string[] orderBy = null, bool hasCountResult = false)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();

            var sql1 = "SELECT Id, ProductName, Price, Amount, Total FROM OrderItems";
            var sql2 = "SELECT COUNT(0) FROM OrderItems";
            if (filter != null)
            {
                var filterString = filter();
                if (!string.IsNullOrWhiteSpace(filterString))
                {
                    sql1 = $"{sql1} WHERE {filterString}";
                    sql2 = $"{sql2} WHERE {filterString}";
                }
            }
            if (orderBy != null && orderBy.Any())
                sql1 = $"{sql1} ORDER BY {string.Join(", ", orderBy)}";

            if (pageIndex.HasValue && pageSize.HasValue && pageIndex > 0 && pageSize > 0)
            {
                sql1 = $"{sql1} LIMIT {(pageIndex - 1) * pageSize}, {pageSize}";
            }

            long count = 0;
            IEnumerable<T> data = null;
            var sql = hasCountResult ? $"{sql1};{sql2}" : sql1;
            using (var query = await db.QueryMultipleAsync(sql))
            {
                data = await query.ReadAsync<T>();
                if (hasCountResult)
                    count = await query.ReadSingleAsync<long>();
            }

            return (count, data);
        }

        public async Task AddAsync(OrderItem entity)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            await db.ExecuteAsync("INSERT INTO OrderItems (Id, OrderId, ProductName, Price, Amount, Total) " +
                "VALUES(@Id, @OrderId, @ProductName, @Price, @Amount, @Total)", entity, tran);
        }

        public async Task UpdateAsync(Guid id, OrderItem entity)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            entity.Id = id;
            await db.ExecuteAsync("UPDATE OrderItems SET ProductName=@ProductName, Price=@Price, Amount=@Amount," +
                "Total=@Total WHERE Id=@Id", entity, tran);
        }

        public async Task DeleteAsync(Guid id)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            await db.ExecuteAsync("DELETE FROM OrderItems WHERE Id=@Id", new { Id = id }, tran);
        }
    }
}
