using Azure.Core;
using Microsoft.Extensions.Configuration;
using ShopDemo.Application.DTOs.RefreshTokenDtos;
using ShopDemo.Application.Helper;
using ShopDemo.Application.Interfaces.IRepositories;
using ShopDemo.Application.Interfaces.IServices;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration, IUserRepository userRepository)
        {
             _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<RefreshToken>> GetAllByUserIdAsync(Guid userId)
        {
            var listRefreshToken = await _refreshTokenRepository.GetAllByUserIdAsync(userId);
            return listRefreshToken;
        }

        public async Task<(bool Success, RefreshResponseDto? Response, string Message)> RefreshAsync(RefreshRequestDto request)
        {
            var refreshToken = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(request.RefreshToken);
            if (refreshToken == null)
            {
                return (false, null, "Refresh token không hợp lệ hoặc đã hết hạn");
            }

            var user = await _userRepository.GetUserIncludeRoleByIdAsync(refreshToken.UserId);
            if (user == null)
            {
                return (false, null, "Người dùng không tồn tại");
            }
            
            var newAccessToken = AuthHelper.GenerateJwtToken(user, _configuration);
            var newRefreshToken = Guid.NewGuid().ToString();

            var newEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            await _refreshTokenRepository.AddAsync(newEntity);
            await _refreshTokenRepository.UpdateAsync(refreshToken.Id, true);

            var response = new RefreshResponseDto(newAccessToken, newRefreshToken,"Refresh token thành công");
            return (true, response, "Refresh thành công");
        }

        public async Task RevokeAllRefreshTokensAsync(Guid userId)
        {
            var listToken = await _refreshTokenRepository.GetAllByUserIdAsync(userId);
            foreach (var token in listToken) 
            {
                await _refreshTokenRepository.UpdateAsync(token.Id, true);
            }
        }

        public async Task<(bool Success, string Message)> RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(refreshToken);
            if(token == null || token.IsRevoked)
            {
                return (false, "Refresh token không tồn tại hoặc đã bị revoke");
            }
            await _refreshTokenRepository.UpdateAsync(token.Id, true);
            return (true, "Logout thành công");
        }

        
    }
}
