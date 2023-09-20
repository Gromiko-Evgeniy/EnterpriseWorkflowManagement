using AutoMapper;
using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Exceptions.Worker;
using MediatR;

namespace HiringService.Application.CQRS.WorkerCommands;

public class RemoveWorkerHandler : IRequestHandler<RemoveWorkerCommand>
{
    private readonly IWorkerRepository _workerRepository;

    public RemoveWorkerHandler(IWorkerRepository workerRepository)
    { 
        _workerRepository = workerRepository;
    }

    public async Task<Unit> Handle(RemoveWorkerCommand request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.
            GetFirstAsync(worker => worker.Email == request.Email);

        if (worker is null) throw new NoWorkerWithSuchEmailException();

        _workerRepository.Remove(worker);
        await _workerRepository.SaveChangesAsync();

        return Unit.Value;
    }
}
