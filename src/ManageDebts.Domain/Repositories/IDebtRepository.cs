using ManageDebts.Domain.Entities;

namespace ManageDebts.Domain.Repositories
{
    public interface IDebtRepository
    {
        Task<Debt?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Debt>> GetByUserAsync(string userId, bool? isPaid = null, CancellationToken ct = default);
        Task AddAsync(Debt debt, CancellationToken ct = default);
        Task UpdateAsync(Debt debt, CancellationToken c);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
