using MongoDB.Bson;
using ProjectManagementService.Domain.Entities;
using System.Linq.Expressions;

namespace ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

public interface IGenericRepository<T> where T : EntityWithId
{
    public Task<List<T>> GetAllAsync();

    public Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);

    public Task<T?> GetByIdAsync(string id);

    public Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate);

    public Task<string> AddOneAsync(T item);

    public Task AddManyAsync(List<T> items);

    public Task RemoveAsync(string id);

    public Task UpdateAsync(BsonDocument update, Expression<Func<T, bool>> predicate);
}
