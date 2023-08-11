using HiringService.Application.Abstractions;
using HiringService.Application.CQRS.CandidateCommands;
using MediatR;

namespace ProjectManagementService.Application.CQRS.ProjectCommands;
public class UpdateCandidateNameHandler : IRequestHandler<UpdateCandidateNameCommand>
{
    private readonly ICandidateRepository candidates;

    public UpdateCandidateNameHandler(ICandidateRepository candidateRepository)
    {
        candidates = candidateRepository;
    }

    async Task<Unit> IRequestHandler<UpdateCandidateNameCommand, Unit>.Handle(UpdateCandidateNameCommand request, CancellationToken cancellationToken)
    {
        await candidates.UpdateNameAsync(request.Id, request.Name);

        return Unit.Value; //fake empty value
    }
}