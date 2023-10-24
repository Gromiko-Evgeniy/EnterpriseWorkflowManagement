using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Cache;
using HiringService.Application.Exceptions.Worker;
using HiringService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace HiringService.Application.CQRS.WorkerQueries;

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
        var emailKey = RedisKeysPrefixes.WorkerPrefix + request.Email;
        var cachedWorker = await _cache.GetRecordAsync<Worker>(emailKey);

        if (cachedWorker is not null) return cachedWorker;

        var worker = await _workerRepository.GetByEmailAsync(request.Email);
        if (worker is null) throw new NoWorkerWithSuchEmailException();

        await _cache.SetRecordAsync(emailKey, worker);

        return worker;
    }
}
