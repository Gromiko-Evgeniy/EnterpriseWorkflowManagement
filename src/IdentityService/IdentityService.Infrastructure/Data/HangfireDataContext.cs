using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

public class HangfireDataContext : DbContext
{
    public HangfireDataContext(DbContextOptions<HangfireDataContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }
}
