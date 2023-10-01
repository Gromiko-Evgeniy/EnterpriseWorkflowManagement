using MediatR;
using ProjectManagementService.Application.TaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetProjectTaskByIdQuery(string Id) : IRequest<TaskMainInfoDTO> { }
