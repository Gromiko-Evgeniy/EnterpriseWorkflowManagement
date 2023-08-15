using System.Linq.Expressions;

namespace HiringService.Application.Abstractions;
public interface IGenericRepository<T> where T : class
{
    public Task<List<T>> GetAllAsync();

    public Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);

    public Task<T?> GetByIdAsync(int id);

    public Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate);

    public Task<T> AddAsync(T item);

    public Task RemoveAsync(T item);

    public Task UpdateAsync(T item);
}