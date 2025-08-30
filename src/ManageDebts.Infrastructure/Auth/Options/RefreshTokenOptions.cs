using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageDebts.Infrastructure.Auth.Options
{
    public sealed class RefreshTokenOptions
    {
        public const string SectionName = "RefreshToken";
        public int ExpirationMinutes { get; init; }
    }
}
