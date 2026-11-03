using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using ServerStatisticsCollectionService.Options;
using ServerStatisticsCollectionService.RabbitMQSenders;
using ServerStatisticsCollectionService.ServerStatisticsCollectorCreators;
using ServerStatisticsCollectionService.ServerStatisticsCollectors;

namespace ServerStatisticsCollectionService;

public class ServiceLoop : BackgroundService
{
    private readonly int _samplingIntervalMilliSeconds;
    private readonly IServerStatisticsCollector _statisticsCollector;
    private readonly IRabbitMQSender _rabbitMQClient;
    private readonly ILogger<ServiceLoop> _logger;

    public ServiceLoop(IServerStatisticsCollectorCreator collectorCreator, IRabbitMQSender rabbitMQClient,
        IOptions<ServerStatisticsOptions> options, ILogger<ServiceLoop> logger)
    {
        _samplingIntervalMilliSeconds = options.Value.SamplingIntervalSeconds * 1000;
        _statisticsCollector = collectorCreator.GetServerStatisticsCollector();
        _rabbitMQClient = rabbitMQClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var data = await _statisticsCollector.GetServerStatistics();
            var message = JsonSerializer.Serialize(data);

            try
            {
                await _rabbitMQClient.QueuePayload(message, stoppingToken);
            }
            catch (Exception exception) when (!stoppingToken.IsCancellationRequested &&
                                              (exception is BrokerUnreachableException or IOException or TimeoutException ||
                                               exception is OperationInterruptedException
                                               {
                                                   ShutdownReason: { Initiator: ShutdownInitiator.Library } or
                                                       { ReplyCode: Constants.ConnectionForced }
                                               }))
            {
                _logger.LogWarning(exception,
                    "Could not publish statistics. The next sample will be attempted after the sampling interval.");
            }

            await Task.Delay(_samplingIntervalMilliSeconds, stoppingToken);
        }
    }
}
