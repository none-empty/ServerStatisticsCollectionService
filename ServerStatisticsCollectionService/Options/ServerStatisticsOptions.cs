namespace ServerStatisticsCollectionService.Options;

public class ServerStatisticsOptions
{
    public int SamplingIntervalSeconds { get; set; } = 5;
    public string ServerIdentifier { get; set; } = "producer1";
    public RabbitMQOptions RabbitMQ { get; set; } = new();
}
