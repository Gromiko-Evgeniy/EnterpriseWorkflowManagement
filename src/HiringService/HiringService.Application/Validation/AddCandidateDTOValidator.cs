using FluentValidation;
using HiringService.Application.DTOs.CandidateDTOs;

namespace HiringService.Application.Validation
{
    public class AddCandidateDTOValidator : AbstractValidator<AddCandidateDTO>
    {
        public AddCandidateDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(60)
                .WithMessage("The name must not be empty and cannot exceed 60 characters.");

            RuleFor(x => x.Email).NotEmpty().EmailAddress()
                .WithMessage("Email must be non-empty and match the email template.");

            RuleFor(x => x.CV).NotEmpty().MaximumLength(500)
                .WithMessage("CV must not be empty and cannot exceed 500 characters.");
        }
    }
}
