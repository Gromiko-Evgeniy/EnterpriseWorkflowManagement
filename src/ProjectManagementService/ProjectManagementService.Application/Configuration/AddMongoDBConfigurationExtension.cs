using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagementService.Application.Configuration;

public static class AddMongoDBConfigurationExtension
{
    public static IServiceCollection AddMongoDBConfiguration(
             this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDBConfiguration>(configuration.GetSection("DBConnectionSettings"));

        return services;
    }
}
