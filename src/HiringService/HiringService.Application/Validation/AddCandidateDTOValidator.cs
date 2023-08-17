using FluentValidation;
using HiringService.Application.DTOs.CandidateDTOs;

namespace HiringService.Application.Validation
{
    public class AddCandidateDTOValidator : AbstractValidator<AddCandidateDTO>
    {
        public AddCandidateDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(60);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.CV).NotEmpty().MaximumLength(500);
        }
    }
}
