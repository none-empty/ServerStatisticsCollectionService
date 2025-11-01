using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerStatisticsCollectionService.RabbitMQSenders;
using ServerStatisticsCollectionService.ServerStatisticsCollectorCreators;

namespace ServerStatisticsCollectionService;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args) .ConfigureServices((context, services) => 
            { 
               services.AddTransient<IServerStatisticsCollectorCreator, ServerStatisticsCollectorCreator>(); 
               services.AddTransient<IRabbitMQSender, RabbitMQSender>(); 
                services.AddSingleton<ServiceLoop>(); 
            }) .Build();

        await using var scope = host.Services.CreateAsyncScope();
        var service = host.Services.GetRequiredService<ServiceLoop>();
        await service.StartService();
    }
}