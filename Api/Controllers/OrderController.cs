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
        private readonly IRequestClient<SubmitOrder> submitOrderRequestClient;
        private readonly IRequestClient<CheckOrder> checkOrderClient;

        public OrderController(
            ILogger<OrderController> logger,
            IRequestClient<SubmitOrder> submitOrderRequestClient,
            IRequestClient<CheckOrder> checkOrderClient)
        {
            this.logger = logger;
            this.submitOrderRequestClient = submitOrderRequestClient;
            this.checkOrderClient = checkOrderClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
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

        [HttpPost]
        public async Task<IActionResult> Post(Guid id, string customerNumber)
        {
            var (accepted, rejected) = await this.submitOrderRequestClient.GetResponse<OrderSubmissionAccepted, OrderSubmissionRejected>(new()
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
    }
}
