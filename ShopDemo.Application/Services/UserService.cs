using AutoMapper;
using ShopDemo.Application.DTOs.UserDtos;
using ShopDemo.Application.Helper;
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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new ArgumentException("User does not exists");
            }
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentException("User does not exists");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetUserByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return await _userRepository.GetAllUsersAsync();
            }
            return await _userRepository.GetUserByNameAsync(name);            
        }

        public async Task<UserDto> UpdateAsync(Guid id, UserUpdatedDto user)
        {
            var updateUser = await _userRepository.UpdateUserAsync(id, user);
            if (updateUser == null)
            {
                throw new ArgumentException("Update user fail");
            }
            return _mapper.Map<UserDto>(updateUser);
        }

        public async Task UpdatePasswordAsync(Guid id, string password) {
            var passwordHash = AuthHelper.HashPassword(password);
            await _userRepository.UpdatePasswordAsync(id, passwordHash);           
        }

        
    }
}
