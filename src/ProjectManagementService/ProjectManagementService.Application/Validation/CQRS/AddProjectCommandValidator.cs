using FluentValidation;
using MongoDB.Bson;
using ProjectManagementService.Application.CQRS.ProjectCommands;

namespace ProjectManagementService.Application.Validation.CQRS;

public class AddProjectCommandValidator : AbstractValidator<AddProjectCommand>
{
    public AddProjectCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("The name must be noтempty mongodb ObjectId.");

        RuleFor(x => x.ProjectDTO.Objective).NotEmpty().MaximumLength(100)
            .WithMessage("The ojective must not be empty and cannot exceed 100 characters.");

        RuleFor(x => x.ProjectDTO.Description).NotEmpty().MaximumLength(500)
            .WithMessage("The description must not be empty and cannot exceed 500 characters.");
    }
}
