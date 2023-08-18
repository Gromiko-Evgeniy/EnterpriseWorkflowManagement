using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTaskByWorkerIdHandler : IRequestHandler<GetProjectTaskByWorkerIdQuery, ProjectTask>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;


    public GetProjectTaskByWorkerIdHandler(IWorkerRepository repository, IProjectTaskRepository projectTaskRepository)
    {
        _workerRepository = repository;
        _projectTaskRepository = projectTaskRepository;
    }


    public async Task<ProjectTask> Handle(GetProjectTaskByWorkerIdQuery request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

        if (worker is null) throw new NoWorkerWithSuchIdException();
        if (worker.CurrentTaskId is null) throw new WorkerHasNoTaskNowException();

        var task = await _projectTaskRepository.GetByIdAsync(worker.CurrentTaskId);

        if (task is null)
        {
            worker.CurrentTaskId = null;
            throw new WorkerHasNoTaskNowException();
        }

        return task;
    }
}