using ChannelManagement.Domain.Entities;

namespace ChannelManagement.Services
{
    public interface IChannelService
    {
        Task<Channel?> GetChannelByIdAsync(int channelId);
        Task<IEnumerable<Channel>> GetAllChannelsAsync();
        Task<Channel> CreateChannelAsync(Channel channel);
        Task<Channel?> UpdateChannelAsync(int channelId, Channel channel);
        Task<bool> DeleteChannelAsync(int channelId);
    }
}