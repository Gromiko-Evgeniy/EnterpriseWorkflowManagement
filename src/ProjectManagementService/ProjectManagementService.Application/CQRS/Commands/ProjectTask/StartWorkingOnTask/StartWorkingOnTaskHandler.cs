using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.Exceptions.Worker;
using System.Threading.Tasks;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class StartWorkingOnTaskHandler : IRequestHandler<StartWorkingOnTaskCommand>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public StartWorkingOnTaskHandler(IWorkerRepository workersRepository,
        IProjectTaskRepository taskRepository, IDistributedCache cache,
        IMapper mapper)
    {
        _workerRepository = workersRepository;
        _taskRepository = taskRepository;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(StartWorkingOnTaskCommand request, CancellationToken cancellationToken)
    {
        var workerTask = await _taskRepository.GetByWorkerIdAsync(request.WorkerId);

        if (workerTask is null) throw new NoTaskWithSuchWorkerIdException();

        await _taskRepository.StartWorkingOnTask(workerTask.Id);

        var idKey = RedisKeysPrefixes.ProjectTaskPrefix + workerTask.Id;
        var taskDTO = _mapper.Map<TaskMainInfoDTO>(workerTask);
        await _cache.SetRecordAsync(idKey, taskDTO);

        return Unit.Value; //fake empty value
    }
}
