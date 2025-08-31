using ManageDebts.Domain.Repositories;
using ManageDebts.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace ManageDebts.Application.Debts
{
    public class GetDebtDetailHandler
    {
        private readonly IDebtRepository _repo;
        private readonly IUserService _userService;
        private readonly ICacheService _cache;
        public GetDebtDetailHandler(IDebtRepository repo, IUserService userService, ICacheService cache)
        {
            _repo = repo;
            _userService = userService;
            _cache = cache;
        }

        public async Task<DebtDto?> Handle(Guid debtId)
        {
            // Intentar obtener de cache
            var cached = await _cache.GetDebtDetailAsync(debtId.ToString());
            if (cached != null) return cached;

            var debt = await _repo.GetByIdAsync(debtId);
            if (debt == null) return null;

            var debtorName = await _userService.GetFullNameAsync(debt.UserId);
            var creditorName = await _userService.GetFullNameAsync(debt.CreditorId);

            var dto = new DebtDto
            {
                Id = debt.Id,
                UserId = debt.UserId,
                DebtorName = debtorName,
                CreditorId = debt.CreditorId,
                CreditorName = creditorName,
                Amount = debt.Amount,
                Description = debt.Description,
                IsPaid = debt.IsPaid,
                CreatedAt = debt.CreatedAt,
                PaidAt = debt.PaidAt
            };
            await _cache.SetDebtDetailAsync(debtId.ToString(), dto);
            return dto;
        }
    }
}
