namespace ChannelManagement.Contracts;

public class ChannelMessageCountUpdatedEvent
{
    public Guid MessageId { get; set; }
    public Guid ChannelId { get; set; }
    public int MessageCount { get; set; }
    public DateTime ProcessedAt { get; set; }
}
