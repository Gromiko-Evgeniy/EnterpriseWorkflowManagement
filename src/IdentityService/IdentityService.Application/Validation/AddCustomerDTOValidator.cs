using FluentValidation;
using IdentityService.Application.DTOs.CustomerDTOs;

namespace IdentityService.Application.Validation;

public class AddCustomerDTOValidator : AbstractValidator<AddCustomerDTO>
{
    public AddCustomerDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
    }
}
