using System.Runtime.InteropServices;
using ServerStatisticsCollectionService.ServerStatisticsCollectorCreators;
using ServerStatisticsCollectionService.ServerStatisticsCollectorFactories;
using ServerStatisticsCollectionService.ServerStatisticsCollectors;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectorCreators;

public class ServerStatisticsCollectorCreator : IServerStatisticsCollectorCreator
{
    public IServerStatisticsCollector GetServerStatisticsCollector()
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