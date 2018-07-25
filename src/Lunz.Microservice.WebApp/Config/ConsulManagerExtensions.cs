using Consul;
using Lunz.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lunz.Microservice.WebApp.Config
{
    public static class ConsulManagerExtensions
    {
		public static void ConfigureConsul(this IServiceCollection services, IConfigurationManager configuration)
		{
			string clientUrl = configuration.GetConfig("TEST1.Consul.Setting:ClientUrl");
			var client = new ConsulClient((c) =>
			{
				c.Address = new Uri(clientUrl);
			});
			services.AddSingleton(p => client);

			RegisterServiceToConsul(client, configuration);
		}

		private static void RegisterServiceToConsul(ConsulClient client, IConfigurationManager aplconfig)
		{
			string serviceName = aplconfig.GetConfig("AgentService.Setting:ServiceName");
			string serverAddr = aplconfig.GetConfig("AgentService.Setting:Address");
			string serverPort = aplconfig.GetConfig("AgentService.Setting:Port");

			var registration = new AgentServiceRegistration()
			{
				Name = serviceName,
				Tags = new[]
				{
					serviceName
				},
				Address = serverAddr,
				Port = Convert.ToInt32(serverPort),
				Checks = new[]
				{
					new AgentServiceCheck
					{
						HTTP = $"http://{serverAddr}:{serverPort}/health",
						Interval = TimeSpan.FromSeconds(10),
						TLSSkipVerify = false,
						Timeout = TimeSpan.FromSeconds(1)
					}
				}
			};

			client.Config.Datacenter = aplconfig.GetConfig("TEST1.Consul.Setting:DataCenter");
			client.Agent.ServiceRegister(registration);
		}
	}
}
