using Microsoft.AspNetCore.Identity;

namespace ManageDebts.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {       
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresUtc { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
