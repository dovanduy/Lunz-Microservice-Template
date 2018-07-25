namespace Lunz.Microservice.Core.Models.OrderManagement
{
    /// <summary>
    /// 订单项目
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
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