using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using ManageDebts.Application.Users;

namespace ManageDebts.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> GetFullNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.FullName ?? string.Empty;
        }
        public async Task<IEnumerable<UserShortDto>> GetAllUsersAsync()
        {
            return _userManager.Users
                .Select(u => new UserShortDto { Id = u.Id, FullName = u.FullName })
                .ToList();
        }
    }
}
