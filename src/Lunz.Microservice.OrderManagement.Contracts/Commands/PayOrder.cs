using System;
using MediatR;

namespace Lunz.Microservice.OrderManagement.Contracts.Commands
{
    /// <summary>
    /// 支付订单。
    /// </summary>
    public class PayOrder : IRequest<PayOrderResponse>
    {
        /// <summary>
        /// 获取或设置订单Id。
        /// </summary>
        public Guid OrderId { get; set; }
        /// <summary>
        /// 获取或设置付款金额。
        /// </summary>
        public decimal Payment { get; set; }
    }

    /// <summary>
    /// 支付订单结果。
    /// </summary>
    public class PayOrderResponse
    {
        /// <summary>
        /// 获取或设置订单Id。
        /// </summary>
        public Guid OrderId { get; set; }
    }
}