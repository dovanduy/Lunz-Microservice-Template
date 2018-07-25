using System;
using System.Collections;
using System.Collections.Generic;
using Lunz.Microservice.Core.Models.ActivityStreams;
using Lunz.Microservice.OrderManagement.Models.Api;
using ServiceStack;

namespace Lunz.Microservice.OrderManagement.Models
{
    public class Order
    {
        [Route("/orders", "GET")]
        public class Query : IReturn<IEnumerable<OrderDetails>>
        {
            // TODO: 查询，排序，分页参数
            public string Filter { get; set; }
            public int? PageIndex { get; set; }
            public int? PageSize { get; set; }
            public string[] OrderBy { get; set; }
        }

        [Route("/order/{Id}", "GET")]
        public class Get : IReturn<OrderDetails>
        {
            public Guid Id { get; set; }
        }

        [Route("/orders", "POST")]
        public class Create : Core.Models.OrderManagement.OrderDetails, IReturn<OrderDetails>
        {
            public List<Core.Models.OrderManagement.OrderItem> Items { get; set; }

            public Create()
            {
                Items = new List<Core.Models.OrderManagement.OrderItem>();
            }
        }

        // TODO: 集成 Update, Delete
        [Route("/orders", "PUT")]
        public class Update : IReturn<OrderDetails>
        {
            public Guid Id { get; set; }
            public string Subject { get; set; }
        }


        [Route("/orders/{Id}", "DELETE")]
        public class Delete : IReturnVoid
        {
            public Guid Id { get; set; }
        }

        [Route("/order/{Id}/pay", "POST")]
        public class Pay : IReturnVoid
        {
            public Guid Id { get; set; }
            public decimal Payment { get; set; }
        }

        [Route("/order/{Id}/activities", "GET")]
        public class Activities : IReturn<IEnumerable<Activity>>
        {
            public Guid Id { get; set; }
        }
    }
}
