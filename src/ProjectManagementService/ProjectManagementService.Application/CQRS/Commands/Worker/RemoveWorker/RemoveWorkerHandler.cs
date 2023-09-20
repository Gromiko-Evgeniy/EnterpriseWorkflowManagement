using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Application.CQRS.WorkerCommands;

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

        await _workerRepository.RemoveAsync(worker.Id);

        return Unit.Value;
    }
}
