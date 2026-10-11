namespace ChannelManagement.Infrastructure;

public interface IMessageClient
{
    Task SubscribeAsync(Func<string, Task> handler);
    Task PublishAsync(string message);
}
