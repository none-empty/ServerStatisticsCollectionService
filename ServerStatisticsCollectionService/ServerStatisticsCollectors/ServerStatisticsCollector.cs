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
    public async Task<ServerStatistics> GetServerStatistics()
    {
        return new ServerStatistics(
         await _memoryMonitor.GetMemorUsage(),
         await _memoryMonitor.GetAvailableMemory(),
         await _cpuMonitor.GetCpuUsage(),
         DateTime.UtcNow
        );
    }
}