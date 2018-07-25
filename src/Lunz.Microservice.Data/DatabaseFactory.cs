using System;
using System.Data.Common;
using Lunz.Data;
using MySql.Data.MySqlClient;

namespace Lunz.Microservice.Data
{

    /// <summary>
    /// 构建 <see cref="DbConnection"/> 实例。
    /// </summary>
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly Func<string, string> _getConnectionString;

        /// <summary>
        /// 初始化 <see cref="DatabaseFactory"/> 类的新实例。
        /// </summary>
        /// <param name="getConnectionString">
        /// 获取数据库连接字符串。
        /// - User = 用户中心数据库 
        /// </param>
        public DatabaseFactory(Func<string, string> getConnectionString)
        {
            if (getConnectionString == null)
                throw new ArgumentNullException(nameof(getConnectionString));

            _getConnectionString = getConnectionString;
        }

        /// <summary>
        /// 创建 参数 key 对应的 <see cref="DbConnection"/> 实例。
        /// </summary>
        /// <param name="key">关键字。</param>
        /// <returns>成功返回 <see cref="DbConnection"/> 实例。</returns>
        public DbConnection CreateDatabase(string key)
        {
            return ConnectDatabase(_getConnectionString(key));
        }

        private DbConnection ConnectDatabase(string connString)
        {
            return new MySqlConnection(connString);
        }
    }
}
