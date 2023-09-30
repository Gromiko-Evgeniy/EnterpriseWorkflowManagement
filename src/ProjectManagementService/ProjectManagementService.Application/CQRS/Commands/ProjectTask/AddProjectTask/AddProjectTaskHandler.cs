using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class AddProjectTaskHandler : IRequestHandler<AddProjectTaskCommand, string>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public AddProjectTaskHandler(IProjectTaskRepository projectTaskRepository,
        IProjectRepository projectRepository, IDistributedCache cache, IMapper mapper)
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<string> Handle(AddProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTaskDTO = request.ProjectTaskDTO;

        var project = _projectRepository.GetByIdAsync(projectTaskDTO.ProjectId);
        if (project is null) throw new NoProjectWithSuchIdException();

        var newProjectTask = _mapper.Map<ProjectTask>(projectTaskDTO);
        var id = await _projectTaskRepository.AddOneAsync(newProjectTask);

        var idKey = RedisKeysPrefixes.ProjectTaskPrefix + id;
        await _cache.SetRecordAsync(idKey, project);

        return id;
    }
}
