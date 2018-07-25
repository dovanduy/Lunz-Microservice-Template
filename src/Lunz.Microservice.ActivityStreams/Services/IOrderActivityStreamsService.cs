using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lunz.Microservice.Core.Models.ActivityStreams;

namespace Lunz.Microservice.ActivityStreams.Services
{
    public interface IOrderActivityStreamsService
    {
        Task<IEnumerable<Activity>> GetActivities(Guid orderId);
    }
}