using ManageDebts.Application.Common;
using ManageDebts.Application.Debts.Commands;
using ManageDebts.Domain.Entities;
using ManageDebts.Domain.Repositories;
using MediatR;

public sealed class CreateDebtCommandHandler
    : IRequestHandler<CreateDebtCommand, Result<Debt>>
{
    private readonly IDebtRepository _repo;

    public CreateDebtCommandHandler(IDebtRepository repo) => _repo = repo;

    public async Task<Result<Debt>> Handle(CreateDebtCommand cmd, CancellationToken ct)
    {
        if (cmd.Amount <= 0)
            return Result<Debt>.Failure("El monto debe ser positivo.");
        if (string.IsNullOrWhiteSpace(cmd.CreditorId))
            return Result<Debt>.Failure("El acreedor es obligatorio.");
        if (cmd.CreditorId == cmd.UserId)
            return Result<Debt>.Failure("No puedes ser tu propio acreedor.");

        var debt = new Debt
        {
            Id = Guid.NewGuid(),
            UserId = cmd.UserId,
            CreditorId = cmd.CreditorId,
            Amount = cmd.Amount,
            Description = cmd.Description,
            IsPaid = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(debt, ct);
        return Result<Debt>.Success(debt);
    }
}
