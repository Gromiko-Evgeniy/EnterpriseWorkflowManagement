using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.Exceptions.HiringStage;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.HiringStageQueries;

public class GetHiringStageByIdHandler : IRequestHandler<GetHiringStageByIdQuery, HiringStageMainInfoDTO>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetHiringStageByIdHandler(IHiringStageRepository stageRepository,
        IMapper mapper, IDistributedCache cache)
    {
        _stageRepository = stageRepository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<HiringStageMainInfoDTO> Handle(GetHiringStageByIdQuery request, CancellationToken cancellationToken)
    {
        var idKey = "HiringStage_" + request.Id;
        var cachedHiringStage = await _cache.GetRecordAsync<HiringStageMainInfoDTO>(idKey);

        if (cachedHiringStage is not null) return cachedHiringStage;

        var hiringStage = await _stageRepository.GetByIdAsync(request.Id);
        if (hiringStage is null) throw new NoHiringStageWithSuchIdException();

        var hiringStageDTO = _mapper.Map<HiringStageMainInfoDTO>(hiringStage);

        await _cache.SetRecordAsync(idKey, hiringStageDTO);

        return hiringStageDTO;
    }
}
