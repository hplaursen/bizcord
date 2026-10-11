
namespace ChannelManagement.Infrastructure;

public class MessageSubscriber : BackgroundService
{
    private readonly IMessageClient _messageClient;

    public MessageSubscriber(IMessageClient messageClient)
    {
        _messageClient = messageClient;
    }

    private Task HandleMessageAsync(string message)
    {
        Console.WriteLine($"Received: {message}");
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageClient.SubscribeAsync(HandleMessageAsync);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
        
        Console.WriteLine("Message handler is stopping..");
    }
}
