using ShopDemo.Application.DTOs.RefreshTokenDtos;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Interfaces.IServices
{
    public  interface IRefreshTokenService
    {
        Task<(bool Success, RefreshResponseDto? Response, string Message)> RefreshAsync(RefreshRequestDto request);
        Task<(bool Success, string Message)> RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeAllRefreshTokensAsync(Guid userId);
        Task<IEnumerable<RefreshToken>> GetAllByUserIdAsync(Guid userId);
    }
}
