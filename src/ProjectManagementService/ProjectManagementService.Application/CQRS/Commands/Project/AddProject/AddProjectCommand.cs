using MediatR;
using ProjectManagementService.Application.ProjectDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectCommands;

public sealed record AddProjectCommand(AddProjectDTO ProjectDTO, string CustomerId) : IRequest<string> { }
