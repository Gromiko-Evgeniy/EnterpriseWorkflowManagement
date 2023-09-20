using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace ProjectManagementService.Infrastucture.Data.Extensions;

public static class AddDatabaseConnectionExtension
{
    public static IServiceCollection AddDatabaseConnection(
             this IServiceCollection services, IConfiguration configuration)
    {
        var connetionString = configuration.GetSection("DBConnectionSettings:ConnetionString").Get<string>();

        services.AddSingleton<IMongoClient>(service => new MongoClient(connetionString));

        return services;
    }
}
