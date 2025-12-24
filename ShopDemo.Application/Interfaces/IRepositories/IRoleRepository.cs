using ShopDemo.Application.DTOs.RoleDtos;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Interfaces.IRepositories
{
    public interface IRoleRepository
    {
        Task<Role?> AddRoleAsync(Role role);
        Task<Role?> UpdateRoleAsync(int id, string name, bool isActive);
        Task<Role?> GetRoleByIdAsync(int id);
        Task<Role?> GetRoleByNameAsync(string name);
        Task<List<Role>> GetAllRolesAsync();
        Task<bool> ExistByNameAsync(string name);
    }
}
