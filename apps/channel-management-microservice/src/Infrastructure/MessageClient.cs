
using EasyNetQ;

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

    public async Task PublishAsync(string message)
    {
        await _bus.PubSub.PublishAsync<string>(message);
    }

}
