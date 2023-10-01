using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTaskByWorkerIdHandler : IRequestHandler<GetProjectTaskByWorkerIdQuery, TaskMainInfoDTO>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetProjectTaskByWorkerIdHandler(
        IProjectTaskRepository projectTaskRepository, IMapper mapper)
    {
        _projectTaskRepository = projectTaskRepository;
        _mapper = mapper;
    }

    public async Task<TaskMainInfoDTO> Handle(GetProjectTaskByWorkerIdQuery request, CancellationToken cancellationToken)
    {
        var workerTask = await _projectTaskRepository.GetByWorkerIdAsync(request.WorkerId);
        if (workerTask is null) throw new NoTaskWithSuchWorkerIdException();

        var idKey = "Task_" + workerTask.Id;
        var taskDTO = _mapper.Map<TaskMainInfoDTO>(workerTask);
        await _cache.SetRecordAsync(idKey, taskDTO);

        return _mapper.Map<TaskMainInfoDTO>(workerTask);
    }
}
