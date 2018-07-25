using Lunz.Data;
using Lunz.Kernel;
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
    public class Pay
    {
        public class Command : IRequest<ResponseResult>
        {
            /// <summary>
            /// 要付款订单的 Id。
            /// </summary>
            public Guid Id { get; set; }
            /// <summary>
            /// 付款金额
            /// </summary>
            public decimal Payment { get; set; }
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
                using (var scope = _databaseScopeFactory.CreateWithTransaction())
                {
                    await _mediator.Send(new PayOrder()
                    {
                        OrderId = request.Id,
                        Payment = request.Payment
                    }, cancellationToken);
                    scope.SaveChanges();
                }

                return ResponseResult.Ok();
            }
        }
    }
}
