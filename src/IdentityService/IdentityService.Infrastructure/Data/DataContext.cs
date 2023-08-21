using IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Worker> Workers { get; set; }

    public DbSet<Candidate> Candidates { get; set; }

    public DbSet<Customer> Customers { get; set; }
}

