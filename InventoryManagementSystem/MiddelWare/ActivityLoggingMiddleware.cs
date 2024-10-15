using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.BLL.Services;
using System.Security.Claims;

namespace InventoryManagementSystem.Pl.MiddelWare
{
    public class ActivityLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ActivityLoggingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var description = $"Requested {context.Request.Path}";

            if (string.IsNullOrEmpty(userId))
            {
                description += " by an anonymous user";
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var activityService = scope.ServiceProvider.GetRequiredService<IActivityService>();
                await activityService.LogActivity(description, userId);
            }

            await _next(context);
        }
    }
}
