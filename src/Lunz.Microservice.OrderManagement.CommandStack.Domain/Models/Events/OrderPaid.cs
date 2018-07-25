using System;

namespace Lunz.Microservice.OrderManagement.CommandStack.Domain.Models.Events
{
    /// <summary>
    /// 订单支付完成。
    /// </summary>
    public class OrderPaid : OrderDomainEventBase
    {
        /// <summary>
        /// 初始化 <see cref="OrderPaid"/> 对象的实例。
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="payment"></param>
        public OrderPaid(Guid aggregateId, decimal payment)
            : base(aggregateId)
        {
            Payment = payment;
        }

        /// <summary>
        /// 获取或设置付款金额。
        /// </summary>
        public decimal Payment { get; }
    }
}
