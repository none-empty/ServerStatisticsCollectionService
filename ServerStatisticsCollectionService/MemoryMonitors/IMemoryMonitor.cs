namespace ServerStatisticsCollectionService.MemoryMonitors;

public interface IMemoryMonitor
{
    public Task<(double memoryUsage,double availableMemory)> GetMemorUsageAndAvailableMemory();
     
}