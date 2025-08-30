using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;


namespace ManageDebts.Application.Auth.Register
{
    public sealed class RegisterHandler : IRequestHandler<RegisterCommand, Result<AuthenticationResponse>>
    {
        private readonly IAuthService _auth;
        public RegisterHandler(IAuthService auth) => _auth = auth;

        public Task<Result<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken ct)
            => _auth.RegisterAsync(request, ct);
    }
}
