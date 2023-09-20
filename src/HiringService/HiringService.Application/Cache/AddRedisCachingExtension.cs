using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HiringService.Application.Cache;

public static class AddRedisCachingExtension
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "HiringService_";
            }
        );

        return services;
    }
}
