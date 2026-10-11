namespace ChannelManagement.Shared.Models;

public class ChannelDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    //public bool Active { get; private set; }

}