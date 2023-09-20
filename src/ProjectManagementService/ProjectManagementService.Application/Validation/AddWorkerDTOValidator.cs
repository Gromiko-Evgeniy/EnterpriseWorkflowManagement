using FluentValidation;
using ProjectManagementService.Application.DTOs;

namespace ProjectManagementService.Application.Validation;

public class NameEmailDTOValidator : AbstractValidator<NameEmailDTO>
{
    public NameEmailDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(60);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
