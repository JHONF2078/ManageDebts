using ManageDebts.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ManageDebts.Application.Debts
{
    public class DeleteDebtHandler
    {
        private readonly IDebtRepository _repo;
        public DeleteDebtHandler(IDebtRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(Guid debtId, string userId)
        {
            var debt = await _repo.GetByIdAsync(debtId);
            if (debt == null || debt.UserId != userId) return false;
            if (debt.IsPaid) return false;
            await _repo.DeleteAsync(debtId);
            return true;
        }
    }
}
