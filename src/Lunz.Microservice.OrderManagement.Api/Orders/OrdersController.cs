using System;
using System.Linq;
using System.Threading.Tasks;
using Lunz.Microservice.Core.Models;
using Lunz.Microservice.OrderManagement.Models.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;

namespace Lunz.Microservice.OrderManagement.Api.Orders
{
    [Route("api/v1/order")]
    public class OrdersController : ControllerBase
    {
        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
            : base(mediator, logger)
        {
        }

        /// <summary>
        /// 获取订单列表。
        /// </summary>
        /// <param name="query"></param>
        /// <returns>返回订单列表。</returns>
        [Route("~/api/v1/orders")]
        [HttpGet]
        [ProducesResponseType(typeof(Query.Response), 200)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("订单")]
        public async Task<ActionResult<Query.Response>> Get(Query.Command query)
        {
            var result = await Mediator.Send(query);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            return result.Data;
        }

        /// <summary>
        /// 获取订单。
        /// </summary>
        /// <param name="command"></param>
        /// <returns>返回订单数据。</returns>
        [Route("{id:guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(OrderDetails), 200)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerResponse(404, null, Description = "参数 Id 无效。")]
        [SwaggerTag("订单")]
        public async Task<ActionResult<OrderDetails>> Get(Details.Command command)
        {
			if (command.Id == Guid.Empty)
				return BadRequest();

			var result = await Mediator.Send(command);

			if (result == null)
				return NotFound();

			return result;
		}

        /// <summary>
        /// 创建订单。
        /// </summary>
        /// <param name="command"></param>
        /// <returns>返回订单数据，并返回详细数据地址（URL）。</returns>
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(OrderDetails), 201)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("订单")]
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
        /// 编辑订单。
        /// </summary>
        /// <param name="id">要编辑订单的 Id</param>
        /// <param name="command"></param>
        /// <returns>无返回值。</returns>
        [Route("{id:guid}")]
        [HttpPut]
        [ProducesResponseType(204)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("订单")]
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
        /// 删除订单。
        /// </summary>
        /// <param name="command"></param>
        /// <returns>无返回值。</returns>
        [Route("{id:guid}")]
        [HttpDelete]
        [ProducesResponseType(204)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("订单")]
        public async Task<IActionResult> Delete(Delete.Command command)
        {
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await Mediator.Send(command);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            return Ok();
        }

        /// <summary>
        /// 支付订单。
        /// </summary>
        /// <param name="command"></param>
        /// <returns>无返回值。</returns>
        [Route("{id:guid}/pay")]
        [HttpPost]
        [ProducesResponseType(200)]
        [SwaggerResponse(400, typeof(string[]), Description = "参数无效。")]
        [SwaggerTag("订单")]
        public async Task<ActionResult> Pay(Pay.Command command)
        {
            var result = await Mediator.Send(command);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            return Ok();
        }
    }
}
