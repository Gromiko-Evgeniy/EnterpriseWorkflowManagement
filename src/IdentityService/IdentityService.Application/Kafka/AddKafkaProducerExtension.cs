using IdentityService.Application.KafkaAbstractions;
using IdentityService.Application.Services.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Application.Kafka;

public static class AddKafkaProducerExtension
{
    public static IServiceCollection AddKafkaProducer(this IServiceCollection services)
    {
        services.AddScoped<IKafkaProducer, KafkaProducer>();

        return services;
    }
}
