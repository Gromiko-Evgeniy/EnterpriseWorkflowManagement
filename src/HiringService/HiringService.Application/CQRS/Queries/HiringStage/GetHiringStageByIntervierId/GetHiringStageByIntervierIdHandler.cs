﻿using AutoMapper;
using HiringService.Application.Abstractions;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Application.Exceptions.HiringStage;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.HiringStageQueries;

public class GetHiringStageByIntervierIdHandler : IRequestHandler<GetHiringStageByIntervierIdQuery, List<HiringStageShortInfoDTO>>
{
    private readonly IHiringStageRepository _stageRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IMapper _mapper;

    public GetHiringStageByIntervierIdHandler(IHiringStageRepository stageRepository,
        IWorkerRepository workerRepository, IMapper mapper)
    {
        _stageRepository = stageRepository;
        _workerRepository = workerRepository;
        _mapper = mapper;
    }

    public async Task<List<HiringStageShortInfoDTO>> Handle(GetHiringStageByIntervierIdQuery request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.IntervierId);

        if (worker is null) throw new NoWorkerWithSuchIdException();

        var stages = await _stageRepository.GetByIntervierIdAsync(request.IntervierId);

        var stageDTOs = stages.Select(_mapper.Map<HiringStageShortInfoDTO>).ToList();

        return stageDTOs;
    }
}
