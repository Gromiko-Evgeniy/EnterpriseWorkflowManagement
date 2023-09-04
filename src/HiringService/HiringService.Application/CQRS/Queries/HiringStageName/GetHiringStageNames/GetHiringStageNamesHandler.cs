using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.DTOs.StageNameDTOs;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public class GetHiringStageNamesHandler : IRequestHandler<GetHiringStageNamesQuery, List<GetStageNameDTO>>
{
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly IMapper _mapper;

    public GetHiringStageNamesHandler(IHiringStageNameRepository nameRepository, IMapper mapper)
    {
        _nameRepository = nameRepository;
        _mapper = mapper;
    }

    public async Task<List<GetStageNameDTO>> Handle(GetHiringStageNamesQuery request, CancellationToken cancellationToken)
    {
        var stageNames = await _nameRepository.GetAllAsync();

        var stageNameDTOs = stageNames.Select(_mapper.Map<GetStageNameDTO>).ToList();

        return stageNameDTOs;
    }
}
