using ManageDebts.Application.Common;
using MediatR;

namespace ManageDebts.Application.Debts.Commands
{
    public sealed record DeleteDebtCommand(Guid Id, string UserId)
    : IRequest<Result<Unit>>;
}
