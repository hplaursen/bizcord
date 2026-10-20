using ChannelManagement.Contracts;
using ChannelManagement.Infrastructure;

namespace ChannelManagement.Services;

public class MessagePostedHandler(IMessageClient messageClient)
{
    public async Task Handle(MessagePostedEvent message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await messageClient.PublishAsync(new ChannelMessageCountUpdatedEvent
        {
            MessageId = message.MessageId,
            ChannelId = message.ChannelId,
            MessageCount = 1,
            ProcessedAt = DateTime.UtcNow
        });
    }
}
