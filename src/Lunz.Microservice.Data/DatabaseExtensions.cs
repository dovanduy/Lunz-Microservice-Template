using System.Data.Common;
using Lunz.Data;

namespace Lunz.Microservice.Data
{
    /// <summary>
    /// Database 扩展类。
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// 获取 Order Management 数据库。
        /// </summary>
        /// <param name="helper"><see cref="IDatabaseCollection"/> 实例。</param>
        /// <returns>成功返回 <see cref="DbConnection"/> 实例。</returns>
        public static DbConnection GetOrderManagementDatabase(this IDatabaseCollection helper)
        {
            return helper.Get("OrderManagement");
        }

        /// <summary>
        /// 获取 Order Management 数据库。
        /// </summary>
        /// <param name="helper"><see cref="IAmbientDatabaseLocator"/> 实例。</param>
        /// <returns>成功返回 <see cref="DbConnection"/> 实例。</returns>
        public static DbConnection GetOrderManagementDatabase(this IAmbientDatabaseLocator helper)
        {
            return helper.Get("OrderManagement");
        }

        /// <summary>
        /// 获取 Order Management 数据库。
        /// </summary>
        /// <param name="helper"><see cref="IDatabaseCollection"/> 实例。</param>
        /// <returns>成功返回 <see cref="DbConnection"/> 实例。</returns>
        public static DbConnection GetReferenceDataDatabase(this IDatabaseCollection helper)
        {
            return helper.Get("ReferenceData");
        }

        /// <summary>
        /// 获取 Reference Data 数据库。
        /// </summary>
        /// <param name="helper"><see cref="IAmbientDatabaseLocator"/> 实例。</param>
        /// <returns>成功返回 <see cref="DbConnection"/> 实例。</returns>
        public static DbConnection GetReferenceDataDatabase(this IAmbientDatabaseLocator helper)
        {
            return helper.Get("ReferenceData");
        }
    }
}