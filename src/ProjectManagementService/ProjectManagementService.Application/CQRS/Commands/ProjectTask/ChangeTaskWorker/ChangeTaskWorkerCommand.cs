using MediatR;

namespace ProjectManagementService.Application.CQRS.Commands.ProjectTask.ChangeTaskWorker;

public sealed record ChangeTaskWorkerCommand(string WorkerId, string TaskId) : IRequest { }
