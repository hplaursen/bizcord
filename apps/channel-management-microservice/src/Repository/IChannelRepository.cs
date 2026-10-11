using ChannelManagement.Domain.Entities;

namespace ChannelManagement.Repository;

public interface IChannelRepository
{
    IEnumerable<Channel> GetAll();
    Channel? GetById(int id);
    Channel Create(Channel channel);
    void Update(Channel channel);
    void Delete(Channel channel);
}