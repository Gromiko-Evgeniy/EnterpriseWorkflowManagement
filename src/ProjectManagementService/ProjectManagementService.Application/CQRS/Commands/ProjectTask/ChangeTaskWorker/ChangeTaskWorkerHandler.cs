using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class ChangeTaskWorkerHandler : IRequestHandler<ChangeTaskWorkerCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public ChangeTaskWorkerHandler(IWorkerRepository workersRepository,
        IProjectTaskRepository taskRepository, IDistributedCache cache,
        IMapper mapper)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Unit> Handle(ChangeTaskWorkerCommand request, CancellationToken cancellationToken)
    {
        var idKey = "Task_" + request.TaskId;

        var taskDTO = await _cache.GetRecordAsync<TaskMainInfoDTO>(idKey);
        var task = _mapper.Map<ProjectTask>(taskDTO);

        if (task is null)
        {
            task = await _taskRepository.GetByIdAsync(request.TaskId);
        }

        if (task is null) throw new NoProjectTaskWithSuchIdException();

        await _cache.SetRecordAsync(idKey, task);

        var newTaskDocument = new BsonDocument()
        {
            { "currentTaskId", task.Id },
            { "currentProjectId", task.ProjectId }
        };

        var update = new BsonDocument("$set", newTaskDocument);
        await _workerRepository.UpdateAsync(update, worker => worker.Id == request.WorkerId);

        await _cache.RemoveAsync("Worker_" + request.WorkerId);

        return Unit.Value;
    }
}
