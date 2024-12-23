namespace FlightSearch.MessageBus;
public interface IMessageBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;
    Task SubscribeAsync<T>(string queueName, Func<T, Task> handler, CancellationToken cancellationToken = default) where T : class;
}
