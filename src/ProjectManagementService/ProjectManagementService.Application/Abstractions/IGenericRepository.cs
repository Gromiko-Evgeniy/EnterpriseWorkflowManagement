using MongoDB.Bson;
using System.Linq.Expressions;

namespace HiringService.Application.Abstractions;

public interface IGenericRepository<T> where T : class
{
    public Task<List<T>> GetAllAsync();

    public Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);

    public Task<T?> GetByIdAsync(string id);

    public Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate);

    public Task<T> AddAsync(T item);

    public Task RemoveAsync(string id);

    public Task UpdateAsync(BsonDocument update, Expression<Func<T, bool>> predicate);
}