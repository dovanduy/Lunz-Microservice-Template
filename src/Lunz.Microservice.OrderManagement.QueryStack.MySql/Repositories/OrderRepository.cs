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
    public class OrderRepository : IOrderRepository
    {
        private readonly IAmbientDatabaseLocator _databaseLocator;

        public OrderRepository(IAmbientDatabaseLocator databaseLocator)
        {
            _databaseLocator = databaseLocator;
        }

        public async Task<Order> FindAsync(Guid id)
        {
            return await FindAsync<Order>(id);
        }

        public async Task<T> FindAsync<T>(Guid id)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();

            // https://msdn.microsoft.com/zh-cn/magazine/mt703432.aspx
            return await db.QueryFirstOrDefaultAsync<T>(
                "SELECT Id, Subject, Date, HearFromId, Amount, Price, Total FROM Orders WHERE Id=@Id",
                new { Id = id });
        }

        public async Task<(long Count, IEnumerable<Order> Data)> QueryAsync(
            Func<string> filter = null, int? pageIndex = null, int? pageSize = null, string[] orderBy = null, bool hasCountResult = false)
        {
            return await QueryAsync<Order>(filter, pageIndex, pageSize, orderBy, hasCountResult);
        }

        public async Task<(long Count, IEnumerable<T> Data)> QueryAsync<T>(
            Func<string> filter = null, int? pageIndex = null, int? pageSize = null, string[] orderBy = null, bool hasCountResult = false)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();

            var sql1 = "SELECT Id, Subject, Date, HearFromId, Amount, Price, Total FROM Orders";
            var sql2 = "SELeCT COUNT(0) FROM Orders";
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

        public async Task<(long Count, IEnumerable<Order> Data)> QueryAsync(
            Func<(string Sql, dynamic Parameters)> filter = null, int? pageIndex = null, int? pageSize = null,
            string orderBy = null, bool hasCountResult = false)
        {
            return await QueryAsync<Order>(filter, pageIndex, pageSize, orderBy, hasCountResult);
        }

        public async Task<(long Count, IEnumerable<T> Data)> QueryAsync<T>(
             Func<(string Sql, dynamic Parameters)> filter = null, int? pageIndex = null, int? pageSize = null,
            string orderBy = null, bool hasCountResult = false)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();

            var sql1 = "SELECT Id, Subject, Date, HearFromId, Amount, Price, Total FROM Orders";
            var sql2 = "SELeCT COUNT(0) FROM Orders";
            object parameters = null;
            if (filter != null)
            {
                var filterValue = filter();
                if (!string.IsNullOrWhiteSpace(filterValue.Sql))
                {
                    sql1 = $"{sql1} WHERE {filterValue.Sql}";
                    sql2 = $"{sql2} WHERE {filterValue.Sql}";
                    parameters = filterValue.Parameters;
                }
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
                sql1 = $"{sql1} ORDER BY {orderBy}";

            if (pageIndex.HasValue && pageSize.HasValue && pageIndex > 0 && pageSize > 0)
            {
                sql1 = $"{sql1} LIMIT {(pageIndex - 1) * pageSize}, {pageSize}";
            }

            long count = 0;
            IEnumerable<T> data = null;
            var sql = hasCountResult ? $"{sql1};{sql2}" : sql1;
            using (var query = await db.QueryMultipleAsync(sql, parameters))
            {
                data = await query.ReadAsync<T>();
                if (hasCountResult)
                    count = await query.ReadSingleAsync<long>();
            }

            return (count, data);
        }

        public async Task AddAsync(Order entity)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            await db.ExecuteAsync("INSERT INTO Orders (Id, Subject, Date, HearFromId, Amount, Price, Total) " +
                "VALUES(@Id, @Subject, @Date, @HearFromId, @Amount, @Price, @Total)", entity, tran);
        }

        public async Task UpdateAsync(Guid id, Order entity)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            entity.Id = id;
            await db.ExecuteAsync("UPDATE Orders SET Subject=@Subject, Date=@Date, HearFromId=@HearFromId, " +
                "Amount=@Amount, Price=@Price, Total=@Total WHERE Id=@Id", entity, tran);
        }

        public async Task DeleteAsync(Guid id)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            //删除关联表 OrderItems
            await db.ExecuteAsync("DELETE FROM OrderItems WHERE OrderId=@OrderId", new { OrderId = id }, tran);

            await db.ExecuteAsync("DELETE FROM Orders WHERE Id=@Id", new { Id = id }, tran);
        }

        public async Task PaidAsync(Guid id)
        {
            var db = _databaseLocator.GetOrderManagementDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            await db.ExecuteAsync("UPDATE Orders SET Paid=1 WHERE Id=@Id", new { Id = id }, tran);
        }
    }
}
