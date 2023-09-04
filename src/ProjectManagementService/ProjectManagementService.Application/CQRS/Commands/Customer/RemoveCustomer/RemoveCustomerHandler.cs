using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Customer;

namespace ProjectManagementService.Application.CQRS.CustomerCommands;

public class RemoveCustomerHandler : IRequestHandler<RemoveCustomerCommand>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectTaskRepository _projectTaskRepository;

    public RemoveCustomerHandler(ICustomerRepository customerRepository,
        IProjectRepository projectRepository, IProjectTaskRepository projectTaskRepository)
    {
        _projectRepository = projectRepository;
        _customerRepository = customerRepository;
        _projectTaskRepository = projectTaskRepository;
    }

    public async Task<Unit> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository
            .GetFirstAsync(customer => customer.Email == request.Email);

        if (customer is null) throw new NoCustomerWithSuchIdException();

        var projects = await _projectRepository.GetFilteredAsync(p => p.CustomerId == customer.Id);

        foreach (var project in projects)
        {
            await _projectRepository.CancelAsync(project.Id);

            var tasks = await _projectTaskRepository.GetFilteredAsync(t => t.ProjectId == project.Id);

            foreach (var task in tasks)
            {
                await _projectTaskRepository.CancelAsync(task.Id);
            }
        }

        //grpc to remove customer from identity

        await _customerRepository.RemoveAsync(customer.Id);

        return Unit.Value;
    }
}
