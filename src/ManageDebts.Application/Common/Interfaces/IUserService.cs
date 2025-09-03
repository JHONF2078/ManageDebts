using System.Collections.Generic;
using System.Threading.Tasks;
using ManageDebts.Application.Users;

namespace ManageDebts.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<string> GetFullNameAsync(string userId, CancellationToken ct = default);
        Task<IReadOnlyList<UserShortDto>> GetAllUsersAsync(CancellationToken ct = default);
    }
}
