using ServerStatisticsCollectionService.CPUMonitors;
using ServerStatisticsCollectionService.MemoryMonitors;
using ServerStatisticsCollectionService.ServerStatisticsCollectors;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectorFactories;

public interface IServerStatisticsCollectorFactory
{
    public ICPUMonitor CreateCPUMonitor();
    public IMemoryMonitor CreateMemoryMonitor();
}