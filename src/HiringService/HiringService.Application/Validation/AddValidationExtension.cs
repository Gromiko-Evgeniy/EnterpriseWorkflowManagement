using FluentValidation;
using FluentValidation.AspNetCore;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.DTOs.HiringStageDTOs;
using Microsoft.Extensions.DependencyInjection;

namespace HiringService.Application.Validation;

public static class AddValidationExtension
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {

        services.AddTransient<IValidator<AddCandidateDTO>, AddCandidateDTOValidator>();
        services.AddTransient<IValidator<AddHiringStageDTO>, AddHiringStageDTOValidator>();
        services.AddFluentValidationAutoValidation();
        
        return services;
    }
}
