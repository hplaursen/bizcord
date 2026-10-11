namespace ChannelManagement.Shared.Models;

public class ChannelDto
{
    public int Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    //public bool Active { get; private set; }

}