using FluentValidation;
using HiringService.Application.DTOs.HiringStageDTOs;

namespace HiringService.Application.Validation;
public class AddHiringStageDTOValidator : AbstractValidator<AddHiringStageDTO>
{
    public AddHiringStageDTOValidator()
    {
        RuleFor(x => x.HiringStageNameId).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CandidateId).GreaterThanOrEqualTo(0);
    }
}