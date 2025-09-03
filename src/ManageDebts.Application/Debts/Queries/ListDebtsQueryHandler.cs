// Application/Debts/Queries/List/ListDebtsQueryHandler.cs
using MediatR;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Domain.Repositories;

namespace ManageDebts.Application.Debts.Queries.List;

public sealed class ListDebtsQueryHandler
  : IRequestHandler<ListDebtsQuery, IReadOnlyList<DebtDto>>
{
    private readonly IDebtRepository _repo;
    private readonly IUserService _users;

    public ListDebtsQueryHandler(IDebtRepository repo, IUserService users)
        => (_repo, _users) = (repo, users);

    public async Task<IReadOnlyList<DebtDto>> Handle(ListDebtsQuery q, CancellationToken ct)
    {
        // 1) Traer deudas del usuario (solo lectura en repo como  AsNoTracking)
        var debts = await _repo.GetByUserAsync(q.UserId, q.IsPaid, ct);

        // 2) Nombres: deudor (siempre el mismo = dueno) + acreedores distintos
        var debtorName = await _users.GetFullNameAsync(q.UserId, ct);

        var creditorIds = debts.Select(d => d.CreditorId)
                               .Where(id => !string.IsNullOrWhiteSpace(id))
                               .Distinct()
                               .ToArray();

        // En paralelo
        var nameTasks = creditorIds.Select(id => _users.GetFullNameAsync(id!, ct)).ToArray();
        var names = await Task.WhenAll(nameTasks);

        var creditorNames = new Dictionary<string, string>(creditorIds.Length);
        for (int i = 0; i < creditorIds.Length; i++)
            creditorNames[creditorIds[i]!] = names[i];

        // 3) Mapear a DTO
        var dtos = debts.Select(d => new DebtDto
        {
            Id = d.Id,
            UserId = d.UserId,
            DebtorName = debtorName,
            CreditorId = d.CreditorId,
            CreditorName = creditorNames.TryGetValue(d.CreditorId, out var n) ? n : string.Empty,
            Amount = d.Amount,
            Description = d.Description,
            IsPaid = d.IsPaid,
            CreatedAt = d.CreatedAt,
            PaidAt = d.PaidAt
        }).ToList();

        return dtos;
    }
}
