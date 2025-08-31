using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageDebts.Application.Users
{
    public class ListUsersHandler
    {
        private readonly IUserService _userService;
        public ListUsersHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IEnumerable<UserShortDto>> Handle()
        {
            // Aquí puedes agregar lógica de negocio si es necesario
            return await _userService.GetAllUsersAsync();
        }
    }
}
