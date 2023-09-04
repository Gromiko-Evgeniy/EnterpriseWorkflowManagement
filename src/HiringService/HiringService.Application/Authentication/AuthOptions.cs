using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HiringService.Application.Authentication;

public class AuthOptions
{
    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string Secret { get; set; } = string.Empty;

    public int TokenLifeTime { get; set; } //minutes

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}
