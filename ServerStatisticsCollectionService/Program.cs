using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ServerStatisticsCollectionService.Options;
using ServerStatisticsCollectionService.RabbitMQSenders;
using ServerStatisticsCollectionService.ServerStatisticsCollectorCreators;

namespace ServerStatisticsCollectionService;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.Configure<ServerStatisticsOptions>(context.Configuration);
                services.AddTransient<IServerStatisticsCollectorCreator, ServerStatisticsCollectorCreator>();
                services.AddSingleton<IConnectionFactory>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<ServerStatisticsOptions>>().Value.RabbitMQ;
                    return new ConnectionFactory
                    {
                        HostName = options.HostName,
                        Port = options.Port,
                        UserName = options.UserName,
                        Password = options.Password,
                        AutomaticRecoveryEnabled = true,
                        TopologyRecoveryEnabled = true
                    };
                });
                services.AddSingleton<IRabbitMQSender, RabbitMQSender>();
                services.AddHostedService<ServiceLoop>();
            })
            .Build();

        await host.RunAsync();
    }
}
