using System.Collections.Generic;
using System.Threading.Tasks;
using ManageDebts.Application.Debts;

namespace ManageDebts.Application.Common.Interfaces
{
    public interface ICacheService
    {
        Task SetDebtDetailAsync(string debtId, DebtDto debt, CancellationToken ct = default);
        Task<DebtDto?> GetDebtDetailAsync(string debtId, CancellationToken ct = default);
        Task RemoveDebtDetailAsync(string debtId, CancellationToken ct = default); // útil para invalidar
    }
}
