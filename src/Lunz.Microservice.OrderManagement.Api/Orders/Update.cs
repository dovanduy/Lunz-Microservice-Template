using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Lunz.Data;
using Lunz.Kernel;
using Lunz.Microservice.OrderManagement.Contracts.Commands;
using Lunz.Microservice.OrderManagement.Models.Api;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.OrderManagement.Api.Orders
{
    public class Update
    {
        public class Command : Core.Models.OrderManagement.OrderDetails, IRequest<ResponseResult>
        {
            /// <summary>
            /// 要编辑订单的 Id。
            /// </summary>
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, ResponseResult>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IDatabaseScopeFactory _databaseScopeFactory;
            private readonly CommandStack.Domain.Repositories.IOrderRepository _repository;
            private readonly IMediator _mediator;

            public Handler(CommandStack.Domain.Repositories.IOrderRepository repository,
                IDatabaseScopeFactory databaseScopeFactory,
                IMediator mediator,
                ILogger<Handler> logger)
            {
                _repository = repository;
                _databaseScopeFactory = databaseScopeFactory;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<ResponseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                using (var scope = _databaseScopeFactory.CreateWithTransaction())
                {
                    await _mediator.Send(new UpdateOrder(request.Id, request.Subject, request.Date,
                        request.HearFromId, request.HearFromName, request.Amount, request.Price), cancellationToken);
                    scope.SaveChanges();
                }

                return ResponseResult.Ok();
            }
        }
    }
}
