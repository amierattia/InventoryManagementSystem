using InventoryManagementSystem.EntitiesLayer.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.sln.Services
{
    public class UserService /*: IUserService*/
    {
        //private readonly UserManager<User> _userManager;

        //public UserService(UserManager<User> userManager)
        //{
        //    _userManager = userManager;
        //}

        //public async Task<IdentityResult> RegisterCustomerAsync(User user, string password)
        //{
        //    var result = await _userManager.CreateAsync(user, password);
        //    if (result.Succeeded)
        //    {
        //        var iRole = await _userManager.IsInRoleAsync(user, "User");
        //        if (!iRole)
        //        {
        //            await _userManager.AddToRoleAsync(user, "User");
        //        }
        //    }
        //    return result;
        //}



        //public async Task<User> LoginCustomerAsync(string email, string password, bool rememberMe)
        //{
        //    var result = await _userManager.FindByEmailAsync(email);
        //    if (result != null && await _userManager.CheckPasswordAsync(result, password))
        //    {
        //        return result;
        //    }

        //    return null;
        //}




    }
}
