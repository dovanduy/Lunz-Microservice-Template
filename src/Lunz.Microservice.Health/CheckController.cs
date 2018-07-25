using Lunz.Microservice.Health.Health;
using Lunz.Microservice.Health.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lunz.Microservice.Health
{
	[Route("api/v1/[controller]")]
	public class CheckController: ControllerBase
	{
		private readonly IEnumerable<IHealthChecker> _healthCheckers;

		public CheckController(IMediator mediator, 
			ILogger<CheckController> logger,
			IEnumerable<IHealthChecker> healthCheckers
			)
			: base(mediator, logger)
		{
			_healthCheckers = healthCheckers;
		}

		[HttpGet("/health")]
		public IActionResult Healthe()
		{
			foreach (var item in _healthCheckers)
			{
				if (!item.DoCheck())
				{
					throw new Exception(item.Message);					
				}
			}
			return Ok();
		}

	}
}
