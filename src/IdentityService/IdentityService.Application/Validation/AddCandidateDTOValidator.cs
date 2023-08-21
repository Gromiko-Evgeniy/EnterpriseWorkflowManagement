using FluentValidation;
using IdentityService.Application.DTOs.CandidateDTO;

namespace ProjectManagementService.Application.Validation;

public class AddCandidateDTOValidator : AbstractValidator<AddCandidateDTO>
{
    public AddCandidateDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
        RuleFor(x => x.CV).NotEmpty().MaximumLength(500);
    }
}

