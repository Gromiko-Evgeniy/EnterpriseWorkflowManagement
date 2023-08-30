using AutoMapper;
using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.WorkerDTOs;
using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Worker;
using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Application.ServiceAbstractions;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Enumerations;

namespace IdentityService.Application.Services.EntityServices;

public class WorkerService : IWorkerService
{
    private readonly IWorkerRepository _workerRepository;
    private readonly IMapper _mapper;

    public WorkerService(IWorkerRepository workerRepository, IMapper mapper)
    {
        _workerRepository = workerRepository;
        _mapper = mapper;
    }

    public async Task<List<GetWorkerDTO>> GetAllAsync()
    {
        var workers = await _workerRepository.GetAllAsync();

        var workerDtos = workers.Select(_mapper.Map<GetWorkerDTO>).ToList();

        return workerDtos;
    }

    public async Task<GetWorkerDTO> GetByEmailAndPasswordAsync(LogInData data)
    {
        var worker = await GetWorkerByEmailAndPasswordAsync(data.Email, data.Password);

        var workerDTO = _mapper.Map<GetWorkerDTO>(worker);

        return workerDTO;
    }

    public async Task<GetWorkerDTO> GetByEmailAsync(string email)
    {
        var worker = await GetByEmailAsync(email);

        var workerDTO = _mapper.Map<GetWorkerDTO>(worker);

        return workerDTO;
    }

    public async Task<LogInData> AddAsync(AddWorkerDTO workerDTO)
    {
        var oldWorker = await _workerRepository.
            GetFirstAsync(worker => worker.Email == workerDTO.Email);

        if (oldWorker is not null) throw new WorkerAlreadyExistsException();

        //send data to services

        var newWorker = _mapper.Map<Worker>(workerDTO);

        _workerRepository.Add(newWorker);
        await _workerRepository.SaveChangesAsync();

        return _mapper.Map<LogInData>(newWorker);
    }

    public async Task UpdateNameAsync(string email, string name)
    {
        var worker = await GetWorkerByEmailAsync(email);

        worker.Name = name;

        _workerRepository.Update(worker);
        await _workerRepository.SaveChangesAsync();

        //overwrite data in other databases
    }

    public async Task UpdatePasswordAsync(string email, string prevPassword, string newPassword)
    {
        var worker = await GetWorkerByEmailAndPasswordAsync(email, prevPassword);

        worker.Password = newPassword;

        _workerRepository.Update(worker);
        await _workerRepository.SaveChangesAsync();
    }

    public async Task PromoteAsync(string email)
    {
        var worker = await GetWorkerByEmailAsync(email);

        if (worker.Position == Position.DepartmentHead) throw new CanNotPromoteWorkerException();

        worker.Position = (Position)((int)worker.Position + 1);

        _workerRepository.Update(worker);
        await _workerRepository.SaveChangesAsync();
    }

    public async Task DemoteAsync(string email)
    {
        var worker = await GetWorkerByEmailAsync(email);

        if (worker.Position == Position.Worker) throw new CanNotDemoteWorkerException();

        worker.Position = (Position)((int)worker.Position - 1);

        _workerRepository.Update(worker);
        await _workerRepository.SaveChangesAsync();
    }

    public async Task DismissAsync(string email)
    {
        var worker = await GetWorkerByEmailAsync(email);

        _workerRepository.Remove(worker);
        await _workerRepository.SaveChangesAsync();

        //overwrite data in other databases
    }

    public async Task QuitAsync(string email, string password)
    {
        var worker = await GetWorkerByEmailAndPasswordAsync(email, password);

        _workerRepository.Remove(worker);
        await _workerRepository.SaveChangesAsync();

        //overwrite data in other databases
    }

    private async Task<Worker> GetWorkerByEmailAsync(string email)
    {
        var worker = await _workerRepository.
            GetFirstAsync(worker => worker.Email == email);

        if (worker is null) throw new NoWorkerWithSuchEmailException();

        return worker;
    }

    private async Task<Worker> GetWorkerByEmailAndPasswordAsync(string email, string password)
    {
        var worker = await GetWorkerByEmailAsync(email);

        if (worker.Password != password) throw new IncorrectPasswordException();

        return worker;
    }
}
