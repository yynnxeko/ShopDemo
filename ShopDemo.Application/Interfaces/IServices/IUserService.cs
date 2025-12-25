using ShopDemo.Application.DTOs.UserDtos;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUserByNameAsync(string name);
        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
        Task<UserDto> UpdateAsync(Guid id, UserUpdatedDto user);
        Task UpdatePasswordAsync(Guid id, string password);
    }
}
