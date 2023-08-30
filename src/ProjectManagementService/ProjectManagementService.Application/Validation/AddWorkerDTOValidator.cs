using FluentValidation;
using ProjectManagementService.Application.DTOs.WorkerDTOs;

namespace ProjectManagementService.Application.Validation;

public class AddWorkerDTOValidator : AbstractValidator<AddWorkerDTO>
{
    public AddWorkerDTOValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(60);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
