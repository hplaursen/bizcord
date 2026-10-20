using ChannelManagement.Contracts;
using ChannelManagement.Infrastructure;
using ChannelManagement.Services;
using Xunit;

namespace ChannelManagement.Tests;

public class MessagePostedHandlerTests
{
    [Fact]
    public async Task Handle_PublishesResultEvent_WithOriginalMessageAndChannelIds()
    {
        var messageId = Guid.NewGuid();
        var channelId = Guid.NewGuid();
        var client = new FakeMessageClient();
        var handler = new MessagePostedHandler(client);

        await handler.Handle(CreateMessage(messageId, channelId), CancellationToken.None);

        var published = Assert.IsType<ChannelMessageCountUpdatedEvent>(Assert.Single(client.Published));
        Assert.Equal(messageId, published.MessageId);
        Assert.Equal(channelId, published.ChannelId);
        Assert.Equal(1, published.MessageCount);
        Assert.InRange(published.ProcessedAt, DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5));
    }

    [Fact]
    public async Task Handle_CanConsumeMinimumValidContract()
    {
        var client = new FakeMessageClient();
        var handler = new MessagePostedHandler(client);

        var exception = await Record.ExceptionAsync(() => handler.Handle(CreateMessage(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None));

        Assert.Null(exception);
        Assert.Single(client.Published);
    }

    [Fact]
    public async Task Handle_WhenCancelled_DoesNotPublish()
    {
        var client = new FakeMessageClient();
        var handler = new MessagePostedHandler(client);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => handler.Handle(CreateMessage(Guid.NewGuid(), Guid.NewGuid()), cancellation.Token));

        Assert.Empty(client.Published);
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
