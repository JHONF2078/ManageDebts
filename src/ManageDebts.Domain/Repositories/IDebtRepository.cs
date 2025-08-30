using ManageDebts.Domain.Entities;

namespace ManageDebts.Domain.Repositories
{
    public interface IDebtRepository
    {
        Task<Debt?> GetByIdAsync(Guid id);
        Task<IEnumerable<Debt>> GetByUserAsync(string userId, bool? isPaid = null);
        Task AddAsync(Debt debt);
        Task UpdateAsync(Debt debt);
        Task DeleteAsync(Guid id);
    }
}
