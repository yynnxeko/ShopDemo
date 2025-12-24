using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using ShopDemo.Application.DTOs.RoleDtos;
using ShopDemo.Application.Interfaces.IRepositories;
using ShopDemo.Core.Entities;
using ShopDemo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ShopDbContext _context;

        public RoleRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> AddRoleAsync(Role role)
        {           
            var entry = await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
       
        public async Task<bool> ExistByNameAsync(string name)
        {
            return await _context.Roles.AnyAsync(x => x.Name == name);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Role?> GetRoleByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Role?> UpdateRoleAsync(int id, string name, bool isActive)
        {           
            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRole == null)
            {
                return null;
            }
            existingRole.Name = name;
            existingRole.IsActive = isActive;         
            await _context.SaveChangesAsync();
            return existingRole;
        }
    }
}

