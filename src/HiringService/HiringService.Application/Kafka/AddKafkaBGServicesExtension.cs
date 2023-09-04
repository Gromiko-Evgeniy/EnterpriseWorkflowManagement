using HiringService.Application.Kafka.BGServices;
using Microsoft.Extensions.DependencyInjection;

namespace HiringService.Application.Kafka;

public static class AddKafkaBGServicesExtension
{
    public static IServiceCollection AddKafkaBGServices(this IServiceCollection services)
    {
        services.AddHostedService<KafkaAddWorkerMessagesConsumer>();
        services.AddHostedService<KafkaRemoveWorkerMessagesConsumer>();

        services.AddHostedService<KafkaAddCandidateMessagesConsumer>();

        return services;
    }
}
