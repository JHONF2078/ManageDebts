using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;
using ManageDebts.Application.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ManageDebts.Application.Debts
{
    public class CreateDebtHandler
    {
        private readonly IDebtRepository _repo;
        public CreateDebtHandler(IDebtRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<Debt>> Handle(CreateDebtCommand cmd, string userId, CancellationToken ct)
        {
            if (cmd.Amount <= 0)
                return Result<Debt>.Failure("El monto debe ser positivo.");
            if (string.IsNullOrWhiteSpace(cmd.CreditorId))
                return Result<Debt>.Failure("El acreedor es obligatorio.");
            if (cmd.CreditorId == userId)
                return Result<Debt>.Failure("No puedes ser tu propio acreedor.");

            var debt = new Debt
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreditorId = cmd.CreditorId,
                Amount = cmd.Amount,
                Description = cmd.Description,
                IsPaid = false,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(debt);
            return Result<Debt>.Success(debt);
        }
    }
}
