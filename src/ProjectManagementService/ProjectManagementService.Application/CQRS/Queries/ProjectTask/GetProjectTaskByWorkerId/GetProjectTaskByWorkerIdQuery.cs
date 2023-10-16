using MediatR;
using ProjectManagementService.Application.TaskDTOs;

namespace ProjectManagementService.Application.CQRS.ProjectTaskQueries;

public sealed record GetProjectTaskByWorkerIdQuery(string WorkerId) : IRequest<TaskMainInfoDTO> { }
