using FluentValidation;
using HiringService.Application.CQRS.CandidateCommands;

namespace HiringService.Application.Validation.CQRS;

public class AddCandidateCommandValidator : AbstractValidator<AddCandidateCommand>
{
    public AddCandidateCommandValidator()
    {
        RuleFor(x => x.CandidateDTO.Name).NotEmpty().MaximumLength(60)
               .WithMessage("The name must not be empty and cannot exceed 60 characters.");

        RuleFor(x => x.CandidateDTO.Email).NotEmpty().EmailAddress()
            .WithMessage("Email must be non-empty and match the email template.");

        RuleFor(x => x.CandidateDTO.CV).NotEmpty().MaximumLength(500)
            .WithMessage("CV must not be empty and cannot exceed 500 characters.");
    }
}