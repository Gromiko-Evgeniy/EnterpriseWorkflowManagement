using Confluent.Kafka;
using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Application.DTOs.CandidateDTOs;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HiringService.Application.Kafka.BGServices;

public class KafkaAddCandidateMessagesConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _topic;

    public KafkaAddCandidateMessagesConsumer(IConfiguration configuration,
        IServiceScopeFactory serviceScopeFactory)
    {
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration.GetSection("KafkaBootstrapServer").Get<string>(),
            GroupId = "HiringService",
            AutoOffsetReset = AutoOffsetReset.Earliest // Latest
        };

        _topic = "add-candidates";

        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Func<AddCandidateDTO, Task> messageValueAsyncProcessing = async (data) =>
        {
            var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new AddCandidateCommand(data));
        };

        await BaseKafkaConsumerFunctionality.StartConsuming(stoppingToken,
            _consumerConfig, _topic, messageValueAsyncProcessing);
    }
}
