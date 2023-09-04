﻿using HiringService.Application.Abstractions.RepositoryAbstractions;
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

        _candidateRepository.Update(candidate);
        await _candidateRepository.SaveChangesAsync();

        return Unit.Value; //fake empty value
    }
}
