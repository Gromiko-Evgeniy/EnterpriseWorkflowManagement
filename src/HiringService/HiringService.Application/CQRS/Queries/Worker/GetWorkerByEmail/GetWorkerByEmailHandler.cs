using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.CQRS.CandidateQueries;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.Queries.Worker.GetWorkerByEmail;

public class GetWorkerByEmailHandler : IRequestHandler<GetWorkerByEmailQuery, CandidateMainInfoDTO>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IMapper _mapper;

    public GetWorkerByEmailHandler(IWorkerRepository workerRepository, IMapper mapper)
    {
        _workerRepository = workerRepository;
        _mapper = mapper;
    }

    public async Task<CandidateMainInfoDTO> Handle(GetWorkerByEmailQuery request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByEmailAsync(request.Email);

        if (worker is null) throw new NoCandidateWithSuchEmailException();

        var candidateDTO = _mapper.Map<CandidateMainInfoDTO>(worker);

        return candidateDTO;
    }
}
