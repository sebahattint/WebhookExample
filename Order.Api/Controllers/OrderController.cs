using Microsoft.AspNetCore.Mvc;
using Order.Api.Models;
using Order.Api.Services;

namespace Order.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly RabbitMqPublisher _publisher;
        private readonly WebhookSubscriptionService _subscriptionService;

        public OrderController(RabbitMqPublisher publisher, WebhookSubscriptionService subscriptionService)
        {
            _publisher = publisher;
            _subscriptionService = subscriptionService;
        }

        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody] string webhookUrl)
        {
            var subscription = new WebhookSubscription
            {
                WebhookUrl = webhookUrl
            };
            _subscriptionService.AddSubscription(subscription);
            return Ok(subscription);
        }

        [HttpPost("update")]
        public IActionResult UpdateOrder([FromBody] OrderUpdateRequest orderUpdateRequest)
        {
            var message = new OrderUpdateMessage
            {
                OrderId = orderUpdateRequest.OrderId,
                Status = orderUpdateRequest.Status,
                UpdatedDate = orderUpdateRequest.UpdatedDate
            };

            _publisher.PublishOrderUpdate(message);

            return Ok("Sipariş başarıyla güncellendi ve RabbitMq kuyruğuna gönderildi.");
        }
    }
}
