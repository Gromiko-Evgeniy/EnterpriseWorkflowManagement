using FluentValidation;
using MongoDB.Bson;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;

namespace ProjectManagementService.Application.Validation.CQRS;

public class AddProjectTaskCommandValidator : AbstractValidator<AddProjectTaskCommand>
{
    public AddProjectTaskCommandValidator()
    {
        RuleFor(x => x.ProjectTaskDTO.Name).NotEmpty().MaximumLength(100)
                    .WithMessage("The name must not be empty and cannot exceed 100 characters.");

        RuleFor(x => x.ProjectTaskDTO.Description).NotEmpty().MaximumLength(500)
                    .WithMessage("The description must not be empty and cannot exceed 500 characters.");

        RuleFor(x => x.ProjectTaskDTO.Priority).IsInEnum()
                    .WithMessage("The priority must be enum value.");

        RuleFor(x => x.ProjectTaskDTO.ProjectId).NotEmpty().Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("The ProjectId must be noтempty mongodb ObjectId.");

    }
}


