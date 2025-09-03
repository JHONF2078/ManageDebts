using ManageDebts.Application.Common;
using ManageDebts.Domain.Entities;
using MediatR;


namespace ManageDebts.Application.Debts.Commands
{

    //Si prefieres devolver un DTO, cambia Result<Debt> por Result<DebtDto> y mapea en el handler.
    public sealed record PayDebtCommand(Guid Id, string UserId)
        : IRequest<Result<Debt>>;
}
