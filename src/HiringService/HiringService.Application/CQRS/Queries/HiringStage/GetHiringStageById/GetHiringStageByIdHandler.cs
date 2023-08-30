using AutoMapper;
using HiringService.Application.Abstractions;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.Exceptions.HiringStage;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public class GetHiringStageByIdHandler : IRequestHandler<GetHiringStageByIdQuery, HiringStageMainInfoDTO>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IMapper _mapper;

    public GetHiringStageByIdHandler(IHiringStageRepository stageRepository, IMapper mapper)
    {
        _stageRepository = stageRepository;
        _mapper = mapper;
    }

    public async Task<HiringStageMainInfoDTO> Handle(GetHiringStageByIdQuery request, CancellationToken cancellationToken)
    {
        var hiringStage = await _stageRepository.GetByIdAsync(request.Id);

        if (hiringStage is null) throw new NoHiringStageWithSuchIdException();

        var hiringStageDTO = _mapper.Map<HiringStageMainInfoDTO>(hiringStage);

        return hiringStageDTO;
    }
}
