namespace ServerStatisticsCollectionService.RabbitMQSenders;

public interface IRabbitMQSender
{
    Task QueuePayload(string message, CancellationToken cancellationToken = default);
}
