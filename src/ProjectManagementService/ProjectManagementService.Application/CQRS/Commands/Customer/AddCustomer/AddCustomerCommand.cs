using MediatR;
using ProjectManagementService.Application.DTOs;

namespace ProjectManagementService.Application.CQRS.CustomerCommands;

public sealed record AddCustomerCommand(NameEmailDTO NameEmailDTO) : IRequest<string> { }
