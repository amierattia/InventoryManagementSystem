using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.sln.Services
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterCustomerAsync(Users User, string password );
        Task<Users> LoginCustomerAsync(string email, string password  , bool RememberMe);
    }
}
