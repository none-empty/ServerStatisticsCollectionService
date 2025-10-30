using System.Runtime.InteropServices;
using ServerStatisticsCollectionService.ServerStatisticsCollectorFactories;
using ServerStatisticsCollectionService.ServerStatisticsCollectors;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectorCreator;

public class ServerStatisticsCollectorCreator : IServerStatisticsCollectorCreator
{
    public IServerStatisticsCollector GetServerStatisticsCollectorFor(string osName)
    {
        IServerStatisticsCollectorFactory? factory = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            factory = new LinuxServerStatisticsCollectorFactory();


        
        if(factory is null)
            throw new PlatformNotSupportedException("The current OS is not supported");

        var statisticsCollector = new ServerStatisticsCollector(factory);
        return statisticsCollector;
    }
}