using ManageDebts.Application.Common;
using ManageDebts.Domain.Entities;
using MediatR;

namespace ManageDebts.Application.Debts.Commands
{
    public sealed record EditDebtCommand(
     Guid Id,
     decimal Amount,
     string Description,
     string? UserId
 ) : IRequest<Result<Debt>>;
}
