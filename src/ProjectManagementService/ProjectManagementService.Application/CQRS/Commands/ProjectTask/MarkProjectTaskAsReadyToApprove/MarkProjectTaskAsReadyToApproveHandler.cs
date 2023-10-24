using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class MarkProjectTaskAsReadyToApproveHandler : IRequestHandler<MarkProjectTaskAsReadyToApproveCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public MarkProjectTaskAsReadyToApproveHandler(
        IWorkerRepository workersRepository,
        IProjectTaskRepository taskRepository,
        IMapper mapper,
        IDistributedCache cache)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
        _mapper = mapper;
        _cache = cache; 
    }

    public async Task<Unit> Handle(MarkProjectTaskAsReadyToApproveCommand request, CancellationToken cancellationToken)
    {
        var workerTask = await _taskRepository.GetByWorkerIdAsync(request.WorkerId);
        if (workerTask == null) throw new NoTaskWithSuchWorkerIdException();

        var worker = await _workerRepository.GetByIdAsync(request.WorkerId);
        if (worker is null) throw new NoWorkerWithSuchIdException();

        await _taskRepository.MarkAsReadyToApproveAsync(workerTask.Id);

        var idKey = RedisKeysPrefixes.ProjectTaskPrefix + workerTask.Id;
        var taskDTO = _mapper.Map<TaskMainInfoDTO>(workerTask);
        await _cache.SetRecordAsync(idKey, taskDTO);

        return Unit.Value; //fake empty value
    }
}
