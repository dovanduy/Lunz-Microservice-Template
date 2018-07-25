using AutoMapper;
using Lunz.Data;
using Lunz.Data.Extensions.Query;
using Lunz.Data.Extensions.Sort;
using Lunz.Data.Extensions.Sql;
using Lunz.Kernel;
using Lunz.Microservice.Core.Models;
using Lunz.Microservice.OrderManagement.Models.Api;
using Lunz.Microservice.OrderManagement.QueryStack.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lunz.Microservice.OrderManagement.Api.Orders
{
    public class Query
    {
        public class Command : IRequest<ResponseResult<Response>>
        {
            /// <summary>
            /// 过滤条件
            /// </summary>
            public QueryGroup Filter { get; set; }
            /// <summary>
            /// 页号，如 1
            /// </summary>
            public int? PageIndex { get; set; }
            /// <summary>
            /// 每页行数，如 10
            /// </summary>
            public int? PageSize { get; set; }
            /// <summary>
            /// 排序规则
            /// </summary>
            public List<PagingSort> Sort { get; set; }

            public Command()
            {
                Sort = new List<PagingSort>();
                Filter = new QueryGroup() { Op = "and" };
            }
        }

        /// <summary>
        /// 订单数据列表
        /// </summary>
        public class Response : IPaginationModel<OrderDetails>
        {
            /// <summary>
            /// 总行数
            /// </summary>
            public long Count { get; }
            /// <summary>
            /// 当前页号
            /// </summary>
            public int? PageIndex { get; }
            /// <summary>
            /// 每页行数
            /// </summary>
            public int? PageSize { get; }
            /// <summary>
            /// 订单列表
            /// </summary>
            public IEnumerable<OrderDetails> Data { get; }

            public Response(IEnumerable<OrderDetails> data, long count, int? pageIndex, int? pageSize)
            {
                Data = data;
                Count = count;
                PageIndex = pageIndex;
                PageSize = pageSize;
            }
        }

        public class Handler : IRequestHandler<Command, ResponseResult<Response>>
        {
            protected readonly ILogger<Handler> _logger;
            protected readonly IDatabaseScopeFactory _databaseScopeFactory;
            private readonly IOrderRepository _orderRepository;

            public Handler(IDatabaseScopeFactory databaseScopeFactory, ILogger<Handler> logger,
                IOrderRepository orderRepository)
            {
                _databaseScopeFactory = databaseScopeFactory;
                _logger = logger;
                _orderRepository = orderRepository;
            }

            public async Task<ResponseResult<Response>> Handle(Command request,
                CancellationToken cancellationToken)
            {
                using (var scope = _databaseScopeFactory.CreateWithTransaction())
                {
                    var filter = request.Filter.ToSql<OrderDetails>();
                    var result = await _orderRepository.QueryAsync<OrderDetails>(() => filter.ToTuble(),
                        request.PageIndex, request.PageSize, request.Sort.ToSql(), true);
                    return ResponseResult<Response>.Ok(new Response(result.Data, result.Count,
                        request.PageIndex, request.PageSize));
                }
            }
        }
    }
}
