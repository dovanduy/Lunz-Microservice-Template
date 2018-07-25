using Lunz.Data;
using Lunz.Kernel;
using Lunz.Microservice.Core.Models.OrderManagement;
using Lunz.Microservice.OrderManagement.Contracts.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lunz.Microservice.OrderManagement.Api.Orders
{
    public class Delete
    {
        public class Command : IRequest<ResponseResult>
        {
            /// <summary>
            /// 要删除订单的 ID。
            /// </summary>
            public Guid? Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, ResponseResult>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IDatabaseScopeFactory _databaseScopeFactory;
            private readonly IMediator _mediator;

            public Handler(IDatabaseScopeFactory databaseScopeFactory, IMediator mediator,
                ILogger<Handler> logger)
            {
                _databaseScopeFactory = databaseScopeFactory;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<ResponseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!request.Id.HasValue)
                    return ResponseResult.Error("Id 不能为空");

                using (var scope = _databaseScopeFactory.CreateWithTransaction())
                {
                    await _mediator.Send(new DeleteOrder() { OrderId = request.Id.Value }, cancellationToken);
                    scope.SaveChanges();
                }

                return ResponseResult.Ok();
            }
        }
    }
}
