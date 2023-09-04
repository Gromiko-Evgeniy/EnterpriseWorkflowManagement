using MediatR;
using ProjectManagementService.Application.DTOs;

namespace HiringService.Application.CQRS.WorkerCommands;

public sealed record AddWorkerCommand(NameEmailDTO NameEmailDTO) : IRequest<int> { }
