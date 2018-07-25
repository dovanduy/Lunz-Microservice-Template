using Lunz.Data;
using Lunz.Microservice.Data;
using Lunz.Microservice.Health.Interface;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lunz.Microservice.Health.Health
{
	public class DBHealthChecker : IHealthChecker
	{
		public string Message { get; set; }

		protected readonly IMediator _mediator;
		private readonly ILogger<DBHealthChecker> _logger;
		private readonly IAmbientDatabaseLocator _databaseLocator;
		protected readonly IDatabaseScopeFactory _databaseScopeFactory;

		public DBHealthChecker(IMediator mediator,
			ILogger<DBHealthChecker> logger,
			IAmbientDatabaseLocator databaseLocator,
			IDatabaseScopeFactory databaseScopeFactory)
		{
			_mediator = mediator;
			_logger = logger;
			_databaseLocator = databaseLocator;
			_databaseScopeFactory = databaseScopeFactory;
		}

		public bool DoCheck()
		{
			bool isOK = false;
			try
			{
				using (var scope = _databaseScopeFactory.CreateWithTransaction())
				{
					//OrderManagement & ReferenceData 为同一数据库，获取dbconnection
					var db = _databaseLocator.GetReferenceDataDatabase();
					isOK = db.State == System.Data.ConnectionState.Open;
				}				
			}
			catch (Exception e)
			{
				Message = $"fatil to check db,msg is = {e.Message},stacktrace = {e.StackTrace}";
				_logger.LogCritical(Message);
			}

			return isOK;			
		}
	}
}
