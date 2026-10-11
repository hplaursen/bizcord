namespace ChannelManagement.Domain.Entities;

public class Channel
{
    public int Id { get; internal set; }

    public string Name { get; internal set; } = string.Empty;
    public string? Description { get; internal set; }

    public DateTime CreatedAt { get; internal set; } = DateTime.UtcNow;

    public bool Active { get; internal set; } = true;

}