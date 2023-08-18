using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Application.ProjectTaskDTOs;
using ProjectManagementService.Application.WorkerDTOs;

namespace ProjectManagementService.Application.Validation;

public static class AddValidationExtension
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {

        services.AddTransient<IValidator<AddProjectDTO>, AddProjectDTOValidator>();
        services.AddTransient<IValidator<AddProjectTaskDTO>, AddProjectTaskDTOValidator>();
        services.AddTransient<IValidator<AddWorkerDTO>, AddWorkerDTOValidator>();
        services.AddFluentValidationAutoValidation();
        
        return services;
    }
}
