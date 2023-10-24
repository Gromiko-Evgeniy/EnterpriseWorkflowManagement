using AutoMapper;
using Hangfire;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Domain.Enumerations;
namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class MarkProjectTaskAsApprovedHandler : IRequestHandler<MarkProjectTaskAsApprovedCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public MarkProjectTaskAsApprovedHandler(
        IProjectTaskRepository tasksRepository,
        IProjectRepository projectRepository,
        IWorkerRepository workerRepository,
        IDistributedCache cache,
        IMapper mapper)
    {
        _taskRepository = tasksRepository;
        _projectRepository = projectRepository;
        _workerRepository = workerRepository;
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

        await SetNewTaskForWorker(task);

        return Unit.Value; //fake empty value
    }

    public async Task SetNewTaskForWorker(ProjectTask task)
    {
        var worker = await _workerRepository.GetByIdAsync(task.WorkerId);
        if (worker is null) throw new NoWorkerWithSuchIdException();

        var today = DateTime.Now;

        if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
        {
            var daysToAdd = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
            var time = new TimeSpan(6, 0, 0);

            var monday = today.AddDays(daysToAdd);
            monday = monday.Date + time;

            BackgroundJob.Schedule(
                () => _taskRepository.SetNewWorker(task.Id, worker.Id),
                monday.Subtract(DateTime.Now)); // Till monday 6 am
        }
        else
        {
            await _taskRepository.SetNewWorker(task.Id, worker.Id);
        }
    }
}
