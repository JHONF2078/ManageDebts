// Application/Debts/Commands/Delete/DeleteDebtHandler.cs
using MediatR;
using ManageDebts.Application.Common;
using ManageDebts.Domain.Repositories;

namespace ManageDebts.Application.Debts.Commands.Delete;

public sealed class DeleteDebtHandler
  : IRequestHandler<DeleteDebtCommand, Result<Unit>>
{
    private readonly IDebtRepository _repo;
    public DeleteDebtHandler(IDebtRepository repo) => _repo = repo;

    public async Task<Result<Unit>> Handle(DeleteDebtCommand cmd, CancellationToken ct)
    {
        var debt = await _repo.GetByIdAsync(cmd.Id, ct);
        if (debt is null || debt.UserId != cmd.UserId)
            return Result<Unit>.Failure("Deuda no encontrada o no pertenece al usuario.");
        if (debt.IsPaid)
            return Result<Unit>.Failure("No se puede eliminar una deuda pagada.");

        await _repo.DeleteAsync(cmd.Id, ct);
        return Result<Unit>.Success(Unit.Value);
    }
}