using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;


namespace ManageDebts.Application.Auth.Register.Commands
{
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthenticationResponse>>
    {
        private readonly IAuthService _auth;
        public RegisterCommandHandler(IAuthService auth) => _auth = auth;

        public Task<Result<AuthenticationResponse>> Handle(RegisterCommand request, CancellationToken ct)
            => _auth.RegisterAsync(request, ct);
    }
}
