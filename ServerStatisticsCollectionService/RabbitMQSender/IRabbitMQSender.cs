namespace ServerStatisticsCollectionService.RabbitMQSender;

public interface IRabbitMQSender
{
    public Task QueuePayload(string message);
}