using System;
using Lunz.Data;
using Lunz.Microservice.ReferenceData.Models.Api;
using Lunz.Microservice.ReferenceData.QueryStack.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Lunz.Kernel;

namespace Lunz.Microservice.ReferenceData.Api.HearFroms
{
    public class Update
    {
        public class Command : HearFromDetails, IRequest<ResponseResult>
        {
        }

        public class Handler : IRequestHandler<Command, ResponseResult>
        {
            protected readonly ILogger<Handler> _logger;
            protected readonly IDatabaseScopeFactory _databaseScopeFactory;
            protected readonly IHearFromRepository _hearFromRepository;

            public Handler(IHearFromRepository hearFromRepository,
                IDatabaseScopeFactory databaseScopeFactory,
                ILogger<Handler> logger)
            {
                _hearFromRepository = hearFromRepository;
                _databaseScopeFactory = databaseScopeFactory;
                _logger = logger;
            }

            public async Task<ResponseResult> Handle(Command request, CancellationToken cancellationToken)
            {
                using (var scope = _databaseScopeFactory.CreateWithTransaction())
                {
                    await _hearFromRepository.UpdateAsync(request.Id, new QueryStack.Models.HearFrom()
                    {
                        Id = request.Id,
                        Name = request.Name,
                    });

                    scope.SaveChanges();
                }
                return ResponseResult.Ok();
            }
        }
    }
}
