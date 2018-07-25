using System;
using System.Threading;
using System.Threading.Tasks;
using Lunz.Data;
using Lunz.Microservice.ReferenceData.Models.Api;
using Lunz.Microservice.ReferenceData.QueryStack.MySql.Repositories;
using Lunz.Microservice.ReferenceData.QueryStack.Repositories;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.ReferenceData.Api.HearFroms
{
	public class Details
	{
		public class Command : IRequest<HearFromDetails>
		{
			/// <summary>
			/// 要获取从哪里听说数据的 Id。
			/// </summary>
			public Guid? Id { get; set; }
		}

		public class Handler : IRequestHandler<Command, HearFromDetails>
		{
			private readonly ILogger<Handler> _logger;
			private readonly IDatabaseScopeFactory _databaseScopeFactory;
			private readonly IHearFromRepository _hearFromRepository;

			public Handler(IHearFromRepository hearFromRepository,
				IDatabaseScopeFactory databaseScopeFactory,
				ILogger<Handler> logger)
			{
				_hearFromRepository = hearFromRepository;
				_databaseScopeFactory = databaseScopeFactory;
				_logger = logger;
			}

			public async Task<HearFromDetails> Handle(Command request, CancellationToken cancellationToken)
			{
				using (var scope = _databaseScopeFactory.CreateReadOnly())
				{
					var hearfrom = await _hearFromRepository.FindAsync<HearFromDetails>(request.Id.Value);
					return hearfrom;
				}
			}
		}
	}
}