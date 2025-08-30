using ManageDebts.Application.Auth.Login;
using ManageDebts.Application.Auth.Register;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Contracts.Auth;
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
        public AccountController(IMediator mediator, IAuthService auth)
            => (_mediator, _auth) = (mediator, auth);

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
    }
}
