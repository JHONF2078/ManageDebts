using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Application.Contracts.Auth;

namespace ManageDebts.Application.Auth.Register.Commands;
public sealed record RegisterCommand(
    string Email, string Password, string FullName)
    : IRequest<Result<AuthenticationResponse>>;