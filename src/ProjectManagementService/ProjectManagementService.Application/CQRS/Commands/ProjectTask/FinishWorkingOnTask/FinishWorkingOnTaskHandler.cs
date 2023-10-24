using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class FinishWorkingOnTaskHandler : IRequestHandler<FinishWorkingOnTaskCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public FinishWorkingOnTaskHandler(
        IProjectTaskRepository taskRepository,
        IDistributedCache cache,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(FinishWorkingOnTaskCommand request, CancellationToken cancellationToken)
    {
        //WorkerId is valid due to GetWorkerByEmail checks.
        var workerTask = await _taskRepository.GetByWorkerIdAsync(request.WorkerId);

        if (workerTask is null) throw new NoTaskWithSuchWorkerIdException();

        await _taskRepository.FinishWorkingOnTask(workerTask.Id);

        var idKey = RedisKeysPrefixes.ProjectTaskPrefix + workerTask.Id;
        var taskDTO = _mapper.Map<TaskMainInfoDTO>(workerTask);
        await _cache.SetRecordAsync(idKey, taskDTO);

        return Unit.Value; //fake empty value
    }
}
