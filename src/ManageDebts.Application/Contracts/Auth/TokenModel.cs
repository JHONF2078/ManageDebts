using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageDebts.Application.Contracts.Auth
{
    public sealed record TokenModel(string Token, string RefreshToken);
}
