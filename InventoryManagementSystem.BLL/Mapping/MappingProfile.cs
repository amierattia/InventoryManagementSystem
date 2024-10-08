using AutoMapper;
using InventoryManagementSystem.BLL.Dto;
using Microsoft.AspNetCore.Identity;

namespace InventoryManagementSystem.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping between IdentityRole and RoleDto
            CreateMap<IdentityRole, RoleDto>();
        }
    }
}
