using System;
using System.Linq;
using System.Text.RegularExpressions;
using Lunz.Microservice.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.OrderManagement.Api
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

        public new IActionResult BadRequest(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                throw new Exception($"{nameof(modelState)} 参数当前是有效的。");
            }

            var errors = ModelState.Select(x => x.Value)
                .SelectMany(x => x.Errors)
                .Select(x => ParseError(x.ErrorMessage))
                .ToList();

            if (errors.Any(x => x.HttpStatusCode == "204"))
                return NoContent();

            if (errors.Any(x => x.HttpStatusCode == "404"))
                return NotFound(errors.Select(x => x.ErrorMessage));

            return BadRequest(errors.Select(x => x.ErrorMessage));
        }

        protected (string HttpStatusCode, string ErrorMessage) ParseError(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                return (null, errorMessage);

            var regex = new Regex(@"^\[(\d{3}\.?\d{0,2})\]");
            var match = regex.Match(errorMessage);

            if (!match.Success)
                return (null, errorMessage);

            var code = match.Groups[0].Value.Substring(1, match.Length - 2);
            var message = errorMessage.Substring(match.Length);

            return (code, message);
        }
    }
}
