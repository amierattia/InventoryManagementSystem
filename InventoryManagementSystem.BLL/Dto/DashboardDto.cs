using InventoryManagementSystem.EntitiesLayer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Dto
{
    public class DashboardDto
    {
        public int TotalItems { get; set; }
        public int LowStockItems { get; set; }
        public int UsersCount { get; set; }
        public int PendingOrders { get; set; }
        public List<ActivityModel> RecentActivities { get; set; }
    }
}
