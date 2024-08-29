using Order.Api.Models;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace Order.Api.Services
{
    public class RabbitMqPublisher
    {
        private readonly IConnection _connection;
        private string _queueName = "order_updated_queue";

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
        }

        public void PublishOrderUpdate(OrderUpdateMessage message)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
            }
        }
    }
}
