namespace ServerStatisticsCollectionService.MemoryMonitors;

public interface IMemoryMonitor
{
    public Task<double> GetMemorUsage();
    public Task<double> GetAvailableMemory();
}