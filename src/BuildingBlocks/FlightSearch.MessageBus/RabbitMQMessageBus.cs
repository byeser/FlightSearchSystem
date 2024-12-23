using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace FlightSearch.MessageBus;
public class RabbitMQMessageBus : IMessageBus, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQMessageBus> _logger;

    public RabbitMQMessageBus(IConfiguration configuration, ILogger<RabbitMQMessageBus> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"],
            UserName = configuration["RabbitMQ:Username"],
            Password = configuration["RabbitMQ:Password"]
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        var queueName = typeof(T).Name;
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);

        _logger.LogInformation("Message published to queue: {QueueName}", queueName);
    }

    public async Task SubscribeAsync<T>(string queueName, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : class
    {
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(json);

                if (message != null)
                {
                    await handler(message);
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message from queue: {QueueName}", queueName);
                _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}