using ManageDebts.Application.Common;
using ManageDebts.Application.Common.Interfaces;
using ManageDebts.Application.Debts.Commands;
using ManageDebts.Application.Debts.Queries;
using ManageDebts.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ManageDebts.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DebtsController : ControllerBase
    {
        private readonly IDebtRepository _repo;
        private readonly IUserService _userService;
        private readonly ICacheService _cache;
        private readonly IMediator _mediator;
        public DebtsController(IMediator mediator, IDebtRepository repo, IUserService userService, ICacheService cache)
        {
            _repo = repo;
            _userService = userService;
            _cache = cache;
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDebtCommand cmd, CancellationToken ct)
        {          
            var command = cmd with { UserId = GetUserId() };
            var result = await _mediator.Send(command, ct);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditDebtCommand cmd, CancellationToken ct)
        {
            // completamos datos que no vienen en el body
            var command = cmd with { Id = id, UserId = GetUserId() };

            var result = await _mediator.Send(command, ct);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var cmd = new DeleteDebtCommand(id, GetUserId());
            var result = await _mediator.Send(cmd, ct);

            if (!result.IsSuccess)
                return BadRequest(result.Error); // si prefieres, puedes mapear a NotFound/Forbidden

            return NoContent();
        }


         [HttpPost("{id}/pay")]
        public async Task<IActionResult> Pay(Guid id, CancellationToken ct)
        {
            var cmd = new PayDebtCommand(id, GetUserId());
            var result = await _mediator.Send(cmd, ct);

            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }


        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? isPaid, CancellationToken ct)
        {
            var query = new ListDebtsQuery(GetUserId(), isPaid);
            var result = await _mediator.Send(query, ct);
            return Ok(result);
        }


        [HttpGet("export")]
        public async Task<IActionResult> Export([FromQuery] string format = "json", [FromQuery] bool? isPaid = null, CancellationToken ct = default)
        {
            var debts = await _mediator.Send(new ListDebtsQuery(GetUserId(), isPaid), ct);

            if (string.Equals(format, "csv", StringComparison.OrdinalIgnoreCase))
            {
                // Helper local para CSV
                static string Csv(string? s) => s is null ? "" : $"\"{s.Replace("\"", "\"\"")}\"";

                var sb = new StringBuilder();
                sb.AppendLine("Id,Amount,Description,IsPaid,CreatedAt,PaidAt,DebtorName,CreditorId,CreditorName");

                foreach (var d in debts)
                {
                    var amount = d.Amount.ToString(CultureInfo.InvariantCulture);
                    var createdAt = d.CreatedAt.ToString("O", CultureInfo.InvariantCulture);  // ISO 8601
                    var paidAt = d.PaidAt?.ToString("O", CultureInfo.InvariantCulture) ?? "";

                    sb.AppendLine($"{d.Id},{amount},{Csv(d.Description)},{d.IsPaid},{createdAt},{paidAt},{Csv(d.DebtorName)},{Csv(d.CreditorId)},{Csv(d.CreditorName)}");
                }

                return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "deudas.csv");
            }

            return Ok(debts);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> Summary([FromQuery] bool? isPaid = null, CancellationToken ct = default)
        {
            var debts = await _mediator.Send(new ListDebtsQuery(GetUserId(), isPaid), ct);

            var totalPagadas = debts.Where(d => d.IsPaid).Sum(d => d.Amount);
            var saldoPendiente = debts.Where(d => !d.IsPaid).Sum(d => d.Amount);

            return Ok(new { totalPagadas, saldoPendiente, total = totalPagadas + saldoPendiente });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(Guid id, CancellationToken ct)
        {
            var query = new GetDebtDetailQuery(id, GetUserId());
            var result = await _mediator.Send(query, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            var statusCode = result.ErrorType switch
            {
                ErrorType.NotFound => 404,
                ErrorType.Unauthorized => 401,
                ErrorType.Validation => 400,
                ErrorType.Conflict => 409,
                _ => 500
            };

            return Problem(detail: result.Error, statusCode: statusCode);
        }
    }
}
