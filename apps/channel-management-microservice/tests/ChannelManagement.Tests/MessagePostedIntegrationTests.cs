using ChannelManagement.Contracts;
using ChannelManagement.Infrastructure;
using ChannelManagement.Services;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ChannelManagement.Tests;

public class MessagePostedIntegrationTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task MessagePostedEvent_IsConsumed_AndResultEventPublished()
    {
        var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5673";
        var messageId = Guid.NewGuid();
        var resultReceived = new TaskCompletionSource<ChannelMessageCountUpdatedEvent>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddEasyNetQ($"host={host};port={port}");
        await using var provider = services.BuildServiceProvider();

        var bus = provider.GetRequiredService<IBus>();
        var client = new MessageClient(bus);
        var subscriber = new MessageSubscriber(client, new MessagePostedHandler(client));

        await bus.PubSub.SubscribeAsync<ChannelMessageCountUpdatedEvent>(
            $"channel-management-tests-{Guid.NewGuid():N}",
            result => resultReceived.TrySetResult(result));

        await subscriber.StartAsync(timeout.Token);
        try
        {
            await client.PublishAsync(new MessagePostedEvent
            {
                MessageId = messageId,
                ChannelId = Guid.NewGuid(),
                AuthorId = Guid.NewGuid(),
                Content = "Hello world",
                PostedAt = DateTime.UtcNow
            });

            var result = await resultReceived.Task.WaitAsync(timeout.Token);
            Assert.Equal(messageId, result.MessageId);
            Assert.Equal(1, result.MessageCount);
        }
        finally
        {
            await subscriber.StopAsync(CancellationToken.None);
        }
    }
}
