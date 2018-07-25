using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lunz.Data;
using Lunz.Microservice.Data;
using Lunz.Microservice.ReferenceData.QueryStack.Models;
using Lunz.Microservice.ReferenceData.QueryStack.Repositories;
using Dapper;

namespace Lunz.Microservice.ReferenceData.QueryStack.MySql.Repositories
{
    public class HearFromRepository : IHearFromRepository
    {
        private readonly IAmbientDatabaseLocator _databaseLocator;

        public HearFromRepository(IAmbientDatabaseLocator databaseLocator)
        {
            _databaseLocator = databaseLocator;
        }

        public async Task<HearFrom> FindAsync(Guid id)
        {
            return await FindAsync<HearFrom>(id);
        }

        public async Task<T> FindAsync<T>(Guid id)
        {
            var db = _databaseLocator.GetReferenceDataDatabase();

            return await db.QueryFirstOrDefaultAsync<T>(
                "SELECT Id, Name FROM HearFroms WHERE Id=@Id",
                new { Id = id });
        }

        public async Task<(long Count, IEnumerable<HearFrom> Data)> QueryAsync(
            Func<string> filter = null, int? pageIndex = null, int? pageSize = null, string[] orderBy = null, bool hasCountResult = false)
        {
            return await QueryAsync<HearFrom>(filter, pageIndex, pageSize, orderBy, hasCountResult);
        }

        public async Task<(long Count, IEnumerable<T> Data)> QueryAsync<T>(
            Func<string> filter = null, int? pageIndex = null, int? pageSize = null, string[] orderBy = null, bool hasCountResult = false)
        {
            var db = _databaseLocator.GetReferenceDataDatabase();

            var sql1 = "SELECT Id, Name FROM HearFroms";
            var sql2 = "SELECT COUNT(0) FROM HearFroms";
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

        public async Task<(long Count, IEnumerable<HearFrom> Data)> QueryAsync(Func<(string Sql, dynamic Parameters)> filter = null, int? pageIndex = null, int? pageSize = null, string orderBy = null, bool hasCountResult = false)
        {
            return await QueryAsync<HearFrom>(filter, pageIndex, pageSize, orderBy, hasCountResult);
        }

        public async Task<(long Count, IEnumerable<T> Data)> QueryAsync<T>(Func<(string Sql, dynamic Parameters)> filter = null, int? pageIndex = null, int? pageSize = null, string orderBy = null, bool hasCountResult = false)
        {
            var db = _databaseLocator.GetReferenceDataDatabase();

            var sql1 = "SELECT Id, Name FROM HearFroms";
            var sql2 = "SELECT COUNT(0) FROM HearFroms";
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

        public async Task AddAsync(HearFrom entity)
        {
            var db = _databaseLocator.GetReferenceDataDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            await db.ExecuteAsync("INSERT INTO HearFroms (Id, Name) VALUES(@Id, @Name)", entity, tran);
        }

        public async Task UpdateAsync(Guid id, HearFrom entity)
        {
            var db = _databaseLocator.GetReferenceDataDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            entity.Id = id;
            await db.ExecuteAsync("UPDATE HearFroms SET Name=@Name WHERE Id=@Id", entity, tran);
        }

        public async Task DeleteAsync(Guid id)
        {
            var db = _databaseLocator.GetReferenceDataDatabase();
            var tran = _databaseLocator.GetDbTransaction(db);

            await db.ExecuteAsync("DELETE FROM HearFroms WHERE Id=@Id", new { Id = id }, tran);
        }
    }
}
