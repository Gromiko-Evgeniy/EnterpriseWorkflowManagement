using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Worker;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.WorkerQueries;

public class GetWorkerByEmailHandler : IRequestHandler<GetWorkerByEmailQuery, Worker>
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IDistributedCache _cache;

    public GetWorkerByEmailHandler(IWorkerRepository workerRepository,
        IDistributedCache cache)
    {
        _workerRepository = workerRepository;
        _cache = cache;
    }

    public async Task<Worker> Handle(GetWorkerByEmailQuery request, CancellationToken cancellationToken)
    {
        var emailKey = "Worker_" + request.Email;
        var worker = await _cache.GetRecordAsync<Worker>(emailKey);

        if (worker is null)
        {
            worker = await _workerRepository.
                GetFirstAsync(worker => worker.Email == request.Email);
        }

        if (worker is null) throw new NoWorkerWithSuchEmailException();

        await _cache.SetRecordAsync(emailKey, worker);

        return worker;
    }
}
