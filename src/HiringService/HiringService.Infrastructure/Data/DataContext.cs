using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HiringService.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }

    public DbSet<Worker> Workers { get; set; }

    public DbSet<Candidate> Candidates { get; set; }

    public DbSet<HiringStageName> HiringStageNames { get; set; }

    public DbSet<HiringStage> HiringStages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
