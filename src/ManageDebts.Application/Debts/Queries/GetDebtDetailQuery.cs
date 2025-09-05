using MediatR;
using ManageDebts.Application.Common;

namespace ManageDebts.Application.Debts.Queries
{
    public sealed record GetDebtDetailQuery(Guid Id, string UserId)
     : IRequest<Result<DebtDto>>;
}
