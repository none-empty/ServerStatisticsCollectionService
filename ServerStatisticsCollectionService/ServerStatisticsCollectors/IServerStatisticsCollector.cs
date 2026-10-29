namespace ServerStatisticsCollectionService.ServerStatisticsCollectors;

public interface IServerStatisticsCollector
{
    public Task<ServerStatistics> GetServerStatistics();
}