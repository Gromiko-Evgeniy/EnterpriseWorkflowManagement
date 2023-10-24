using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectCommands;

public class AddProjectHandler : IRequestHandler<AddProjectCommand, string>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public AddProjectHandler(
        IProjectRepository repository,
        IDistributedCache cache,
        IMapper mapper)
    {
        _projectRepository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<string> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var projectDTO = request.ProjectDTO;

        var newProject = _mapper.Map<Project>(projectDTO);

        //CustomerId is valid due to GetCustomerByEmail checks.
        newProject.CustomerId = request.CustomerId;

        var id = await _projectRepository.AddOneAsync(newProject);

        var idKey = RedisKeysPrefixes.ProjectPrefix + newProject.Id;
        await _cache.SetRecordAsync(idKey, newProject);

        return id;
    }
}
