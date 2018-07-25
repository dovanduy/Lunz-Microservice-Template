using System;
using System.Collections.Generic;
using Lunz.Microservice.ReferenceData.Models.Api;
using ServiceStack;

namespace Lunz.Microservice.ReferenceData.Models
{
    public class HearFrom
    {
        [Route("/hear-froms", "GET")]
        public class Query : IReturn<IEnumerable<HearFromDetails>>
        {
            // TODO: 查询，排序，分页参数
            public string Filter { get; set; }
            public int? PageIndex { get; set; }
            public int? PageSize { get; set; }
            public string[] OrderBy { get; set; }
        }

        [Route("/hear-froms/{Id}", "GET")]
        public class Get : IReturn<HearFromDetails>
        {
            public Guid Id { get; set; }
        }

        [Route("/hear-froms", "POST")]
        public class Create : IReturn<HearFromDetails>
        {
            public string Name { get; set; }
        }

        //[Route("/hear-froms", "PUT")]
        //public class Update : IReturn<HearFromDetails>
        //{
        //    public Guid Id { get; set; }
        //    public string Name { get; set; }
        //}


        //[Route("/hear-froms/{Id}", "DELETE")]
        //public class Delete : IReturnVoid
        //{
        //    public Guid Id { get; set; }
        //}
    }
}
