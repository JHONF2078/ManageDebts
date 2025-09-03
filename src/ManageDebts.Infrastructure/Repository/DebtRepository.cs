using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;
using ManageDebts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ManageDebts.Infrastructure.Repository
{
    public class DebtRepository : IDebtRepository
    {
        private readonly AppDbContext _db;
        public DebtRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Debt?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            // return await _db.Debts.FindAsync(id);
            // Busca por PK; si ya está trackeada evita ir a la BD
            return await _db.Debts.FindAsync(new object?[] { id }, ct);
        }

        public async Task<IReadOnlyList<Debt>> GetByUserAsync(
         string userId,
         bool? isPaid = null,
         CancellationToken ct = default)
        {
            var query = _db.Debts
                .AsNoTracking()                 // mejora performance en lecturas
                .Where(d => d.UserId == userId);

            if (isPaid.HasValue)
                query = query.Where(d => d.IsPaid == isPaid.Value);

            return await query
                .OrderByDescending(d => d.CreatedAt) // opcional: orden consistente
                .ToListAsync(ct);
        }

        public async Task AddAsync(Debt debt, CancellationToken ct = default)
        {
            await _db.Debts.AddAsync(debt, ct);
            await _db.SaveChangesAsync(ct);
        }

        public Task UpdateAsync(Debt debt, CancellationToken ct = default)
        {
            // 'debt' ya está adjunta y con cambios detectados
            return _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var debt = await _db.Debts.FindAsync([id], ct);
            if (debt is not null)
            {
                _db.Debts.Remove(debt);
                await _db.SaveChangesAsync(ct);
            }
        }
    }
}
