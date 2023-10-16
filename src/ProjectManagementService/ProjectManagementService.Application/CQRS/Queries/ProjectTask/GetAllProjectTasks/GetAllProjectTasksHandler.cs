using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetAllProjectTasksHandler : IRequestHandler<GetAllProjectTasksQuery, List<TaskShortInfoDTO>>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IMapper _mapper;

    public GetAllProjectTasksHandler(IProjectTaskRepository repository, IMapper mapper)
    {
        _projectTaskRepository = repository;
        _mapper = mapper;
    }

    public async Task<List<TaskShortInfoDTO>> Handle(GetAllProjectTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _projectTaskRepository.GetAllAsync();

        return tasks.Select(_mapper.Map<TaskShortInfoDTO>).ToList();
    }
}
