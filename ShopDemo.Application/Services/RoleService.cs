using AutoMapper;
using ShopDemo.Application.DTOs.RoleDtos;
using ShopDemo.Application.Interfaces.IRepositories;
using ShopDemo.Application.Interfaces.IServices;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleService(IRoleRepository roleRepository, IMapper  mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;   
        }
        public async Task<Role> AddRoleAsync(RoleCreatedDto role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role), "Role cannot be null");
            }
            var newRole = _mapper.Map<Role>(role);
            newRole.IsActive = true;
            await _roleRepository.AddRoleAsync(newRole);
            return newRole;
        }
        

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return roleDtos;
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if(role == null || role.IsActive == false)
            {
                throw new ArgumentException("Role not found");
            }
            
            return role;
        }

        public async Task<Role> UpdateRoleAsync(int id, RoleUpdatedDto updatedDto)
        {
            var existRole = await _roleRepository.ExistByNameAsync(updatedDto.Name);
            if (existRole)
            {
                throw new ArgumentException("Role name existed");
            }
            var role = await _roleRepository.UpdateRoleAsync(id, updatedDto.Name, updatedDto.IsActive);
            if(role == null)
            {
                throw new ArgumentException("Role not found");
            }
            return role;
        }
    }
}
