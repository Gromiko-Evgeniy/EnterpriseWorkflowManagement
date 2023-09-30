using FluentValidation;
using IdentityService.Application.DTOs.WorkerDTOs;

namespace IdentityService.Application.Validation;

public class AddWorkerDTOValidator : AbstractValidator<AddWorkerDTO>
{
    public AddWorkerDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
    }
}
