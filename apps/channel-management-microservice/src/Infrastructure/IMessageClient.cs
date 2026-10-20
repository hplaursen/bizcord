namespace ChannelManagement.Infrastructure;

using ChannelManagement.Contracts;

public interface IMessageClient
{
    Task SubscribeAsync(Func<string, Task> handler);
    Task PublishAsync(string message);
    Task SubscribeToMessagesAsync(Func<MessagePostedEvent, CancellationToken, Task> handler, CancellationToken cancellationToken);
    Task PublishAsync<T>(T message) where T : class;
}
