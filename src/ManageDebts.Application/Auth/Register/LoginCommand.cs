using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Application.Contracts.Auth;

namespace ManageDebts.Application.Auth.Login
{
    public sealed record LoginCommand(string Email, string Password)
     : IRequest<Result<AuthenticationResponse>>;
}
