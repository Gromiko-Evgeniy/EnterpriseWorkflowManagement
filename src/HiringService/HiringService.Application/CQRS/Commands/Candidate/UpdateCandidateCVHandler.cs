using HiringService.Application.Abstractions;
using HiringService.Application.CQRS.CandidateCommands;
using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectCommands;
public class UpdateCandidateCVHandler : IRequestHandler<UpdateCandidateCVCommand>
{
    private readonly ICandidateRepository candidates;

    public UpdateCandidateCVHandler(ICandidateRepository candidateRepository)
    {
        candidates = candidateRepository;
    }

    async Task<Unit> IRequestHandler<UpdateCandidateCVCommand, Unit>.Handle(UpdateCandidateCVCommand request, CancellationToken cancellationToken)
    {
        await candidates.UpdateCVAsync(request.Id, request.CV);

        return Unit.Value; //fake empty value
    }
}

