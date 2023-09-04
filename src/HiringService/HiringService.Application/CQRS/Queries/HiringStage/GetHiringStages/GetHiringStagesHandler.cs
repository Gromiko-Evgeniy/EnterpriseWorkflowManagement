using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.DTOs.HiringStageDTOs;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public class GetHiringStagesHandler : IRequestHandler<GetHiringStagesQuery, List<HiringStageShortInfoDTO>>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IMapper _mapper;

    public GetHiringStagesHandler(IHiringStageRepository stageRepository, IMapper mapper)
    {
        _stageRepository = stageRepository;
        _mapper = mapper;
    }

    public async Task<List<HiringStageShortInfoDTO>> Handle(GetHiringStagesQuery request, CancellationToken cancellationToken)
    {
        var stages =  await _stageRepository.GetAllAsync();

        var stageDTOs = stages.Select(_mapper.Map<HiringStageShortInfoDTO>).ToList();

        return stageDTOs;
    }
}
