using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectManagementService.Application.CQRS.CustomerCommands;
using ProjectManagementService.Application.DTOs;

namespace ProjectManagementService.Application.Kafka.BGServices;

public class KafkaAddCustomerMessagesConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _topic;

    public KafkaAddCustomerMessagesConsumer(IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory)
    {
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration.GetSection("KafkaBootstrapServer").Get<string>(),
            GroupId = "PMService",
            AutoOffsetReset = AutoOffsetReset.Earliest // Latest
        };

        _topic = "add-customers";

        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Func<NameEmailDTO, Task> messageValueAsyncProcessing = async (data) =>
        {
            var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new AddCustomerCommand(data));
        };

        await BaseKafkaConsumerFunctionality.StartConsuming(stoppingToken,
            _consumerConfig, _topic, messageValueAsyncProcessing);
    }
}
