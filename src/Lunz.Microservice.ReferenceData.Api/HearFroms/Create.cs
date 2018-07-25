using AutoMapper;
using Lunz.Data;
using Lunz.Kernel;
using Lunz.Microservice.ReferenceData.Models.Api;
using Lunz.Microservice.ReferenceData.QueryStack.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Lunz.Microservice.ReferenceData.Api.HearFroms
{
    public class Create
    {
        public class Command : IRequest<ResponseResult<HearFromDetails>>
        {
            /// <summary>
            /// 要创建从哪里听说数据的名称 Name
            /// </summary>
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Command, ResponseResult<HearFromDetails>>
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

            public async Task<ResponseResult<HearFromDetails>> Handle(Command request, CancellationToken cancellationToken)
            {
                var id = NewId.NextGuid();

                if (string.IsNullOrWhiteSpace(request.Name))
                    return ResponseResult<HearFromDetails>.Error("名称不能为空。");

                using (var scope = _databaseScopeFactory.CreateWithTransaction())
                {
                    await _hearFromRepository.AddAsync(new QueryStack.Models.HearFrom()
                    {
                        Id = id,
                        Name = request.Name,
                    });

                    scope.SaveChanges();
                }

                var hearFrom = Mapper.Map<HearFromDetails>(request);
                hearFrom.Id = id;

                return ResponseResult<HearFromDetails>.Ok(hearFrom);
            }
        }
    }
}