using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;
using ManageDebts.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ManageDebts.Application.Debts
{
    public class ListDebtsHandler
    {
        private readonly IDebtRepository _repo;
        private readonly IUserService _userService;
        public ListDebtsHandler(IDebtRepository repo, IUserService userService)
        {
            _repo = repo;
            _userService = userService;
        }

        public async Task<IEnumerable<DebtDto>> Handle(string userId, bool? isPaid = null)
        {
            var debts = await _repo.GetByUserAsync(userId, isPaid);
            // Obtener nombres de deudor y acreedor
            var debtorName = await _userService.GetFullNameAsync(userId);
            var creditorIds = debts.Select(d => d.CreditorId).Distinct().ToList();
            var creditorNames = new Dictionary<string, string>();
            foreach (var creditorId in creditorIds)
            {
                creditorNames[creditorId] = await _userService.GetFullNameAsync(creditorId);
            }
            return debts.Select(d => new DebtDto
            {
                Id = d.Id,
                UserId = d.UserId,
                DebtorName = debtorName,
                CreditorId = d.CreditorId,
                CreditorName = creditorNames.ContainsKey(d.CreditorId) ? creditorNames[d.CreditorId] : string.Empty,
                Amount = d.Amount,
                Description = d.Description,
                IsPaid = d.IsPaid,
                CreatedAt = d.CreatedAt,
                PaidAt = d.PaidAt
            });
        }
    }
}
