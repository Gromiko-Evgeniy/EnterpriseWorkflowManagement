namespace IdentityService.Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
