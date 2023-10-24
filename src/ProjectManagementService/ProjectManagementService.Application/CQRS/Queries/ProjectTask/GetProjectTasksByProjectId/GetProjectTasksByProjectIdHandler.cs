using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Application.Exceptions.Project;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public class GetProjectTasksByProjectIdHandler : IRequestHandler<GetProjectTasksByProjectIdQuery, List<TaskShortInfoDTO>>
{
    private readonly IProjectTaskRepository _projectTaskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetProjectTasksByProjectIdHandler(
        IProjectTaskRepository projectTaskRepository,
        IProjectRepository projectRepository,
        IMapper mapper)
    {
        _projectTaskRepository = projectTaskRepository;
        _projectRepository = projectRepository;
        _mapper = mapper; 
    }

    public async Task<List<TaskShortInfoDTO>> Handle(GetProjectTasksByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project is null) throw new NoProjectWithSuchIdException();

        var tasks = await _projectTaskRepository.GetByProjectIdAsync(request.ProjectId);

        return tasks.Select(_mapper.Map<TaskShortInfoDTO>).ToList();
    }
}
