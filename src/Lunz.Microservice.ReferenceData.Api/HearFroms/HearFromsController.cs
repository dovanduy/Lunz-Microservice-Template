using Lunz.Microservice.ReferenceData.Models.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lunz.Microservice.AspNetCore;
using Lunz.Microservice.ReferenceData.Api.ReferenceData;

namespace Lunz.Microservice.ReferenceData.Api.HearFroms
{
    [Route("api/v1/hear-from")]
    public class HearFromsController : ControllerBase
    {
        public HearFromsController(IMediator mediator, ILogger<HearFromsController> logger)
            : base(mediator, logger)
        {
        }

        /// <summary>
        /// 获取从哪里听说列表。
        /// </summary>
        /// <param name="query"></param>
        /// <returns>返回从哪里听说数据列表。</returns>
        [Route("~/api/v1/hear-froms")]
        [HttpGet]
        [ProducesResponseType(typeof(Query.Response), 200)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("从哪里听说")]
        public async Task<ActionResult<Query.Response>> Get(Query.Command query)
        {
            // TODO: 这是一个测试，获取登录用户。
            var userDetails = Request.UserDetails();

            var result = await Mediator.Send(query);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            return result.Data;
        }

        /// <summary>
        /// 获取从哪里听说数据。
        /// </summary>
        /// <param name="command"></param>
        /// <returns>返回从哪里听说数据。</returns>
        [Route("{id:guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(HearFromDetails), 200)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerResponse(404, null, Description = "参数 Id 无效。")]
        [SwaggerTag("从哪里听说")]
        public async Task<ActionResult<HearFromDetails>> Get(Details.Command command)
        {
            if (command.Id == Guid.Empty)
                return BadRequest();

			var result = await Mediator.Send(command);

			if (result == null)
				return NotFound();

			return result;
		}

        /// <summary>
        /// 创建从哪里听说数据。
        /// </summary>
        /// <param name="command"></param>
        /// <returns>返回从哪里听说数据，并返回详细数据地址（URL）。</returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(HearFromDetails), 201)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("从哪里听说")]
        public async Task<IActionResult> Post([FromBody]Create.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Mediator.Send(command);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(Get), new { result.Data.Id }, result.Data);
        }

        /// <summary>
        /// 编辑从哪里听说数据。 
        /// </summary>
        /// <param name="id">要编辑的从哪里听说 Id</param>
        /// <param name="command"></param>
        /// <returns>无返回值。</returns>
        [Route("{id:guid}")]
        [HttpPut]
        [ProducesResponseType(204)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("从哪里听说")]
        public async Task<IActionResult> Update(Guid? id, [FromBody]Update.Command command)
        {
            if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await Mediator.Send(command);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            return Ok();
        }

        /// <summary>
        /// 删除从哪里听说数据。
        /// </summary>
        /// <param name="command"></param>
        /// <returns>无返回值。</returns>
        [Route("{id:guid}")]
        [HttpDelete]
        [ProducesResponseType(204)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("从哪里听说")]
        public async Task<IActionResult> Delete(Delete.Command command)
        {
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await Mediator.Send(command);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}