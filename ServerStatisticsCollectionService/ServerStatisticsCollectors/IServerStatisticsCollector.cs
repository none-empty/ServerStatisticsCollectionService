namespace ServerStatisticsCollectionService.ServerStatisticsCollectors;

public interface IServerStatisticsCollector
{
    public ServerStatistics GetServerStatistics();
}