using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageDebts.Application.Debts.Queries
{    public sealed record ListDebtsQuery(string UserId, bool? IsPaid = null)
    : IRequest<IReadOnlyList<DebtDto>>;
}
