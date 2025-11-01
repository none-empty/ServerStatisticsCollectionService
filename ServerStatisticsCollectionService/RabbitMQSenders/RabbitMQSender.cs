using System.Text;
using RabbitMQ.Client;

namespace ServerStatisticsCollectionService.RabbitMQSenders;

public class RabbitMQSender : IRabbitMQSender
{
    public async Task QueuePayload(string payload)
    {
        var factory = new ConnectionFactory() 
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ__HOSTNAME") ?? "localhost",
            Port = int.TryParse(Environment.GetEnvironmentVariable("RABBITMQ__PORT"), out var port) ? port : 5672,
            UserName = Environment.GetEnvironmentVariable("RABBITMQ__USERNAME") ?? "guest",
            Password = Environment.GetEnvironmentVariable("RABBITMQ__PASSWORD") ?? "guest"
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