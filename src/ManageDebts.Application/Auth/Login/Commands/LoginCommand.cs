using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Application.Contracts.Auth;

namespace ManageDebts.Application.Auth.Login.Commands
{
    public sealed record LoginCommand(string Email, string Password)
     : IRequest<Result<AuthenticationResponse>>;
}
