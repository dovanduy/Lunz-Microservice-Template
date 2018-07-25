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
    public class Create
    {
        public class Command : Core.Models.OrderManagement.OrderDetails, IRequest<ResponseResult<OrderDetails>>
        {
            /// <summary>
            /// 订单项目列表
            /// </summary>
            public List<Core.Models.OrderManagement.OrderItem> Items { get; }

            public Command()
            {
                Items = new List<Core.Models.OrderManagement.OrderItem>();
            }
        }

        public class Handler : IRequestHandler<Command, ResponseResult<OrderDetails>>
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

            public async Task<ResponseResult<OrderDetails>> Handle(Command request, CancellationToken cancellationToken)
            {
                var orderId = NewId.NextGuid();

                using (var scope = _databaseScopeFactory.CreateWithTransaction())
                {
                    await _mediator.Send(new CreateOrder(orderId, request.Subject, request.Date, request.HearFromId,
                        request.HearFromName, request.Amount, request.Price, request.Items), cancellationToken);
                    scope.SaveChanges();
                }

                var order = await _repository.FindAsync(orderId);

                return ResponseResult<OrderDetails>.Ok(Mapper.Map<OrderDetails>(order));
            }
        }
    }
}