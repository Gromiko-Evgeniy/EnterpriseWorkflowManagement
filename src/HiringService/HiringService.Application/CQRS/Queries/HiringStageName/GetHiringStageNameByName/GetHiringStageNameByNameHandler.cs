using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Application.Exceptions.HiringStageName;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.StageNameQueries;

public class GetHiringStageNameByNameHandler : IRequestHandler<GetHiringStageNameByNameQuery, GetStageNameDTO>
{
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetHiringStageNameByNameHandler(IHiringStageNameRepository nameRepository,
        IMapper mapper, IDistributedCache cache)
    {
        _nameRepository = nameRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<GetStageNameDTO> Handle(GetHiringStageNameByNameQuery request, CancellationToken cancellationToken)
    {
        //var nameKey = RedisKeysPrefixes.StageNamePrefix + request.Name;
        //var cachedStageName = await _cache.GetRecordAsync<GetStageNameDTO>(nameKey);

        //if (cachedStageName is not null) return cachedStageName;

        var stageName = await _nameRepository.GetByNameAsync(request.Name);

        if (stageName == null) throw new NoStageNameWithSuchNameException();

        var stageNameDTO = _mapper.Map<GetStageNameDTO>(stageName);

        //var idKey = RedisKeysPrefixes.StageNamePrefix + stageName.Id;

        //await _cache.SetRecordAsync(nameKey, stageNameDTO);
        //await _cache.SetRecordAsync(idKey, stageNameDTO);

        return stageNameDTO;
    }
}

