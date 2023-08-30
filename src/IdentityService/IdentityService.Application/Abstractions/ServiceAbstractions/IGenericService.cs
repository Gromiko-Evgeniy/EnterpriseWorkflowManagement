using IdentityService.Application.DTOs;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Abstractions.ServiceAbstractions;

public interface IGenericService<TEntity, TDTO> where TEntity : class 
                                                where TDTO : class
{
    public Task<TEntity> GetByEmailAndPasswordAsync(LogInData data);

    public Task<LogInData> AddAsync(TDTO customerDTO);

    public Task UpdatePasswordAsync(string email, string prevPassword, string newPassword);
}
