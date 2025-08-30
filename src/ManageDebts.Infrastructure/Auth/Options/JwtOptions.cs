using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageDebts.Infrastructure.Auth.Options
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";
        public string Issuer { get; init; } = "";
        public string Audience { get; init; } = "";
        public string Key { get; init; } = "";
        public int ExpirationMinutes { get; init; }
    }
}
