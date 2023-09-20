using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.DTOs.ProjectTaskDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class MarkProjectTaskAsApprovedHandler : IRequestHandler<MarkProjectTaskAsApprovedCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public MarkProjectTaskAsApprovedHandler(IProjectTaskRepository tasksRepository,
        IProjectRepository projectRepository, IDistributedCache cache,
        IMapper mapper)
    {
        _taskRepository = tasksRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Unit> Handle(MarkProjectTaskAsApprovedCommand request, CancellationToken cancellationToken)
    {
        var idKey = RedisKeysPrefixes.ProjectTaskPrefix + request.ProjectTaskId;

        var taskDTO = await _cache.GetRecordAsync<TaskMainInfoDTO>(idKey);
        var task = _mapper.Map<ProjectTask>(taskDTO);

        if (task is null)
        {
            task = await _taskRepository.GetByIdAsync(request.ProjectTaskId);
        }

        if (task is null) throw new NoProjectTaskWithSuchIdException();

        var projectLeaderProject = await _projectRepository.GetProjectByProjectLeaderId(request.ProjectLeaderId);

        if (projectLeaderProject is null) throw new NoProjectWithSuchIdException();
        if (task.ProjectId != projectLeaderProject.Id) throw new AccessToApproveProjectTaskDeniedException();
        await _taskRepository.MarkAsApproved(request.ProjectTaskId);

        task.Status = ProjectTaskStatus.Approved;
        await _cache.SetRecordAsync(idKey, task);

        return Unit.Value; //fake empty value
    }
}
