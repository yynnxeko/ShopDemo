using ShopDemo.Application.DTOs.UserDtos;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> AddUserAsync(User user);
        Task<bool> IsEmailExistAsync(string email);
        Task<List<User>> GetUserByNameAsync(string name);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserIncludeRoleByIdAsync(Guid id);
        Task<User?> UpdateUserAsync(Guid id, UserUpdatedDto dto);
        Task<User?> UpdatePasswordAsync(Guid id, string password);
    }
}
