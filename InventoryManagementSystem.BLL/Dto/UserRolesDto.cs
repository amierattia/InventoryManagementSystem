using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.BLL.Dto
{

    public class UserRolesDto
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public List<RoleDto> Roles { get; set; }
    }

}
