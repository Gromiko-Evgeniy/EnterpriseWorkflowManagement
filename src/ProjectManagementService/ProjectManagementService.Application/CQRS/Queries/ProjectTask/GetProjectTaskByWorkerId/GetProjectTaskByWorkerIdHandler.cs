using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.DTOs.ProjectTaskDTOs;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTaskByWorkerIdHandler : IRequestHandler<GetProjectTaskByWorkerIdQuery, TaskMainInfoDTO>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IMapper _mapper;

    public GetProjectTaskByWorkerIdHandler(IWorkerRepository repository,
        IProjectTaskRepository projectTaskRepository, IMapper mapper)
    {
        _workerRepository = repository;
        _projectTaskRepository = projectTaskRepository;
        _mapper = mapper;
    }

    public async Task<TaskMainInfoDTO> Handle(GetProjectTaskByWorkerIdQuery request, CancellationToken cancellationToken)
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

        return _mapper.Map<TaskMainInfoDTO>(task);
    }
}
