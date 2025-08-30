using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;


namespace ManageDebts.Application.Auth.Login
{
   public sealed class LoginHandler : IRequestHandler<LoginCommand, Result<AuthenticationResponse>>
{
    private readonly IAuthService _auth;
    public LoginHandler(IAuthService auth) => _auth = auth;

    public Task<Result<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken ct)
        => _auth.LoginAsync(request, ct);
}
}
