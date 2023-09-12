using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.DTOs.ProjectTaskDTOs;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTaskByWorkerIdHandler : IRequestHandler<GetProjectTaskByWorkerIdQuery, TaskMainInfoDTO>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetProjectTaskByWorkerIdHandler(IWorkerRepository repository,
        IProjectTaskRepository projectTaskRepository, IMapper mapper,
        IWorkerRepository workerRepository)
    {
        _workerRepository = repository;
        _projectTaskRepository = projectTaskRepository;
        _mapper = mapper;
        _workerRepository = workerRepository;
    }

    public async Task<TaskMainInfoDTO> Handle(GetProjectTaskByWorkerIdQuery request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetByIdAsync(request.WorkerId);

        if (worker is null) throw new NoWorkerWithSuchIdException();
        if (worker.CurrentTaskId is null) throw new WorkerHasNoTaskNowException();

        var idKey = "Task_" + worker.CurrentTaskId;
        var taskDTO = await _cache.GetRecordAsync<TaskMainInfoDTO>(idKey);

        if (taskDTO is not null) return taskDTO;

        var task = await _projectTaskRepository.GetByIdAsync(worker.CurrentTaskId);
        if (task is null)
        {
            worker.CurrentTaskId = null;
            throw new WorkerHasNoTaskNowException();
        }

        return _mapper.Map<TaskMainInfoDTO>(task);
    }
}
