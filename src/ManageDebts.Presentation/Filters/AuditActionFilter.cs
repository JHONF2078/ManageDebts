using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ManageDebts.Presentation.Filters
{
    // Este filtro usa ILogger<T> y no depende directamente de Serilog
    public class AuditActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<AuditActionFilter> _logger;

        public AuditActionFilter(ILogger<AuditActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Obtiene el usuario autenticado de forma segura
            var user = context.HttpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(user))
            {
                user = context.HttpContext.User.FindFirst("email")?.Value
                    ?? context.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
                    ?? context.HttpContext.User.FindFirst("sub")?.Value
                    ?? "Anonymous";
            }
            var action = context.ActionDescriptor.DisplayName;
            var time = DateTime.UtcNow;

            _logger.LogInformation("User {User} is executing {Action} at {Time}", user, action, time);

            // Ejecuta la acción
            var resultContext = await next();

            _logger.LogInformation("User {User} finished {Action} at {Time}", user, action, DateTime.UtcNow);
        }
    }
}
