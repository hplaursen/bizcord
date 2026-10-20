using ChannelManagement.Contracts;
using ChannelManagement.Infrastructure;
using ChannelManagement.Repository;

namespace ChannelManagement.Services;

public class MessagePostedHandler(IMessageClient messageClient, IChannelRepository channelRepository) : IMessageHandler<MessagePostedEvent>
{
    public async Task Handle(MessagePostedEvent message, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var messageCount = channelRepository.IncrementMessageCount(message.ChannelId);
        if (messageCount is null)
            return;

        await messageClient.PublishAsync(new ChannelMessageCountUpdatedEvent
        {
            MessageId = message.MessageId,
            ChannelId = message.ChannelId,
            MessageCount = messageCount.Value,
            ProcessedAt = DateTime.UtcNow
        });
    }
}
