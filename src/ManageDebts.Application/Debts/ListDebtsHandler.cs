using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageDebts.Application.Debts
{
    public class ListDebtsHandler
    {
        private readonly IDebtRepository _repo;
        public ListDebtsHandler(IDebtRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Debt>> Handle(string userId, bool? isPaid = null)
        {
            return await _repo.GetByUserAsync(userId, isPaid);
        }
    }
}
