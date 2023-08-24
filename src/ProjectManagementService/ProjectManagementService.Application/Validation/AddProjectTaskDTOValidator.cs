using FluentValidation;
using ProjectManagementService.Application.ProjectTaskDTOs;

namespace ProjectManagementService.Application.Validation;

public class AddProjectTaskDTOValidator : AbstractValidator<AddProjectTaskDTO>
{
    public AddProjectTaskDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Priority).IsInEnum();
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
