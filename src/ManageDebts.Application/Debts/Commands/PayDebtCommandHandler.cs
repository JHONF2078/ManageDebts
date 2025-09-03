// Application/Debts/Commands/Pay/PayDebtHandler.cs
using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;

namespace ManageDebts.Application.Debts.Commands.Pay;

public sealed class PayDebtCommandHandler
    : IRequestHandler<PayDebtCommand, Result<Debt>>
{
    private readonly IDebtRepository _repo;

    public PayDebtCommandHandler(IDebtRepository repo) => _repo = repo;

    public async Task<Result<Debt>> Handle(PayDebtCommand cmd, CancellationToken ct)
    {
        var debt = await _repo.GetByIdAsync(cmd.Id, ct);
        if (debt is null || debt.UserId != cmd.UserId)
            return Result<Debt>.Failure("Deuda no encontrada.");

        if (debt.IsPaid)
            return Result<Debt>.Failure("La deuda ya está pagada.");

        debt.IsPaid = true;
        debt.PaidAt = DateTime.UtcNow;

        await _repo.UpdateAsync(debt, ct);
        return Result<Debt>.Success(debt);
    }
}
