using ManageDebts.Application.Auth.Login.Commands;
using ManageDebts.Application.Auth.Register.Commands;
using ManageDebts.Application.Common;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;
using ManageDebts.Infrastructure.Auth.Options;
using ManageDebts.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ManageDebts.Infrastructure.Auth
{
    internal sealed class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly RoleManager<ApplicationRole> _roles;
        private readonly IJwtService _jwt;
        private readonly RefreshTokenOptions _rtOptions;

        public AuthService(
            UserManager<ApplicationUser> users,
            RoleManager<ApplicationRole> roles,
            IJwtService jwt,
            IOptions<RefreshTokenOptions> refreshOptions)
        {
            _users = users; _roles = roles; _jwt = jwt;
            _rtOptions = refreshOptions.Value;
        }

        public async Task<Result<AuthenticationResponse>> RegisterAsync(RegisterCommand cmd, CancellationToken ct)
        {
            var user = new ApplicationUser
            {
                Email = cmd.Email,
                UserName = cmd.Email,
                FullName = cmd.FullName
            };

            var res = await _users.CreateAsync(user, cmd.Password);
            if (!res.Succeeded)
                return Result<AuthenticationResponse>.Failure(string.Join(" | ", res.Errors.Select(e => e.Description)), ErrorType.Validation);

            var roles = await _users.GetRolesAsync(user);
            var auth = _jwt.CreateJwtToken(user.Id, user.Email!, roles);
            await SaveRefreshAsync(user, auth.RefreshToken, ct);

            return Result<AuthenticationResponse>.Success(auth);
        }

        public async Task<Result<AuthenticationResponse>> LoginAsync(LoginCommand cmd, CancellationToken ct)
        {
            var user = await _users.FindByEmailAsync(cmd.Email);
            if (user is null)
                return Result<AuthenticationResponse>.Failure("User not found", ErrorType.NotFound);

            var passwordValid = await _users.CheckPasswordAsync(user, cmd.Password);
            if (!passwordValid)
                return Result<AuthenticationResponse>.Failure("Invalid email or password", ErrorType.Unauthorized);

            var roles = await _users.GetRolesAsync(user);
            var auth = _jwt.CreateJwtToken(user.Id, user.Email!, roles);
            await SaveRefreshAsync(user, auth.RefreshToken, ct);

            return Result<AuthenticationResponse>.Success(auth);
        }

        public async Task<Result<AuthenticationResponse>> RefreshAsync(TokenModel model, CancellationToken ct)
        {
            var principal = _jwt.GetPrincipalFromJwtToken(model.Token);
            if (principal is null) return Result<AuthenticationResponse>.Failure("Invalid jwt access token", ErrorType.Unauthorized);

            var email = principal.FindFirstValue(ClaimTypes.Email);
            var user = email is null ? null : await _users.FindByEmailAsync(email);
            if (user is null) return Result<AuthenticationResponse>.Failure("User not found", ErrorType.NotFound);

            if (user.RefreshTokenHash is null || user.RefreshTokenExpiresUtc is null || user.RefreshTokenExpiresUtc <= DateTime.UtcNow)
                return Result<AuthenticationResponse>.Failure("Refresh token expired", ErrorType.Unauthorized);

            if (!VerifyRefresh(model.RefreshToken, user.RefreshTokenHash))
                return Result<AuthenticationResponse>.Failure("Invalid refresh token", ErrorType.Unauthorized);

            var roles = await _users.GetRolesAsync(user);
            var auth = _jwt.CreateJwtToken(user.Id, user.Email!, roles);
            await SaveRefreshAsync(user, auth.RefreshToken, ct); // rotación

            return Result<AuthenticationResponse>.Success(auth);
        }

        public async Task LogoutAsync(string userId, CancellationToken ct)
        {
            var user = await _users.FindByIdAsync(userId);
            if (user is not null)
            {
                user.RefreshTokenHash = null;
                user.RefreshTokenExpiresUtc = null;
                await _users.UpdateAsync(user);
            }
        }

        private async Task SaveRefreshAsync(ApplicationUser user, string refreshTokenPlain, CancellationToken ct)
        {
            user.RefreshTokenHash = Hash(refreshTokenPlain);
            user.RefreshTokenExpiresUtc = DateTime.UtcNow.AddMinutes(_rtOptions.ExpirationMinutes);
            await _users.UpdateAsync(user);
        }

        private static string Hash(string value)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(bytes);
        }
        private static bool VerifyRefresh(string provided, string storedHash) => Hash(provided) == storedHash;
    }
}
