using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;
using ManageDebts.Application.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManageDebts.Application.Debts
{
    public class EditDebtHandler
    {
        private readonly IDebtRepository _repo;
        public EditDebtHandler(IDebtRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<Debt>> Handle(EditDebtCommand cmd, string userId, CancellationToken ct)
        {
            var debt = await _repo.GetByIdAsync(cmd.Id);
            if (debt == null || debt.UserId != userId)
                return Result<Debt>.Failure("Deuda no encontrada.");
            if (debt.IsPaid)
                return Result<Debt>.Failure("No se puede modificar una deuda pagada.");
            if (cmd.Amount <= 0)
                return Result<Debt>.Failure("El monto debe ser positivo.");

            debt.Amount = cmd.Amount;
            debt.Description = cmd.Description;
            await _repo.UpdateAsync(debt);
            return Result<Debt>.Success(debt);
        }
    }
}
