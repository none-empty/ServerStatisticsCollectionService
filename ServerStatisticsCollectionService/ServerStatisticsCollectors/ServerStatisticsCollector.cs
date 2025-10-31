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
        
        (double memoryUsage, double availableMemory) = await _memoryMonitor.GetMemorUsageAndAvailableMemory();
        double cpuUsage = await _cpuMonitor.GetCpuUsage();
        
        return new ServerStatistics(
            memoryUsage,
            availableMemory,
            cpuUsage,
            DateTime.UtcNow
        );
        
    }
}