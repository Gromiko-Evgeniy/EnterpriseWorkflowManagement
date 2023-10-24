using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityService.Infrastructure.Data;

namespace IdentityService.Integration.Tests;

internal class IdentityServiceWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services => 
            {
                services.RemoveAll(typeof(DbContextOptions<DataContext>));

                var connectionString = GetConnectionString();
                services.AddDbContext<DataContext>(o => o.UseSqlServer(connectionString));

                var dbContext = GetDataContext(services);
                dbContext.Database.EnsureDeleted();
            }
        );
    }

    public static string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var configuration = builder.Build();

        return configuration.GetConnectionString("IdentityServiceTestDB")!;
    }

    private static DataContext GetDataContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        return dbContext;
    }
}