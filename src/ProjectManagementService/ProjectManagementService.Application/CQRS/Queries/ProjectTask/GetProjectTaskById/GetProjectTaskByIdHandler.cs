using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTaskByIdHandler : IRequestHandler<GetProjectTaskByIdQuery, TaskMainInfoDTO>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public GetProjectTaskByIdHandler(IProjectTaskRepository repository,
        IDistributedCache cache, IMapper mapper)
    {
        _projectTaskRepository = repository;
        _mapper = mapper;
        _cache = cache; 
    }

    public async Task<TaskMainInfoDTO> Handle(GetProjectTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var idKey = "Task_" + request.Id;
        var taskDTO = await _cache.GetRecordAsync<TaskMainInfoDTO>(idKey);
        if (taskDTO is not null) return taskDTO;
         
        var task = await _projectTaskRepository.GetByIdAsync(request.Id);
        if (task == null) throw new NoProjectTaskWithSuchIdException();

        taskDTO = _mapper.Map<TaskMainInfoDTO>(task);

        await _cache.SetRecordAsync(idKey, taskDTO);

        return taskDTO;
    }
}
