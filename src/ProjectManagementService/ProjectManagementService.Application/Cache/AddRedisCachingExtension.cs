using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagementService.Application.Cache;

public static class AddRedisCachingExtension
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "ProjectManagementService";
            }
        );

        return services;
    }
}
