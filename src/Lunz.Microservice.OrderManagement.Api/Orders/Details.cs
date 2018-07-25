using AutoMapper;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Repositories;
using Lunz.Microservice.OrderManagement.Models.Api;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lunz.Microservice.OrderManagement.Api.Orders
{
    public class Details
    {
        public class Command : IRequest<OrderDetails>
        {
            /// <summary>
            /// 要获取订单的 Id。
            /// </summary>
            public Guid? Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, OrderDetails>
        {
            private readonly ILogger<Handler> _logger;
            private readonly IOrderRepository _repository;

            public Handler(IOrderRepository repository, ILogger<Handler> logger)
            {
                _repository = repository;
                _logger = logger;
            }

            public async Task<OrderDetails> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await _repository.FindAsync(request.Id.Value);
                return order == null ? null : Mapper.Map<OrderDetails>(order);
            }
        }
    }
}