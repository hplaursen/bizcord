using ChannelManagement.Domain.Entities;

namespace ChannelManagement.Services
{
    public interface IChannelService
    {
        Task<Channel?> GetChannelByIdAsync(Guid channelId);
        Task<IEnumerable<Channel>> GetAllChannelsAsync();
        Task<Channel> CreateChannelAsync(Channel channel);
        Task<Channel?> UpdateChannelAsync(Guid channelId, Channel channel);
        Task<bool> DeleteChannelAsync(Guid channelId);
    }
}
