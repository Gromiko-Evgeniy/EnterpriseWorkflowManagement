using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ProjectManagementService.Infrastucture.Data.Extensions;

public static class AddDatabaseConfigurationExtension
{
    public static IServiceCollection AddDatabaseConfiguration(
             this IServiceCollection services, IConfiguration configuration)
    {
        var connetionString = configuration.GetSection("DBConnectionSettings:ConnetionString").Get<string>();

        services.AddSingleton<IMongoClient>(service => new MongoClient(connetionString));

        return services;
    }
}
