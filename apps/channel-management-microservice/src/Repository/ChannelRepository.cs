using ChannelManagement.Domain.Entities;

namespace ChannelManagement.Repository;

public class ChannelRepository : IChannelRepository
{
    private readonly Dictionary<Guid, Channel> _channels = [];
    public IEnumerable<Channel> GetAll()
    {
        return _channels.Values;
    }

    public Channel? GetById(Guid id)
    {
        return _channels.TryGetValue(id, out Channel? channel) ? channel : null;
    }

    public Channel Create(Channel channel)
    {
        if (channel.Id == Guid.Empty)
            channel.Id = Guid.NewGuid();

        _channels[channel.Id] = channel;
        return channel;
    }

    public int? IncrementMessageCount(Guid channelId)
    {
        if (!_channels.TryGetValue(channelId, out Channel? channel))
            return null;

        channel.MessageCount++;
        return channel.MessageCount;
    }

    public void Update(Channel channel)
    {
        if (_channels.ContainsKey(channel.Id))
            _channels[channel.Id] = channel;
    }

    public void Delete(Channel channel)
    {
        if (_channels.ContainsKey(channel.Id))
            _channels.Remove(channel.Id);
    }
}
