using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lunz.Microservice.Health
{
	public abstract class ControllerBase : Controller
	{
		protected readonly ILogger Logger;
		protected readonly IMediator Mediator;

		protected ControllerBase(IMediator mediator, ILogger logger)
		{
			Mediator = mediator;
			Logger = logger;
		}

		public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
		{
			return BadRequest(ModelState.Select(x => x.Value).SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
		}
	}
}