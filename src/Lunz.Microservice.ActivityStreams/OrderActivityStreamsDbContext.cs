using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lunz.Data.MongoDb;
using Lunz.Microservice.Core.Models.ActivityStreams;

namespace Lunz.Microservice.ActivityStreams
{
    public class OrderActivityStreamsDbContext : MongoDbContextBase
    {
        private Set<OrderActivity> _orderActivities;

        public Set<OrderActivity> OrderActivities
            => _orderActivities ?? (_orderActivities = Get<OrderActivity>(Constants.TableNames.OrderActivities));

        public OrderActivityStreamsDbContext(MongoDbConfiguration settings)
            : base(settings)
        {
        }
    }
}