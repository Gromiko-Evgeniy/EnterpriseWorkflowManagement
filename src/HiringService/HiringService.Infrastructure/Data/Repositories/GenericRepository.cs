using HiringService.Application.Abstractions.RepositoryAbstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HiringService.Infrastructure.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<List<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public T Add(T item)
    {
        _dbSet.Add(item);

        return item;
    }

    public void Remove(T item)
    {
        _dbSet.Remove(item);
    }

    public void Update(T item)
    {
        _dbSet.Update(item);
    }

    public async Task SaveChangesAsync() 
    {
        await _context.SaveChangesAsync();
    }
}
