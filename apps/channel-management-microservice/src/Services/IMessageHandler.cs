namespace ChannelManagement.Services;

public interface IMessageHandler<in TMessage>
{
    Task Handle(TMessage message, CancellationToken cancellationToken);
}
