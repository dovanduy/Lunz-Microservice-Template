using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lunz.Microservice.ReferenceData.Api.HearFroms;
using Lunz.Microservice.ReferenceData.Models.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace Lunz.Microservice.ReferenceData.Api.ReferenceData
{
    [Route("api/v1/reference-data")]
    public class ReferenceDataController : ControllerBase
    {
        public ReferenceDataController(IMediator mediator, ILogger<ReferenceDataController> logger)
            : base(mediator, logger)
        {
        }

        [Route("/api/v1/reference-data")]
        [HttpPost]
        [SwaggerTag("基础数据")]
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取从哪里听说全部数据集合。
        /// </summary>
        /// <param name="query"></param>
        /// <returns>返回从哪里听说数据集合。</returns>
        [Route("hear-froms")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HearFromDetails>), 200)]
        [SwaggerTag("基础数据")]
        public async Task<IEnumerable<HearFromDetails>> Get(HearFrom.Command query)
        {
            var result = await Mediator.Send(query);

            return result;
        }
    }
}