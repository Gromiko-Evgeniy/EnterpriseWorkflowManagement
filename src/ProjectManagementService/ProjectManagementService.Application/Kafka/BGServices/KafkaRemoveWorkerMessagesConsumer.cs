using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectManagementService.Application.CQRS.WorkerCommands;

namespace ProjectManagementService.Application.Kafka.BGServices;

public class KafkaRemoveWorkerMessagesConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _topic;

    public KafkaRemoveWorkerMessagesConsumer(IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory)
    {
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration.GetSection("KafkaBootstrapServer").Get<string>(),
            GroupId = "PMService",
            AutoOffsetReset = AutoOffsetReset.Earliest // Latest
        };

        _topic = "remove-workers";
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Func<string, Task> messageValueAsyncProcessing = async (email) =>
        {
            var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new RemoveWorkerCommand(email));
        };

        await BaseKafkaConsumerFunctionality.StartConsuming(stoppingToken,
            _consumerConfig, _topic, messageValueAsyncProcessing);
    }
}
