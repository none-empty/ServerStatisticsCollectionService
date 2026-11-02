using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ServerStatisticsCollectionService.Options;

namespace ServerStatisticsCollectionService.RabbitMQSenders;

public class RabbitMQSender : IRabbitMQSender
{
    private readonly RabbitMQOptions _options;

    public RabbitMQSender(IOptions<ServerStatisticsOptions> options)
    {
        _options = options.Value.RabbitMQ;
    }

    public async Task QueuePayload(string payload)
    {
        var factory = new ConnectionFactory() 
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };
         
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();


            const string queueName = "server_statistics_queue";

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(payload);

            await channel.BasicPublishAsync(exchange: string.Empty,
                routingKey: queueName,
                mandatory: false,
                basicProperties: new BasicProperties { Persistent = true },
                body: body);

    }
}
