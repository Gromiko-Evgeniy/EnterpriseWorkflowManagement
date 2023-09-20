using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;


public class GetHiringStageNameByIdHandler : IRequestHandler<GetHiringStageNameByIdQuery, GetStageNameDTO>
{
    private readonly IHiringStageNameRepository _nameRepository;
    private readonly IMapper _mapper;

    public GetHiringStageNameByIdHandler(IHiringStageNameRepository nameRepository, IMapper mapper)
    {
        _nameRepository = nameRepository;
        _mapper = mapper;
    }

    public async Task<GetStageNameDTO> Handle(GetHiringStageNameByIdQuery request, CancellationToken cancellationToken)
    {

        var stageName = await _nameRepository.GetByIdAsync(request.Id);

        if (stageName == null) throw new NoStageNameWithSuchIdException();

        var stageNameDTO = _mapper.Map<GetStageNameDTO>(stageName);

        return stageNameDTO;
    }
}
