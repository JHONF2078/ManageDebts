using ManageDebts.Application.Common;
using ManageDebts.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ManageDebts.Application.Debts.Commands
{
    // Elimina [property: Required] y usa [Required] directamente en el parámetro
    public sealed record CreateDebtCommand(
        [Required] decimal Amount,
        [Required] string Description,
        [Required] string CreditorId,
        string? UserId
    ) : IRequest<Result<Debt>>;
}
