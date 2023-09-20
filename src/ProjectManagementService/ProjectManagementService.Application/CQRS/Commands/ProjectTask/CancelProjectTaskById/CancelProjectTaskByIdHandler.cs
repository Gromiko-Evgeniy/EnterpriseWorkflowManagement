using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.DTOs.ProjectTaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class CancelProjectTaskByIdHandler : IRequestHandler<CancelProjectTaskByIdCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public CancelProjectTaskByIdHandler(IProjectTaskRepository taskRepository,
        IProjectRepository projectRepository, IDistributedCache cache, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Unit> Handle(CancelProjectTaskByIdCommand request, CancellationToken cancellationToken)
    {
        var idKey = RedisKeysPrefixes.ProjectTaskPrefix + request.ProjectTaskId;

        var taskDTO = await _cache.GetRecordAsync<TaskMainInfoDTO>(idKey);
        var task = _mapper.Map<ProjectTask>(taskDTO);

        if (task is null) 
        {
            task = await _taskRepository.GetByIdAsync(request.ProjectTaskId);
        }

        if (task is null) throw new NoProjectTaskWithSuchIdException();

        var customerProjects = await _projectRepository.GetAllCustomerProjectsAsync(request.CustomerId);
        if (!customerProjects.Any(p => p.Id == task.ProjectId)) throw new AccessToCancelProjectTaskDeniedException();

        await _taskRepository.CancelAsync(request.ProjectTaskId);

        task.Status = ProjectTaskStatus.Canceled;
        await _cache.SetRecordAsync(idKey, task);

        return Unit.Value; //fake empty value
    }
}
