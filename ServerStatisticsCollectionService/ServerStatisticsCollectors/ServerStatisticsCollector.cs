using Microsoft.Extensions.Options;
using ServerStatisticsCollectionService.CPUMonitors;
using ServerStatisticsCollectionService.MemoryMonitors;
using ServerStatisticsCollectionService.Options;
using ServerStatisticsCollectionService.ServerStatisticsCollectorFactories;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectors;

public class ServerStatisticsCollector : IServerStatisticsCollector
{
    private readonly ICPUMonitor _cpuMonitor;
    private readonly IMemoryMonitor _memoryMonitor;
    private readonly string _serverIdentifier;

    public ServerStatisticsCollector(IServerStatisticsCollectorFactory factory,
        IOptions<ServerStatisticsOptions> options)
    {
        _cpuMonitor = factory.CreateCPUMonitor();
        _memoryMonitor = factory.CreateMemoryMonitor();
        _serverIdentifier = options.Value.ServerIdentifier;
    }
    public async Task<ServerStatistics> GetServerStatistics()
    {
        
        (double memoryUsage, double availableMemory) = await _memoryMonitor.GetMemorUsageAndAvailableMemory();
        double cpuUsage = await _cpuMonitor.GetCpuUsage();
        
        return new ServerStatistics(
            memoryUsage,
            availableMemory,
            cpuUsage,
            DateTime.UtcNow,
            _serverIdentifier
        );
        
    }
}
