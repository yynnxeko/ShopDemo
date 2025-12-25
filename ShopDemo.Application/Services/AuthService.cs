using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopDemo.Application.DTOs.AuthDtos;
using ShopDemo.Application.DTOs.UserDtos;
using ShopDemo.Application.Helper;
using ShopDemo.Application.Interfaces.IRepositories;
using ShopDemo.Application.Interfaces.IServices;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ShopDemo.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IMapper _mapper;
        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, IConfiguration config, IRefreshTokenRepository refreshTokenRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _config = config;
            _refreshTokenRepository = refreshTokenRepository;
            _mapper = mapper;
        }

        public async Task<(bool, string)> RegisterAsync(RegisterRequestDto request)
        {

            var existUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existUser != null)
            {
                return (false, "Email has been used");
            }
            var userRole = await _roleRepository.GetRoleByNameAsync("User");
            if (userRole == null)
            {
                return (false, "Role is not found");
            }
            var newUser = new User
            {
                Email = request.Email.Trim(),
                FullName = request.FullName.Trim(),
                RoleId = userRole.Id,
                PasswordHash = AuthHelper.HashPassword(request.Password),
                CreatedAt = DateTime.Now
            };

            await _userRepository.AddUserAsync(newUser);
            return (true, "Create user successfully");
        }

        public async Task<(bool Success, string? Token, string? RefreshToken, string Message)> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                return (false, null, null, "Incorrect email or password");
            }
            if (!AuthHelper.VerifyPassword(dto.Password, user.PasswordHash))
            {
                return (false, null, null, "Incorrect email or password");
            }
            var token = AuthHelper.GenerateJwtToken(user, _config);
            var refreshToken = Guid.NewGuid().ToString();
            var newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.Now.AddDays(7),
                CreatedAt = DateTime.Now,
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken);
            return (true, token, refreshToken, "Login successfully");

        }

        public async Task<(bool Success, string Message)> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return (false, "User không tồn tại");
            if (!AuthHelper.VerifyPassword(request.CurrentPassword, user.PasswordHash))
                return (false, "Mật khẩu cũ không đúng");

            string newPassword = AuthHelper.HashPassword(request.NewPassword);
            await _userRepository.UpdatePasswordAsync(user.Id, newPassword);
            return (true, "Đổi mật khẩu thành công");
        }
    }
}
