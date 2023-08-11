using HiringService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HiringService.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Worker> Workers { get; set; }

    public DbSet<Candidate> Candidates { get; set; }

    public DbSet<HiringStageName> HiringStageNames { get; set; }

    public DbSet<HiringStage> HiringStages { get; set; }
}
