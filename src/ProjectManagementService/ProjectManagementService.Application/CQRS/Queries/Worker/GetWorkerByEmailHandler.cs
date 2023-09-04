using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.WorkerQueries;

public class GetWorkerByEmailHandler : IRequestHandler<GetWorkerByEmailQuery, Worker>
{
    private readonly IWorkerRepository _workerRepository;

    public GetWorkerByEmailHandler(IWorkerRepository workerRepository)
    {
        _workerRepository = workerRepository;
    }

    public async Task<Worker> Handle(GetWorkerByEmailQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _workerRepository.
            GetFirstAsync(customer => customer.Email == request.Email);

        if (candidate is null) throw new NoWorkerWithSuchEmailException();

        return candidate;
    }
}
