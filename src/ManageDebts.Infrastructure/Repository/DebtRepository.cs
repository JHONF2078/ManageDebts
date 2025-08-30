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

        public async Task<Debt?> GetByIdAsync(Guid id)
        {
            return await _db.Debts.FindAsync(id);
        }

        public async Task<IEnumerable<Debt>> GetByUserAsync(string userId, bool? isPaid = null)
        {
            var query = _db.Debts.AsQueryable().Where(d => d.UserId == userId);
            if (isPaid.HasValue)
                query = query.Where(d => d.IsPaid == isPaid.Value);
            return await query.ToListAsync();
        }

        public async Task AddAsync(Debt debt)
        {
            _db.Debts.Add(debt);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Debt debt)
        {
            _db.Debts.Update(debt);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var debt = await _db.Debts.FindAsync(id);
            if (debt != null)
            {
                _db.Debts.Remove(debt);
                await _db.SaveChangesAsync();
            }
        }
    }
}
