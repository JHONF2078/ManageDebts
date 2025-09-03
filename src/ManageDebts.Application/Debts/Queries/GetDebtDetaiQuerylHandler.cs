// Application/Debts/Queries/GetDetail/GetDebtDetailHandler.cs
using ManageDebts.Application.Common.Exceptions;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Domain.Repositories;
using MediatR;

namespace ManageDebts.Application.Debts.Queries.GetDetail;

public sealed class GetDebtDetaiQuerylHandler
  : IRequestHandler<GetDebtDetailQuery, DebtDto?>
{
    private readonly IDebtRepository _repo;
    private readonly IUserService _users;
    private readonly ICacheService _cache;

    public GetDebtDetaiQuerylHandler(IDebtRepository repo, IUserService users, ICacheService cache)
        => (_repo, _users, _cache) = (repo, users, cache);

    public async Task<DebtDto?> Handle(GetDebtDetailQuery q, CancellationToken ct)
    {
        // 1) Cache
        var cached = await _cache.GetDebtDetailAsync(q.Id.ToString(), ct);
        if (cached is not null) return cached;

        // 2) BD
        var debt = await _repo.GetByIdAsync(q.Id, ct);
        if (debt is null)
            throw new NotFoundException($"Deuda no encontrada (id: {q.Id})");
        if (debt.UserId != q.UserId)
            throw new NotFoundException("No tienes acceso a esta deuda.");

        // 3) Enriquecer
        var debtorName = await _users.GetFullNameAsync(debt.UserId, ct);
        var creditorName = await _users.GetFullNameAsync(debt.CreditorId, ct);

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

        // 4) Cache set
        await _cache.SetDebtDetailAsync(q.Id.ToString(), dto, ct);
        return dto;
    }
}
