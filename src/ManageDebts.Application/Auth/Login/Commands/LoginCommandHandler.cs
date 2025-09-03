using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;


namespace ManageDebts.Application.Auth.Login.Commands
{
   public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthenticationResponse>>
{
    private readonly IAuthService _auth;
    public LoginCommandHandler(IAuthService auth) => _auth = auth;

    public Task<Result<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken ct)
        => _auth.LoginAsync(request, ct);
}
}
