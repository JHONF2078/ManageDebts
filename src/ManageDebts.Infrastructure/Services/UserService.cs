using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Users;
using ManageDebts.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // ToListAsync, AsNoTracking

namespace ManageDebts.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
            => _userManager = userManager;

        public async Task<string> GetFullNameAsync(string userId, CancellationToken ct = default)
        {
            // UserManager.FindByIdAsync no expone CancellationToken
            var user = await _userManager.FindByIdAsync(userId);
            return user?.FullName ?? string.Empty;
        }

        public async Task<IReadOnlyList<UserShortDto>> GetAllUsersAsync(CancellationToken ct = default)
        {
            return await _userManager.Users
                .AsNoTracking()
                .Select(u => new UserShortDto
                {
                    Id = u.Id,
                    FullName = u.FullName
                })
                .ToListAsync(ct);
        }
    }
}
