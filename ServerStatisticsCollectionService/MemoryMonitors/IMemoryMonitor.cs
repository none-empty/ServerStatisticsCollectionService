namespace ServerStatisticsCollectionService.MemoryMonitors;

public interface IMemoryMonitor
{
    public double GetMemorUsage();
    public double GetAvailableMemory();
}