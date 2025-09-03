using ManageDebts.Application.Auth.Login.Commands;
using ManageDebts.Application.Auth.Register.Commands;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;
using ManageDebts.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ManageDebts.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]   
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _auth;
        private readonly IUserService _userService;
        public AccountController(IMediator mediator, IAuthService auth, IUserService userService)
            => (_mediator, _auth, _userService) = (mediator, auth, userService);

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand cmd, CancellationToken ct)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand cmd, CancellationToken ct)
        {
            var result = await _mediator.Send(cmd, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
        }

        [HttpPost("generate-new-jwt-token")]
        public async Task<IActionResult> Refresh(TokenModel model, CancellationToken ct)
        {
            var result = await _auth.RefreshAsync(model, ct);
            return result.IsSuccess ? Ok(result.Value) : Problem(result.Error);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _auth.LogoutAsync(userId, ct);
            return NoContent();
        }

        [HttpGet("users")]
        [Authorize]
        public async Task<IActionResult> ListUsers()
        {
            var handler = new ListUsersHandler(_userService);
            var users = await handler.Handle();
            return Ok(users);
        }
    }
}
