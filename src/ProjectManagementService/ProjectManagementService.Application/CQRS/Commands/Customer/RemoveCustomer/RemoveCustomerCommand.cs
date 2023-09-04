using MediatR;

namespace ProjectManagementService.Application.CQRS.CustomerCommands;

public sealed record RemoveCustomerCommand(string Email) : IRequest { }
