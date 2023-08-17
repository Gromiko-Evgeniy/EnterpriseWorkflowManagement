using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using MediatR;

namespace HiringService.Application.CQRS.CandidateCommands;

public class UpdateCandidateCVHandler : IRequestHandler<UpdateCandidateCVCommand>
{
    private readonly ICandidateRepository _candidateRepository;

    public UpdateCandidateCVHandler(ICandidateRepository candidateRepository)
    {
        _candidateRepository = candidateRepository;
    }

    async Task<Unit> IRequestHandler<UpdateCandidateCVCommand, Unit>.Handle(UpdateCandidateCVCommand request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id);

        if (candidate is null) throw new NoCandidateWithSuchIdException();

        candidate.CV = request.CV;

        _candidateRepository.UpdateAsync(candidate);

        await _candidateRepository.SaveChangesAsync();

        return Unit.Value; //fake empty value
    }
}

