using ManageDebts.Application.Auth.Login.Commands;
using ManageDebts.Application.Auth.Register.Commands;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;
using ManageDebts.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ManageDebts.Application.Common; // Importa ErrorType

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
            if (result.IsSuccess)
                return Ok(result.Value);

            var statusCode = result.ErrorType switch
            {
                ErrorType.Unauthorized => 401,
                ErrorType.NotFound => 404,
                ErrorType.Validation => 400,
                ErrorType.Conflict => 409,
                _ => 500
            };

            //if (!result.IsSuccess)
            //return BadRequest(new { error = result.Error, type = result.ErrorType });
            //Problem  formato estándar para errores HTTP
            return Problem(detail: result.Error, statusCode: statusCode);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand cmd, CancellationToken ct)
        {
            var result = await _mediator.Send(cmd, ct);
            if (result.IsSuccess)
                return Ok(result.Value);

            var statusCode = result.ErrorType switch
            {
                ErrorType.Unauthorized => 401,
                ErrorType.NotFound => 404,
                ErrorType.Validation => 400,
                ErrorType.Conflict => 409,
                _ => 500
            };

            return Problem(detail: result.Error, statusCode: statusCode);
        }

        [HttpPost("generate-new-jwt-token")]
        public async Task<IActionResult> Refresh(TokenModel model, CancellationToken ct)
        {
            var result = await _auth.RefreshAsync(model, ct);
            if (result.IsSuccess)
                return Ok(result.Value);

            var statusCode = result.ErrorType switch
            {
                ErrorType.Unauthorized => 401,
                ErrorType.NotFound => 404,
                ErrorType.Validation => 400,
                ErrorType.Conflict => 409,
                _ => 500
            };

            return Problem(detail: result.Error, statusCode: statusCode);
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
