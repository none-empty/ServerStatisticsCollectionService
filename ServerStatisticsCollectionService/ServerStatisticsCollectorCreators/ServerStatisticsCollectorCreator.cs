using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using ServerStatisticsCollectionService.Options;
using ServerStatisticsCollectionService.ServerStatisticsCollectorFactories;
using ServerStatisticsCollectionService.ServerStatisticsCollectors;

namespace ServerStatisticsCollectionService.ServerStatisticsCollectorCreators;

public class ServerStatisticsCollectorCreator : IServerStatisticsCollectorCreator
{
    private readonly IOptions<ServerStatisticsOptions> _options;

    public ServerStatisticsCollectorCreator(IOptions<ServerStatisticsOptions> options)
    {
        _options = options;
    }

    public IServerStatisticsCollector GetServerStatisticsCollector()
    {
        IServerStatisticsCollectorFactory? factory = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            factory = new LinuxServerStatisticsCollectorFactory();


        
        if(factory is null)
            throw new PlatformNotSupportedException("The current OS is not supported");

        var statisticsCollector = new ServerStatisticsCollector(factory, _options);
        return statisticsCollector;
    }
}
