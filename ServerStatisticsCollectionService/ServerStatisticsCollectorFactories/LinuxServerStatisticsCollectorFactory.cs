using ServerStatisticsCollectionService.CPUMonitors;
using ServerStatisticsCollectionService.MemoryMonitors;
using ServerStatisticsCollectionService.ServerStatisticsCollectors;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectorFactories;

public class LinuxServerStatisticsCollectorFactory : IServerStatisticsCollectorFactory
{
    public ICPUMonitor CreateCPUMonitor()
    {
        return new LinuxCPUMonitor();
    }

    public IMemoryMonitor CreateMemoryMonitor()
    {
        return new LinuxMemoryMonitor();
    }
}