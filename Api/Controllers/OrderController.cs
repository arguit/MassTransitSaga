using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController
        : ControllerBase
    {
        private readonly ILogger<OrderController> logger;

        private readonly IRequestClient<SubmitOrder> submitOrderClient;
        private readonly IRequestClient<CheckOrder> checkOrderClient;
        private readonly IRequestClient<CloseOrder> closeOrderClient;

        public OrderController(
            ILogger<OrderController> logger,
            IRequestClient<SubmitOrder> submitOrderClient,
            IRequestClient<CheckOrder> checkOrderClient,
            IRequestClient<CloseOrder> closeOrderClient)
        {
            this.logger = logger;
            this.submitOrderClient = submitOrderClient;
            this.checkOrderClient = checkOrderClient;
            this.closeOrderClient = closeOrderClient;
        }

        [HttpGet("~/CheckOrder")]
        public async Task<IActionResult> CheckOrder(Guid id)
        {
            var (status, notFound) = await checkOrderClient.GetResponse<OrderStatus, OrderNotFound>(new()
            {
                OrderId = id
            });

            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return NotFound(response.Message);
            }
        }

        [HttpPost("~/SubmitOrder")]
        public async Task<IActionResult> SubmitOrder(Guid id, string customerNumber)
        {
            var (accepted, rejected) = await this.submitOrderClient.GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(new()
            {
                OrderId = id,
                Timestamp = InVar.Timestamp,
                CustomerNumber = customerNumber
            });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Accepted(response.Message);
            }
            else
            {
                var response = await rejected;
                return BadRequest(response.Message);
            }

        }

        [HttpPost("~/CloseOrder")]
        public async Task<IActionResult> CloseOrder(Guid id)
        {
            var (closed, notFound) = await this.closeOrderClient.GetResponse<OrderCloseAccepted, OrderNotFound>(new()
            {
                OrderId = id,
                Timestamp = InVar.Timestamp
            });

            if (closed.IsCompletedSuccessfully)
            {
                var response = await closed;
                return Accepted(response.Message);
            }
            else
            {
                var response = await notFound;
                return BadRequest(response.Message);
            }
        }
    }
}
