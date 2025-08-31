using ManageDebts.Application.Debts;
using ManageDebts.Domain.Repositories;
using ManageDebts.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public DebtsController(IDebtRepository repo, IUserService userService, ICacheService cache)
        {
            _repo = repo;
            _userService = userService;
            _cache = cache;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDebtCommand cmd, CancellationToken ct)
        {
            var handler = new CreateDebtHandler(_repo);
            var result = await handler.Handle(cmd, GetUserId(), ct);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditDebtCommand cmd, CancellationToken ct)
        {
            cmd.Id = id;
            var handler = new EditDebtHandler(_repo);
            var result = await handler.Handle(cmd, GetUserId(), ct);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var handler = new DeleteDebtHandler(_repo);
            var success = await handler.Handle(id, GetUserId());
            if (!success) return BadRequest("No se puede eliminar la deuda (no existe o está pagada)");
            return NoContent();
        }

        [HttpPost("{id}/pay")]
        public async Task<IActionResult> Pay(Guid id, CancellationToken ct)
        {
            var handler = new PayDebtHandler(_repo);
            var result = await handler.Handle(id, GetUserId(), ct);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] bool? isPaid)
        {
            var handler = new ListDebtsHandler(_repo, _userService);
            var result = await handler.Handle(GetUserId(), isPaid);
            return Ok(result);
        }

        
        [HttpGet("export")]
        public async Task<IActionResult> Export([FromQuery] string format = "json")
        {
            var handler = new ListDebtsHandler(_repo, _userService);
            var debts = await handler.Handle(GetUserId());
            if (format.ToLower() == "csv")
            {
                var csv = string.Join("\n", debts.Select(d => $"{d.Id},{d.Amount},{d.Description},{d.IsPaid},{d.CreatedAt},{d.PaidAt},{d.DebtorName},{d.CreditorName}"));
                return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "deudas.csv");
            }
            return Ok(debts);
        }

        
        [HttpGet("summary")]
        public async Task<IActionResult> Summary()
        {
            var handler = new ListDebtsHandler(_repo, _userService);
            var debts = await handler.Handle(GetUserId());
            var totalPagadas = debts.Where(d => d.IsPaid).Sum(d => d.Amount);
            var saldoPendiente = debts.Where(d => !d.IsPaid).Sum(d => d.Amount);
            return Ok(new { totalPagadas, saldoPendiente });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var handler = new GetDebtDetailHandler(_repo, _userService, _cache);
            var result = await handler.Handle(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
