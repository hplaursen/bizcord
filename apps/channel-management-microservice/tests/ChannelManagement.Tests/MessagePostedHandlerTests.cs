using ChannelManagement.Contracts;
using ChannelManagement.Domain.Entities;
using ChannelManagement.Infrastructure;
using ChannelManagement.Repository;
using ChannelManagement.Services;
using Xunit;

namespace ChannelManagement.Tests;

public class MessagePostedHandlerTests
{
    [Fact]
    public async Task Handle_PublishesResultEvent_WithOriginalMessageAndChannelIds()
    {
        var messageId = Guid.NewGuid();
        var client = new FakeMessageClient();
        var repository = new ChannelRepository();
        var channel = repository.Create(new Channel());
        var handler = new MessagePostedHandler(client, repository);

        await handler.Handle(CreateMessage(messageId, channel.Id), CancellationToken.None);

        var singlePublished = Assert.Single(client.Published); // Is there exactly one published event?
        var published = Assert.IsType<ChannelMessageCountUpdatedEvent>(singlePublished); // Is it the expected event type?
        Assert.Equal(messageId, published.MessageId); // Is the MessageId preserved?
        Assert.Equal(channel.Id, published.ChannelId); // Is the ChannelId preserved?
        Assert.Equal(1, published.MessageCount); // Is the count 1 after the first message?

        await handler.Handle(CreateMessage(Guid.NewGuid(), channel.Id), CancellationToken.None);

        var secondPublished = Assert.IsType<ChannelMessageCountUpdatedEvent>(client.Published[1]);
        Assert.Equal(2, secondPublished.MessageCount); // Does the count increase to 2 after the next message?
    }

    [Fact]
    public async Task Handle_CanConsumeMinimumValidContract()
    {
        var client = new FakeMessageClient();
        var repository = new ChannelRepository();
        var channel = repository.Create(new Channel());
        var handler = new MessagePostedHandler(client, repository);

        var exception = await Record.ExceptionAsync(() => handler.Handle(CreateMessage(Guid.NewGuid(), channel.Id), CancellationToken.None));

        Assert.Null(exception);
        Assert.Single(client.Published);
    }

    private static MessagePostedEvent CreateMessage(Guid messageId, Guid channelId) => new()
    {
        MessageId = messageId,
        ChannelId = channelId,
        AuthorId = Guid.NewGuid(),
        Content = "Hello world",
        PostedAt = DateTime.UtcNow
    };

    private sealed class FakeMessageClient : IMessageClient
    {
        public List<object> Published { get; } = [];

        public Task SubscribeAsync(Func<string, Task> handler) => Task.CompletedTask;
        public Task PublishAsync(string message) => Task.CompletedTask;
        public Task SubscribeToMessagesAsync(Func<MessagePostedEvent, CancellationToken, Task> handler, CancellationToken cancellationToken) => Task.CompletedTask;

        public Task PublishAsync<T>(T message) where T : class
        {
            Published.Add(message);
            return Task.CompletedTask;
        }
    }
}
