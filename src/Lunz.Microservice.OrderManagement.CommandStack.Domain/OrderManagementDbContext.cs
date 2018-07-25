using Lunz.Data.MongoDb;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models;
using Microsoft.Extensions.Options;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain
{
    public class OrderManagementDbContext : MongoDbContextBase
    {
        private Set<Order> _orders;

        public Set<Order> Orders => _orders ?? (_orders = Get<Order>(Constants.TableNames.Orders));

        public OrderManagementDbContext(MongoDbConfiguration settings)
            : base(settings)
        {
        }
    }
}