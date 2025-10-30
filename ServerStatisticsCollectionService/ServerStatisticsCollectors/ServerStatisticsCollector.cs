using ServerStatisticsCollectionService.CPUMonitors;
using ServerStatisticsCollectionService.MemoryMonitors;
using ServerStatisticsCollectionService.ServerStatisticsCollectorFactories;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectors;

public class ServerStatisticsCollector : IServerStatisticsCollector
{
    private readonly ICPUMonitor _cpuMonitor;
    private readonly IMemoryMonitor _memoryMonitor;

    public ServerStatisticsCollector(IServerStatisticsCollectorFactory factory)
    {
        _cpuMonitor = factory.CreateCPUMonitor();
        _memoryMonitor = factory.CreateMemoryMonitor();
    }
    public ServerStatistics GetServerStatistics()
    {
        return new ServerStatistics(
         _memoryMonitor.GetMemorUsage(),
         _memoryMonitor.GetAvailableMemory(),
         _cpuMonitor.GetCpuUsage(),
         DateTime.UtcNow
        );
    }
}