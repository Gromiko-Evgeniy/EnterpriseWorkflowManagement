using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using MediatR;

namespace HiringService.Application.CQRS.CandidateCommands;

public class UpdateCandidateNameHandler : IRequestHandler<UpdateCandidateNameCommand>
{
    private readonly ICandidateRepository _candidateRepository;

    public UpdateCandidateNameHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    async Task<Unit> IRequestHandler<UpdateCandidateNameCommand, Unit>.Handle(UpdateCandidateNameCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id);

        if (candidate is null) throw new NoCandidateWithSuchIdException();

        candidate.Name = request.Name;

        await _candidateRepository.UpdateAsync(candidate);

        return Unit.Value; //fake empty value
    }
}