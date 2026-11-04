using System.Text;
using RabbitMQ.Client;

namespace ServerStatisticsCollectionService.RabbitMQSenders;

public class RabbitMQSender : IRabbitMQSender, IAsyncDisposable
{
    private const string QueueName = "server_statistics_queue";
    private readonly IConnectionFactory _connectionFactory;
    private readonly SemaphoreSlim _channelLock = new(1, 1);
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _disposed;

    public RabbitMQSender(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task QueuePayload(string payload, CancellationToken cancellationToken = default)
    {
        // A shared publishing channel must not be used by concurrent callers.
        await _channelLock.WaitAsync(cancellationToken);
        try
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            var channel = await GetChannelAsync(cancellationToken);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: QueueName,
                mandatory: true,
                basicProperties: new BasicProperties { Persistent = true },
                body: Encoding.UTF8.GetBytes(payload),
                cancellationToken: cancellationToken);
        }
        finally
        {
            _channelLock.Release();
        }
    }

    private async Task<IChannel> GetChannelAsync(CancellationToken cancellationToken)
    {
        _connection ??= await _connectionFactory.CreateConnectionAsync(cancellationToken);

        // Keep the same objects during network recovery; the client restores them.
        if (_channel is not null)
        {
            return _channel;
        }

        var channel = await _connection.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true),
            cancellationToken);
        try
        {
            // RabbitMQ 4.3+ rejects non-durable, non-exclusive queues by default.
            // Receivers must declare the same durable queue.
            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            _channel = channel;
            return channel;
        }
        catch
        {
            await channel.DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _channelLock.WaitAsync();
        try
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            try
            {
                if (_channel is not null)
                {
                    await _channel.DisposeAsync();
                }
            }
            finally
            {
                if (_connection is not null)
                {
                    await _connection.DisposeAsync();
                }
            }
        }
        finally
        {
            _channelLock.Release();
        }
    }
}
