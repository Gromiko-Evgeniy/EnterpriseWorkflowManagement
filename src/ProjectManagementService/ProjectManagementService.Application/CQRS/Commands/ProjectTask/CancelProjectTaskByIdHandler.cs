﻿using MediatR;
using ProjectManagementService.Application.Abstractions;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.Exceptions.ProjectTask;

namespace ProjectManagementService.Application.CQRS.ProjectTaskCommands;

public class CancelProjectTaskByIdHandler : IRequestHandler<CancelProjectTaskByIdCommand>
{
    private readonly IProjectTaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public CancelProjectTaskByIdHandler(IProjectTaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    async Task<Unit> IRequestHandler<CancelProjectTaskByIdCommand, Unit>.Handle(CancelProjectTaskByIdCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.ProjectTaskId);

        if (task is null) throw new NoProjectTaskWithSuchIdException();

        var customerProjects = await _projectRepository.GetAllCustomerProjectsAsync(request.CustomerId);

        if (!customerProjects.Any(p => p.Id == task.ProjectId)) throw new AccessToCancelProjecTaskDeniedException();

        await _taskRepository.CancelAsync(request.ProjectTaskId);

        return Unit.Value; //fake empty value
    }
}