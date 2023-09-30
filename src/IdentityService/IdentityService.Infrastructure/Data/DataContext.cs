using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityService.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }

    public DbSet<Worker> Workers { get; set; }

    public DbSet<Candidate> Candidates { get; set; }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
