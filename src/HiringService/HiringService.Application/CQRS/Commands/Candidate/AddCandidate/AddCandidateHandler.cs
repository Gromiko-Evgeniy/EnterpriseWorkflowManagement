using AutoMapper;
using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.CandidateCommands;

public class AddCandidateHandler : IRequestHandler<AddCandidateCommand, int>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public AddCandidateHandler(ICandidateRepository candidateRepository, IMapper mapper)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(AddCandidateCommand request, CancellationToken cancellationToken)
    {
        var candidateDTO = request.CandidateDTO;

        var oldCandidate = await _candidateRepository.GetByEmailAsync(request.CandidateDTO.Email);

        if (oldCandidate is not null) throw new CandidateAlreadyExistsException();

        var candidate = _mapper.Map<Candidate>(candidateDTO);

        candidate = _candidateRepository.AddAsync(candidate);

        await _candidateRepository.SaveChangesAsync();

        return candidate.Id;
    }
}
