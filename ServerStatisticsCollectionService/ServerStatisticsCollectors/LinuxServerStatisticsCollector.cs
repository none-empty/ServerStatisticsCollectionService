 
 

using ServerStatisticsCollectionService.CPUMonitors.LinuxCPUMonitors;
using ServerStatisticsCollectionService.MemoryMonitors.LinuxMemoryMonitors;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectors;

public class LinuxServerStatisticsCollector : IServerStatisticsCollector
{
    private readonly ILinuxCPUMonitor _linuxCpuMonitor;
    private readonly ILinuxMemoryMonitor _linuxMemoryMonitor;

    public LinuxServerStatisticsCollector(ILinuxCPUMonitor linuxCpuMonitor, ILinuxMemoryMonitor linuxMemoryMonitor)
    {
        _linuxCpuMonitor = linuxCpuMonitor;
        _linuxMemoryMonitor = linuxMemoryMonitor;
    }
    public ServerStatistics GetServerStatistics()
    {
        return new ServerStatistics(
         _linuxMemoryMonitor.GetMemorUsage(),
         _linuxMemoryMonitor.GetAvailableMemory(),
         _linuxCpuMonitor.GetCpuUsage(),
         DateTime.UtcNow
        );
    }
}