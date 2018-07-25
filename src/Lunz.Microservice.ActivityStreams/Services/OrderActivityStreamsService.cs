using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lunz.Microservice.Core.Models.ActivityStreams;

namespace Lunz.Microservice.ActivityStreams.Services
{
    public class OrderActivityStreamsService : IOrderActivityStreamsService
    {
        private readonly OrderActivityStreamsDbContext _dbContext;

        public OrderActivityStreamsService(OrderActivityStreamsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Activity>> GetActivities(Guid orderId)
        {
            return _dbContext.OrderActivities.Query.Where(x => x.OrderId == orderId);
        }
    }
}