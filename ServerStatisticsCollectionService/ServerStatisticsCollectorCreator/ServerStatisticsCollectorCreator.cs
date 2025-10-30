using System.Runtime.InteropServices;
using ServerStatisticsCollectionService.ServerStatisticsCollectors;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectorCreator;

public class ServerStatisticsCollectorCreator : IServerStatisticsCollectorCreator
{
    public IServerStatisticsCollector GetServerStatisticsCollectorFor(string osName)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new LinuxServerStatisticsCollector();

        throw new PlatformNotSupportedException("The current OS is not supported");
    }
}