using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;

namespace ManageDebts.Application.Debts.Commands;

public sealed class EditDebtHandler
    : IRequestHandler<EditDebtCommand, Result<Debt>>
{
    private readonly IDebtRepository _repo;
    public EditDebtHandler(IDebtRepository repo) => _repo = repo;

    public async Task<Result<Debt>> Handle(EditDebtCommand cmd, CancellationToken ct)
    {
        var debt = await _repo.GetByIdAsync(cmd.Id, ct);
        if (debt is null || debt.UserId != cmd.UserId)
            return Result<Debt>.Failure("Deuda no encontrada.");
        if (debt.IsPaid)
            return Result<Debt>.Failure("No se puede modificar una deuda pagada.");
        if (cmd.Amount <= 0)
            return Result<Debt>.Failure("El monto debe ser positivo.");

        debt.Amount = cmd.Amount;
        debt.Description = cmd.Description;

        await _repo.UpdateAsync(debt, ct);
        return Result<Debt>.Success(debt);
    }
}
