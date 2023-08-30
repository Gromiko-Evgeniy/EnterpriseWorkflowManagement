using MediatR;
using ProjectManagementService.Application.DTOs.ProjectTaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetProjectTaskByIdQuery(string Id) : IRequest<TaskMainInfoDTO> { }
