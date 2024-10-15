using InventoryManagementSystem.EntitiesLayer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.interfaces
{
    public interface IInventoryService
    {
        Task<int> GetTotalItemsAsync();
        Task<int> GetLowStockItemsAsync();
        Task<int> GetUsersCountAsync();
        Task<List<ActivityModel>> GetRecentActivityAsync();
    }

}
