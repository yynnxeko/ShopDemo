using ShopDemo.Application.DTOs.RefreshTokenDtos;
using ShopDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Interfaces.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> UpdateAsync(int id, bool isRevoke);
        Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token);
        Task<List<RefreshToken>> GetAllByUserIdAsync(Guid userId);
    }
}
