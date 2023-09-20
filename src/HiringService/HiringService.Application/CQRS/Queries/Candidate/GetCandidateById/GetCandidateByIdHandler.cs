using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;
using MediatR;

namespace HiringService.Application.CQRS.CandidateQueries;

public class GetCandidateByIdHandler : IRequestHandler<GetCandidateByIdQuery, CandidateMainInfoDTO>
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public GetCandidateByIdHandler(ICandidateRepository candidateRepository, IMapper mapper)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
    }

    public async Task<CandidateMainInfoDTO> Handle(GetCandidateByIdQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(request.Id);
        
        if (candidate is null) throw new NoCandidateWithSuchIdException();

        var candidateDTO = _mapper.Map<CandidateMainInfoDTO>(candidate);

        return candidateDTO;
    }
}
