using System.Collections.Generic;
using System.Threading.Tasks;
using ManageDebts.Application.Debts;

namespace ManageDebts.Application.Common.Interfaces
{
    public interface ICacheService
    {      
        Task SetDebtDetailAsync(string debtId, DebtDto debt);
        Task<DebtDto?> GetDebtDetailAsync(string debtId);
    }
}
