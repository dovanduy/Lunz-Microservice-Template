using System;
using System.Collections.Generic;
using System.Text;

namespace Lunz.Microservice.Health.Interface
{
    public interface IHealthChecker
    {
		string Message { get; set; }

		bool DoCheck();
    }
}
