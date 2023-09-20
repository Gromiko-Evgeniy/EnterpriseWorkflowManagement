using Microsoft.Extensions.DependencyInjection;
using ProjectManagementService.Application.Kafka.BGServices;

namespace ProjectManagementService.Application.Kafka;

public static class AddKafkaBGServicesExtension
{
    public static IServiceCollection AddKafkaBGServices(this IServiceCollection services)
    {
        services.AddHostedService<KafkaAddWorkerMessagesConsumer>();
        services.AddHostedService<KafkaRemoveWorkerMessagesConsumer>();
        
        services.AddHostedService<KafkaAddCustomerMessagesConsumer>();

        return services;
    }
}
 