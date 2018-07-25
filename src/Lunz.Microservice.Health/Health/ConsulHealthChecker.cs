using Consul;
using Lunz.Microservice.Health.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lunz.Microservice.Health.Health
{
	public class ConsulHealthChecker : IHealthChecker
	{
		public string Message { get; set; }

		private readonly ConsulClient _client;
		private readonly ILogger<ConsulHealthChecker> _logger;

		public ConsulHealthChecker(ConsulClient client, ILogger<ConsulHealthChecker> logger)
		{
			_client = client;
			_logger = logger;
		}

		public bool DoCheck()
		{
			bool isOK = false;
			try
			{
				var leader = _client.Status.Leader().GetAwaiter().GetResult();
				if (!string.IsNullOrEmpty(leader))
				{
					isOK = true;
				}
			}
			catch (Exception e)
			{
				Message = $"fatil to check consul,msg is = {e.Message},stacktrace = {e.StackTrace}";
				_logger.LogCritical(Message);
			}

			return isOK;
		}




	}
}
