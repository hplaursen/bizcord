
using ChannelManagement.Services;
using ChannelManagement.Contracts;

namespace ChannelManagement.Infrastructure;

public class MessageSubscriber : BackgroundService
{
    private readonly IMessageClient _messageClient;
    private readonly IMessageHandler<MessagePostedEvent> _messagePostedHandler;

    public MessageSubscriber(IMessageClient messageClient, IMessageHandler<MessagePostedEvent> messagePostedHandler)
    {
        _messageClient = messageClient;
        _messagePostedHandler = messagePostedHandler;
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
