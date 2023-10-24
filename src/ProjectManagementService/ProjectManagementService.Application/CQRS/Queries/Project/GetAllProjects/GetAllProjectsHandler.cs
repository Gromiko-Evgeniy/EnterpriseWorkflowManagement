using MediatR;
using AutoMapper;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetAllProjectsHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectShortInfoDTO>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetAllProjectsHandler(
        IProjectRepository repository,
        IMapper mapper)
    {
        _projectRepository = repository;
        _mapper = mapper;
    }

    public async Task<List<ProjectShortInfoDTO>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync();

        return projects.Select(_mapper.Map<ProjectShortInfoDTO>).ToList();
    }
}
