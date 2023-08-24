using Microsoft.Extensions.DependencyInjection;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Infrastucture.Data.Repositories;

namespace ProjectManagementService.Infrastucture.Data.Extensions;

public static class AddRepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
        services.AddScoped<IWorkerRepository, WorkerRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
