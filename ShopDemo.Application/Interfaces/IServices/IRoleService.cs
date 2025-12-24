using ShopDemo.Application.DTOs.RoleDtos;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Interfaces.IServices
{
    public interface IRoleService
    {
        Task<Role> AddRoleAsync(RoleCreatedDto role);
        Task<Role> UpdateRoleAsync(int id, RoleUpdatedDto updatedDto);
        Task<Role> GetRoleByIdAsync(int id);    
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    }
}
