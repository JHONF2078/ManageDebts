using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;
using ManageDebts.Infrastructure.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ManageDebts.Infrastructure.Auth
{
    internal sealed class JwtService : IJwtService
    {
        private readonly JwtOptions _jwt;
        private readonly RefreshTokenOptions _refresh;

        public JwtService(IOptions<JwtOptions> jwt, IOptions<RefreshTokenOptions> refresh)
            => (_jwt, _refresh) = (jwt.Value, refresh.Value);

        public AuthenticationResponse CreateJwtToken(string userId, string email, IEnumerable<string> roles)
        {
            var nowUtc = DateTime.UtcNow;
            var expUtc = nowUtc.AddMinutes(_jwt.ExpirationMinutes);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)nowUtc).ToUnixTimeSeconds().ToString()),
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email),            
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: nowUtc,
                expires: expUtc,
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthenticationResponse(
                token,
                GenerateRefreshToken(),
                expUtc,
                email
            );
        }

        public ClaimsPrincipal? GetPrincipalFromJwtToken(string token)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwt.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwt.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, parameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
