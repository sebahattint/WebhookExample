using Order.Api.Models;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace Order.Api.Services
{
    public class WebhookNotificationService
    {
        private readonly WebhookSubscriptionService _subscriptionService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private string _queueName = "order_updated_queue";

        public WebhookNotificationService(WebhookSubscriptionService subscriptionService, IHttpClientFactory httpClientFactory)
        {
            _subscriptionService = subscriptionService;
            _httpClientFactory = httpClientFactory;

            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Start()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonSerializer.Deserialize<OrderUpdateMessage>(Encoding.UTF8.GetString(body));

                if (message != null)
                {
                    await NotifySubscribers(message);
                }
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }

        private async Task NotifySubscribers(OrderUpdateMessage message)
        {
            var client = _httpClientFactory.CreateClient();

            var webhookPayload = new
            {
                orderId = message.OrderId,
                status = message.Status,
                updatedDate = message.UpdatedDate
            };

            var jsonPayload = JsonSerializer.Serialize(webhookPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            foreach (var subscription in _subscriptionService.GetAllSubscriptions())
            {
                await client.PostAsync(subscription.WebhookUrl, content);
            }
        }
    }
}
