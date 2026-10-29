using System.Text.Json;
using ServerStatisticsCollectionService.RabbitMQSenders;
using ServerStatisticsCollectionService.ServerStatisticsCollectorCreators;
using ServerStatisticsCollectionService.ServerStatisticsCollectors;

namespace ServerStatisticsCollectionService;

public class ServiceLoop
{
    private readonly int _samplingIntervalMilliSeconds;
    private readonly IServerStatisticsCollector _statisticsCollector;
    private readonly IRabbitMQSender _rabbitMQClient;
    private bool _serviceIsRunning;
    
    public ServiceLoop(IServerStatisticsCollectorCreator collectorCreator,IRabbitMQSender rabbitMQClient)
    {
       _samplingIntervalMilliSeconds = (
           int.TryParse(Environment.GetEnvironmentVariable("SamplingIntervalSeconds"),out var intervalSeconds) 
               ? intervalSeconds:5) * 1000;
       
       _serviceIsRunning = true;

       _statisticsCollector = collectorCreator.GetServerStatisticsCollector();

       _rabbitMQClient = rabbitMQClient;
    }
    
    public async Task StartService()
    {
        while (_serviceIsRunning)
        {
           var data = await _statisticsCollector.GetServerStatistics();
           var message = JsonSerializer.Serialize(data);
           
           await _rabbitMQClient.QueuePayload(message);
           
           await Task.Delay(_samplingIntervalMilliSeconds);
        }
    }
}