using InventoryManagementSystem.BLL.interfaces;
using InventoryManagementSystem.DAL.Db;
using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.EntityFrameworkCore;


namespace InventoryManagementSystem.BLL.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationContext _context;

        public ActivityService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task LogActivity(string description, string userName)
        {
            var activity = new ActivityModel
            {
                Description = description,
                Date = DateTime.Now,
                UserName = userName
            };

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ActivityModel>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }
    }
}
