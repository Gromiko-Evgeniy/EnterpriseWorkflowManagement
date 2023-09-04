﻿using Confluent.Kafka;
using HiringService.Application.CQRS.WorkerCommands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectManagementService.Application.DTOs;

namespace HiringService.Application.Kafka.BGServices;

public class KafkaAddWorkerMessagesConsumer : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly string _topic;

    public KafkaAddWorkerMessagesConsumer(IConfiguration configuration,
         IServiceScopeFactory serviceScopeFactory)
    {
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration.GetSection("KafkaBootstrapServer").Get<string>(),
            GroupId = "HiringService",
            AutoOffsetReset = AutoOffsetReset.Earliest // Latest
        };

        _topic = "add-workers";
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Func<NameEmailDTO, Task> messageValueAsyncProcessing = async (data) =>
        {
            var scope = _serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(new AddWorkerCommand(data));
        };

        await BaseKafkaConsumerFunctionality.StartConsuming(stoppingToken,
            _consumerConfig, _topic, messageValueAsyncProcessing);
    }
}
