using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace HiringService.Infrastructure.Data.Repositories;

public class GenericRepository<T> where T : class
{
    DbContext _context;
    DbSet<T> _dbSet;

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

    public async Task<T> AddAsync(T item)
    {
        _dbSet.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task RemoveAsync(T item)
    {
        _dbSet.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T item)
    {
        _dbSet.Update(item);
        await _context.SaveChangesAsync();
    }
}