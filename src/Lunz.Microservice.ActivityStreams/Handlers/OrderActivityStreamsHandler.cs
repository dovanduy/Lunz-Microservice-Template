using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Lunz.Microservice.Core.Models.ActivityStreams;
using Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events;
using MassTransit;
using MediatR;
using Lunz.Microservice.ActivityStreams.Services;

namespace Lunz.Microservice.ActivityStreams.Handlers
{
    public class OrderActivityStreamsHandler
        : ActivityStreamsHandler<OrderHtmlContext>
        , INotificationHandler<OrderEntered>
    {
        private readonly OrderActivityStreamsDbContext _dbContext;

        public OrderActivityStreamsHandler(OrderActivityStreamsDbContext dbContext, OrderHtmlContext htmlContext)
            : base(htmlContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(OrderEntered message, CancellationToken cancellationToken)
        {
            var activity = NewActivity<OrderActivity>(message.User, message.Timestamp, ActivityType.Create);

            activity.Id = NewId.NextGuid();
            activity.OrderId = message.AggregateId;
            activity.Summary = $"{message.User?.Name} 创建了订单 #{message.AggregateId}";
            activity.Content = Html.UserLink(message.User) +
                                " 创建了订单 " +
                               Html.OrderLink(message.AggregateId);

            activity.MediaType = MediaTypeNames.Text.Html;

            await _dbContext.OrderActivities.Add(activity);
        }
    }
}