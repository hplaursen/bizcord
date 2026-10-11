using ChannelManagement.Domain.Entities;

namespace ChannelManagement.Repository;

public class ChannelRepository : IChannelRepository
{
    private readonly Dictionary<int, Channel> _channels = [];
    public IEnumerable<Channel> GetAll()
    {
        return _channels.Values;
    }

    public Channel? GetById(int id)
    {
        return _channels.TryGetValue(id, out Channel? channel) ? channel : null;
    }

    public Channel Create(Channel channel)
    {
        int newId = _channels.Count > 0 ? _channels.Keys.Max() + 1 : 1;
        channel.Id = newId;
        _channels[newId] = channel;
        return channel;
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