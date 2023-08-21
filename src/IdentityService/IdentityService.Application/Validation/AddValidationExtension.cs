using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityService.Application.DTOs.CandidateDTO;
using IdentityService.Application.DTOs.CustomerDTOs;
using IdentityService.Application.DTOs.WorkerDTOs;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectManagementService.Application.Validation;

public static class AddValidationExtension
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {

        services.AddTransient<IValidator<AddCandidateDTO>, AddCandidateDTOValidator>();
        services.AddTransient<IValidator<AddCustomerDTO>, AddCustomerDTOValidator>();
        services.AddTransient<IValidator<AddWorkerDTO>, AddWorkerDTOValidator>();
        services.AddFluentValidationAutoValidation();
        
        return services;
    }
}
