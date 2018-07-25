using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lunz.Kernel.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Lunz.Microservice.AspNetCore
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;

        public HttpGlobalExceptionFilter(
            IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(
                new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var errors = new List<string>();
            HttpStatusCode httpStatusCode;

            if (typeof(ValidationException).IsAssignableFrom(context.Exception.GetType()))
            {
                httpStatusCode = HttpStatusCode.BadRequest;
                errors.Add(context.Exception.Message);
            }
            else
            {
                httpStatusCode = HttpStatusCode.InternalServerError;
                errors.Add("服务器发生未知错误，请重试。");
            }

            if (_env.IsDevelopment())
            {
                errors.AddRange(GetExceptionMessage(context.Exception));
                errors.Add(context.Exception.StackTrace);
            }

            context.Result = new ObjectResult(errors)
            {
                StatusCode = (int)httpStatusCode
            };
            context.HttpContext.Response.StatusCode = (int)httpStatusCode;

            context.ExceptionHandled = true;
        }

        private IEnumerable<string> GetExceptionMessage(Exception ex)
        {
            if (ex == null)
                yield break;

            yield return ex.Message;

            if (ex.InnerException != null)
            {
                foreach (var item in GetExceptionMessage(ex.InnerException))
                {
                    yield return item;
                }
            }
        }
    }
}