namespace ServerStatisticsCollectionService.RabbitMQSenders;

public interface IRabbitMQSender
{
    public Task QueuePayload(string message);
}