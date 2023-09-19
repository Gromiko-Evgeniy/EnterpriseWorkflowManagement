using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Application.Exceptions.HiringStageName;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.StageNameQueries;


public class GetHiringStageNameByIdHandler : IRequestHandler<GetHiringStageNameByIdQuery, GetStageNameDTO>
{
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetHiringStageNameByIdHandler(IHiringStageNameRepository nameRepository,
        IMapper mapper, IDistributedCache cache)
    {
        _nameRepository = nameRepository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<GetStageNameDTO> Handle(GetHiringStageNameByIdQuery request, CancellationToken cancellationToken)
    {
        var idKey = RedisKeysPrefixes.StageNamePrefix + request.Id;
        var cachedStageName = await _cache.GetRecordAsync<GetStageNameDTO>(idKey);

        if (cachedStageName is not null) return cachedStageName;

        var stageName = await _nameRepository.GetByIdAsync(request.Id);
        if (stageName == null) throw new NoStageNameWithSuchIdException();

        var stageNameDTO = _mapper.Map<GetStageNameDTO>(stageName);

        var nameKey = RedisKeysPrefixes.StageNamePrefix + stageName.Name;

        await _cache.SetRecordAsync(nameKey, stageNameDTO);
        await _cache.SetRecordAsync(idKey, stageNameDTO);

        return stageNameDTO;
    }
}
