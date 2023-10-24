using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.StageNameCommands;

public class AddStageNameHandler : IRequestHandler<AddStageNameCommand, int>
{
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public AddStageNameHandler(IHiringStageNameRepository nameRepository,
        IDistributedCache cache, IMapper mapper)
    {
        _nameRepository = nameRepository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<int> Handle(AddStageNameCommand request, CancellationToken cancellationToken)
    {
        var addNameDTO = request.StageNameDTO;

        var OldStageName = await _nameRepository.GetByNameAsync(addNameDTO.Name);
        if (OldStageName is not null) throw new StageNameAlreadyExistsException();

        var newStageName = _mapper.Map<HiringStageName>(addNameDTO);

        var stageNames = await _nameRepository.GetAllAsync(); // shifting the indices of all subsequent elements in the list
        if (stageNames.Any(n => n.Index == addNameDTO.Index))
        {
            foreach (var stageName in stageNames.Where(n => n.Index >= addNameDTO.Index))
            {
                stageName.Index += 1;
                _nameRepository.Update(stageName);
            }
        }
        else
        {
            await RemovePossibleIndexErrorsAsync(stageNames);

            newStageName.Index = stageNames.Count;
        }

        newStageName = _nameRepository.Add(newStageName);
        await _nameRepository.SaveChangesAsync();

        var stageNameDTO = _mapper.Map<GetStageNameDTO>(newStageName);
        var idKey = RedisKeysPrefixes.StageNamePrefix + newStageName.Id;
        var nameKey = RedisKeysPrefixes.StageNamePrefix + newStageName.Name;

        await _cache.SetRecordAsync(nameKey, stageNameDTO);
        await _cache.SetRecordAsync(idKey, stageNameDTO);

        return newStageName.Id;
    }

    private async Task RemovePossibleIndexErrorsAsync(List<HiringStageName> stageNames)
    {
        stageNames.Sort((x, y) => x.Index.CompareTo(y.Index));

        for (int i = 0; i < stageNames.Count; i++)
        {
            if (stageNames[i].Index != i)
            {
                stageNames[i].Index = i;
                _nameRepository.Update(stageNames[i]);
                await _nameRepository.SaveChangesAsync();
            }
        }
    }
}
