using System.Linq.Expressions;

namespace HiringService.Application.Abstractions.RepositoryAbstractions;
public interface IGenericRepository<T> where T : class
{
    public Task<List<T>> GetAllAsync();

    public Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);

    public Task<T?> GetByIdAsync(int id);

    public Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate);

    public T Add(T item);

    public void Remove(T item);

    public void Update(T item);

    public Task SaveChangesAsync();
}
