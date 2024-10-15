using InventoryManagementSystem.EntitiesLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.interfaces
{
    public interface IActivityService
    {
        Task LogActivity(string description, string userName);
        Task<List<ActivityModel>> GetActivities();
    }

}
