using System;

namespace Lunz.Microservice.Core.Models.OrderManagement
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderDetails
    {
        /// <summary>
        /// 订单摘要
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 从何处了解本站 Id
        /// </summary>
        public Guid? HearFromId { get; set; }
        /// <summary>
        /// 从何处了解本站名称
        /// </summary>
        public string HearFromName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Total { get; set; }
    }
}
