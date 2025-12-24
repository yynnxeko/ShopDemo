using Microsoft.EntityFrameworkCore;
using ShopDemo.Application.DTOs.UserDtos;
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
    public class UserRepository : IUserRepository
    {
        private readonly ShopDbContext _shopDbContext;
        public UserRepository(ShopDbContext shopDbContext)
        {
            _shopDbContext = shopDbContext;
        }

        public async Task<User?> AddUserAsync(User user)
        {
            var newUser = await _shopDbContext.Users.AddAsync(user);
            await _shopDbContext.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _shopDbContext.Users.AsNoTracking().Include(x => x.Role).ToListAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _shopDbContext.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);           
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _shopDbContext.Users.FirstOrDefaultAsync(x =>x.Id == id);           
        }

        public async Task<User?> GetUserIncludeRoleByIdAsync(Guid id)
        {
            return await _shopDbContext.Users.Include(x =>x.Role).FirstOrDefaultAsync(y => y.Id == id);
        }

        public async Task<List<User>> GetUserByNameAsync(string name)
        {
            return await _shopDbContext.Users.AsNoTracking().Where(x => x.FullName.Contains(name)).ToListAsync();
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _shopDbContext.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<User?> UpdateUserAsync(Guid id, UserUpdatedDto dto)
        {
            var updateUser = await _shopDbContext.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
            if (updateUser == null)
            {
                return null;
            }
            updateUser.FullName = dto.FullName;
            updateUser.RoleId = dto.RoleId;
            await _shopDbContext.SaveChangesAsync();
            return updateUser;
        }

        public async Task<User?> UpdatePasswordAsync(Guid id, string password)
        {
            var user = await _shopDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user == null) 
                return null;

            user.PasswordHash = password;
            await _shopDbContext.SaveChangesAsync();
            return user;
        }
    }
}
