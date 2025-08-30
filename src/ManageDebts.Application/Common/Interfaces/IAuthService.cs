using ManageDebts.Application.Auth.Login;
using ManageDebts.Application.Auth.Register;
using ManageDebts.Application.Contracts.Auth;


namespace ManageDebts.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthenticationResponse>> RegisterAsync(RegisterCommand command, CancellationToken ct);
        Task<Result<AuthenticationResponse>> LoginAsync(LoginCommand command, CancellationToken ct);
        Task<Result<AuthenticationResponse>> RefreshAsync(TokenModel model, CancellationToken ct);
        Task LogoutAsync(string userId, CancellationToken ct);
    }
}
