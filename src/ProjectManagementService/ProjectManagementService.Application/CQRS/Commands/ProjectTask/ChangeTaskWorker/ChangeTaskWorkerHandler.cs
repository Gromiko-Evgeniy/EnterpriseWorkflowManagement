using MediatR;
using MongoDB.Bson;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.ProjectTask;

namespace ProjectManagementService.Application.CQRS.Commands.ProjectTask.ChangeTaskWorker;

public class ChangeTaskWorkerHandler : IRequestHandler<ChangeTaskWorkerCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;

    public ChangeTaskWorkerHandler(IWorkerRepository workersRepository, IProjectTaskRepository taskRepository)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
    }

    public async Task<Unit> Handle(ChangeTaskWorkerCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);

        if (task is null) throw new NoProjectTaskWithSuchIdException();
        
        var newTaskDocument = new BsonDocument()
        {
            { "currentTaskId", task.Id },
            { "currentProjectId", task.ProjectId }
        };

        var update = new BsonDocument("$set", newTaskDocument);
        await _workerRepository.UpdateAsync(update, worker => worker.Id == request.WorkerId);

        return Unit.Value;
    }
}
