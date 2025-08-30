using ManageDebts.Application.Contracts.Auth;
using System.Security.Claims;

namespace ManageDebts.Application.Common.Interfaces
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(string userId, string email, IEnumerable<string> roles);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string token);
    }
}
