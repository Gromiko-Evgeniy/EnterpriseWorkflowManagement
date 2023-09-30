using FluentValidation;
using HiringService.Application.CQRS.HiringStageCommands;

namespace HiringService.Application.Validation.CQRS;

public class AddHiringStageValidator : AbstractValidator<AddHiringStageCommand>
{
    public AddHiringStageValidator()
    {
        RuleFor(x => x.StageDTO.HiringStageNameId).GreaterThanOrEqualTo(0)
            .WithMessage("HiringStageNameId must be greater than 0");

        RuleFor(x => x.StageDTO.CandidateId).GreaterThanOrEqualTo(0)
            .WithMessage("CandidateId must be greater than 0");

        RuleFor(x => x.StageDTO.IntervierId).GreaterThanOrEqualTo(0)
            .WithMessage("IntervierId must be greater than 0");

        RuleFor(x => x.StageDTO.Description).NotEmpty().MaximumLength(200)
            .WithMessage("Description must not be empty and cannot exceed 200 characters.");
    }
}