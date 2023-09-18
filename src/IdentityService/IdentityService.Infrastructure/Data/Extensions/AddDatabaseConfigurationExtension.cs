using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Data.Extensions;

public static class AddDatabaseConfigurationExtension
{
    public static IServiceCollection AddDatabaseConfiguration(
             this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(
            o => o.UseSqlServer(
                configuration.GetConnectionString("IdentityServiceDB"),
                b => b.MigrationsAssembly(configuration.GetSection("MigrationsAssembly").Get<string>())
            )
        );

        services.AddDbContext<HangfireDataContext>(
            o => o.UseSqlServer(
                configuration.GetConnectionString("Hangfire")
            )
        );

        return services;
    }
}
