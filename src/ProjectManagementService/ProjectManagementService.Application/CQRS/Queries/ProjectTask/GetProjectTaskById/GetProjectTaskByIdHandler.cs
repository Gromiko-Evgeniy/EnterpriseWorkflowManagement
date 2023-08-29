using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.DTOs.ProjectTaskDTOs;
using ProjectManagementService.Application.Exceptions.ProjectTask;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTaskByIdHandler : IRequestHandler<GetProjectTaskByIdQuery, TaskMainInfoDTO>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IMapper _mapper;

    public GetProjectTaskByIdHandler(IProjectTaskRepository repository, IMapper mapper)
    {
        _projectTaskRepository = repository;
        _mapper = mapper;
    }

    public async Task<TaskMainInfoDTO> Handle(GetProjectTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _projectTaskRepository.GetByIdAsync(request.Id);

        if (task == null) throw new NoProjectTaskWithSuchIdException();

        return _mapper.Map<TaskMainInfoDTO>(task);
    }
}
