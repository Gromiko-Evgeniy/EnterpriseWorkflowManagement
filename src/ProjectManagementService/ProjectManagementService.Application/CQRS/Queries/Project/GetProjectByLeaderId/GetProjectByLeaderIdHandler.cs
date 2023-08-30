using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.DTOs.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.ProjectQueries;

public class GetProjectByLeaderIdHandler : IRequestHandler<GetProjectByLeaderIdQuery, ProjectMainInfoDTO>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IMapper _mapper;


    public GetProjectByLeaderIdHandler(IProjectRepository repository,
        IWorkerRepository workerRepository, IMapper mapper)
    {
        _projectRepository = repository;
        _workerRepository = workerRepository;
        _mapper = mapper;
    }

    public async Task<ProjectMainInfoDTO> Handle(GetProjectByLeaderIdQuery request, CancellationToken cancellationToken)
    {
        var projectLeader = await _workerRepository.GetByIdAsync(request.ProjectLeaderId);

        if (projectLeader is null) throw new NoWorkerWithSuchIdException();

        var project = await _projectRepository.GetProjectByProjectLeaderId(request.ProjectLeaderId);

        if (project is null) throw new NoProjectWithSuchProjectLeader();

        return _mapper.Map<ProjectMainInfoDTO>(project);
    }
}
