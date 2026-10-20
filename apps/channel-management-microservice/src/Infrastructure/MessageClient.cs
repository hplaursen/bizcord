
using EasyNetQ;
using ChannelManagement.Contracts;

namespace ChannelManagement.Infrastructure;

public class MessageClient : IMessageClient
{
    private readonly IBus _bus;

    public MessageClient(IBus bus)
    {
        _bus = bus;
    }

    public async Task SubscribeAsync(Func<string, Task> handler)
    {
        await _bus.PubSub.SubscribeAsync<string>("channel-management", handler);
                         Console.WriteLine($"Subscriber is starting.. channel-management test");
    }

    public async Task SubscribeToMessagesAsync(Func<MessagePostedEvent, CancellationToken, Task> handler, CancellationToken cancellationToken)
    {
        await _bus.PubSub.SubscribeAsync<MessagePostedEvent>("channel-management", message => handler(message, cancellationToken));
    }

    public async Task PublishAsync(string message)
    {
        await _bus.PubSub.PublishAsync<string>(message);
    }

    public async Task PublishAsync<T>(T message) where T : class
    {
        await _bus.PubSub.PublishAsync(message);
    }

}
