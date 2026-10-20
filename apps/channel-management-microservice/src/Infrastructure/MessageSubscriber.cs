
using ChannelManagement.Services;

namespace ChannelManagement.Infrastructure;

public class MessageSubscriber : BackgroundService
{
    private readonly IMessageClient _messageClient;
    private readonly MessagePostedHandler _messagePostedHandler;

    public MessageSubscriber(IMessageClient messageClient, MessagePostedHandler messagePostedHandler)
    {
        _messageClient = messageClient;
        _messagePostedHandler = messagePostedHandler;
    }

    private Task HandleMessageAsync(string message)
    {
        Console.WriteLine($"Received: {message}");
        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageClient.SubscribeToMessagesAsync(_messagePostedHandler.Handle, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
        
        Console.WriteLine("Message handler is stopping..");
    }
}
