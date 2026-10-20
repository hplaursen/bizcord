using ChannelManagement.Domain.Entities;

namespace ChannelManagement.Repository;

public interface IChannelRepository
{
    IEnumerable<Channel> GetAll();
    Channel? GetById(Guid id);
    Channel Create(Channel channel);
    int? IncrementMessageCount(Guid channelId);
    void Update(Channel channel);
    void Delete(Channel channel);
}
