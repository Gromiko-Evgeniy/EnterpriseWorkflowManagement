using FluentValidation;
using HiringService.Application.DTOs.HiringStageDTOs;

namespace HiringService.Application.Validation;
public class AddHiringStageDTOValidator : AbstractValidator<AddHiringStageDTO>
{
    public AddHiringStageDTOValidator()
    {
        RuleFor(x => x.HiringStageNameId).GreaterThanOrEqualTo(0)
            .WithMessage("HiringStageNameId must be greater than 0");

        RuleFor(x => x.CandidateId).GreaterThanOrEqualTo(0)
            .WithMessage("CandidateId must be greater than 0");

        RuleFor(x => x.IntervierId).GreaterThanOrEqualTo(0)
            .WithMessage("IntervierId must be greater than 0");

        RuleFor(x => x.Description).NotEmpty().MaximumLength(200)
            .WithMessage("Description must not be empty and cannot exceed 200 characters.");
    }
}
