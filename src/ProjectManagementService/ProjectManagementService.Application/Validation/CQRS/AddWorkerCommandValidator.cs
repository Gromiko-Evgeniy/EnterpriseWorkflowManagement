using FluentValidation;
using ProjectManagementService.Application.CQRS.WorkerCommands;

namespace ProjectManagementService.Application.Validation.CQRS;

public class AddWorkerCommandValidator : AbstractValidator<AddWorkerCommand>
{
    public AddWorkerCommandValidator()
    {
        RuleFor(x => x.NameEmailDTO.Name).NotEmpty().MaximumLength(60)
            .WithMessage("The name must not be empty and cannot exceed 100 characters.");

        RuleFor(x => x.NameEmailDTO.Email).NotEmpty().EmailAddress()
            .WithMessage("Email must be non-empty and match the email template.");
    }
}
