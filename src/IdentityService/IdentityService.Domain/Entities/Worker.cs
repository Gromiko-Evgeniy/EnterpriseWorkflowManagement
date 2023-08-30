using IdentityService.Domain.Enumerations;

namespace IdentityService.Domain.Entities;

public class Worker
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Position Position { get; set; }
}

 