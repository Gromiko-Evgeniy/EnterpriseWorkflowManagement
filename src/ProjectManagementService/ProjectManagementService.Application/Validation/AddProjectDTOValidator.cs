using FluentValidation;
using ProjectManagementService.Application.ProjectDTOs;

namespace ProjectManagementService.Application.Validation;

public class AddProjectDTOValidator : AbstractValidator<AddProjectDTO>
{
    public AddProjectDTOValidator()
    {
        RuleFor(x => x.Objective).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}
