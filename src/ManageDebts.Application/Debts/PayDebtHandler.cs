using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;
using ManageDebts.Application.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManageDebts.Application.Debts
{
    public class PayDebtHandler
    {
        private readonly IDebtRepository _repo;
        public PayDebtHandler(IDebtRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<Debt>> Handle(Guid debtId, string userId, CancellationToken ct)
        {
            var debt = await _repo.GetByIdAsync(debtId);
            if (debt == null || debt.UserId != userId)
                return Result<Debt>.Failure("Deuda no encontrada.");
            if (debt.IsPaid)
                return Result<Debt>.Failure("La deuda ya está pagada.");

            debt.IsPaid = true;
            debt.PaidAt = DateTime.UtcNow;
            await _repo.UpdateAsync(debt);
            return Result<Debt>.Success(debt);
        }
    }
}
