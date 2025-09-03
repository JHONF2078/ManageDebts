using ManageDebts.Application.Common;
using ManageDebts.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageDebts.Application.Debts.Commands
{
    public sealed record EditDebtCommand(
     Guid Id,
     decimal Amount,
     string Description,
     string? UserId
 ) : IRequest<Result<Debt>>;
}
