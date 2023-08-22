using AutoMapper;
using HiringService.Application.Abstractions;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Application.Exceptions.HiringStageName;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public class GetHiringStageNameByNameHandler : IRequestHandler<GetHiringStageNameByNameQuery, GetStageNameDTO>
{
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly IMapper _mapper;

    public GetHiringStageNameByNameHandler(IHiringStageNameRepository nameRepository, IMapper mapper)
    {
        _nameRepository = nameRepository;
        _mapper = mapper;
    }

    public async Task<GetStageNameDTO> Handle(GetHiringStageNameByNameQuery request, CancellationToken cancellationToken)
    {
        var stageName = await _nameRepository.GetByNameAsync(request.Name);

        if (stageName == null) throw new NoStageNameWithSuchNameException(); 
        
        var stageNameDTO = _mapper.Map<GetStageNameDTO>(stageName);

        return stageNameDTO;
    }
}

