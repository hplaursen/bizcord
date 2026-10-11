namespace ChannelManagement.Domain.Entities;

public class Channel
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public bool Active { get; private set; } = true;


    public Channel (string name)
    {
        this.Name = name;
    }
}