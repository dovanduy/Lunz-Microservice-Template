using System;
using System.Threading.Tasks;
using Lunz.Reactive;

namespace Lunz.Microservice.Clients
{
    /// <summary>
    /// Http Client 的基类。
    /// </summary>
    public abstract class HttpClientBase
    {
        /// <summary>
        /// 以观察模式发送请求。
        /// </summary>
        /// <typeparam name="T">返回值类型。</typeparam>
        /// <param name="functionAsync">执行请求的函数。</param>
        /// <returns>返回 <see cref="RxExtensions.Wrapper{T}"/> 类的新实例。</returns>
        /*******************************************************************
         
            var client = new HearFromsClient();

            // 异步调用，不等待。
            client.Observable(() => client.GetAsync(Guid.Empty))
                .WaitAndRetry(
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(20),
                    TimeSpan.FromSeconds(30)
                ).Subscribe(details =>
                {
                    var d = details;
                }, ex =>
                {
                    var c = (ex as SwaggerException);
                    Console.WriteLine($"{c.Message}({c.StatusCode})");
                });
            
            // 同步调用，等待。
            client.Observable(() => client.GetAsync(Guid.Empty))
                .WaitAndRetry(
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5)
                ).Execute(details =>
                {
                    var d = details;
                }, ex =>
                {
                    Console.WriteLine(ex.Message);
                    var c = (ex as SwaggerException);
                    Console.WriteLine($"{c?.Message}({c?.StatusCode})");
                });

        *******************************************************************/
        public RxExtensions.Wrapper<T> Observable<T>(Func<Task<T>> functionAsync)
        {
            return RxExtensions.Observable(functionAsync);
        }
    }
}
