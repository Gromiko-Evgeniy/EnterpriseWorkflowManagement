﻿using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.DTOs.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectMainInfoDTO>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetProjectByIdHandler(IProjectRepository repository,
        IDistributedCache cache, IMapper mapper)
    {
        _projectRepository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ProjectMainInfoDTO> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var idKey = "Project_" + request.Id;
        var project = await _cache.GetRecordAsync<Project>(idKey);

        if (project is null)
        {
            project = await _projectRepository.GetByIdAsync(request.Id);
        }

        if (project is null) throw new NoProjectWithSuchIdException();

        await _cache.SetRecordAsync(idKey, project);

        return _mapper.Map<ProjectMainInfoDTO>(project);
    }
}
