using Microsoft.AspNetCore.SignalR;

namespace InventoryManagementSystem.Pl.Hubs
{

    public class StockHub : Hub
    {
        public async Task NotifyLowStock(string productName, int stock)
        {
            await Clients.All.SendAsync("ReceiveLowStockNotification", productName, stock);
        }
    }
}
