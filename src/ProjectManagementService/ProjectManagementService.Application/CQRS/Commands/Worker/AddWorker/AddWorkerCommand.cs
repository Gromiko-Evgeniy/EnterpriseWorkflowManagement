using MediatR;
using ProjectManagementService.Application.DTOs;

namespace ProjectManagementService.Application.CQRS.WorkerCommands;

public sealed record AddWorkerCommand(NameEmailDTO NameEmailDTO) : IRequest<string> { }
