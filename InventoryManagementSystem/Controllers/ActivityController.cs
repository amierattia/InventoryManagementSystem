using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Pl.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public async Task<IActionResult> Index()
        {
            var activities = await _activityService.GetActivities();
            return View(activities);
        }
    }

}
