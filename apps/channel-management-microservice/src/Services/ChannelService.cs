using ChannelManagement.Domain.Entities;
using ChannelManagement.Repository;

namespace ChannelManagement.Services
{
    public class ChannelService(IChannelRepository repository) : IChannelService
    {
        public Task<Channel?> GetChannelByIdAsync(int channelId) {
           return Task.FromResult(repository.GetById(channelId));
        }

        public Task<IEnumerable<Channel>> GetAllChannelsAsync() {
            return Task.FromResult(repository.GetAll());
        }

        public Task<Channel> CreateChannelAsync(Channel channel) {
            Channel? createdChannel = repository.Create(channel);
            return Task.FromResult(createdChannel);
        }

        public Task<Channel?> UpdateChannelAsync(int channelId, Channel channel) {
            Channel? existingChannel = GetChannelByIdAsync(channelId).Result;
            if (existingChannel == null)
                return Task.FromResult<Channel?>(null);

            channel.Id = channelId;
            repository.Update(channel);
            return Task.FromResult<Channel?>(channel);
        }

        public Task<bool> DeleteChannelAsync(int channelId) {
            Channel? existingChannel = repository.GetById(channelId);
            if (existingChannel == null)
                return Task.FromResult(false);

            repository.Delete(existingChannel);
            return Task.FromResult(true);
        }
    }
}