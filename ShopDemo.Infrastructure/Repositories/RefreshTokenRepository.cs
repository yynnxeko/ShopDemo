using Microsoft.EntityFrameworkCore;
using ShopDemo.Application.DTOs.RefreshTokenDtos;
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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ShopDbContext _context;

        public RefreshTokenRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            _context.SaveChanges();
            return refreshToken;
        }

        public async Task<List<RefreshToken>> GetAllByUserIdAsync(Guid userId)
        {
            var listRefreshToken = await _context.RefreshTokens.Where(r => r.UserId == userId).ToListAsync();
            return listRefreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token)
        {
            return await _context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<RefreshToken?> UpdateAsync(int id,bool isRevoke)
        {
             var updateRefreshToken = await _context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            if (updateRefreshToken == null)
            { 
                return null;
            }
            updateRefreshToken.IsRevoked = isRevoke;
            await _context.SaveChangesAsync();
            return updateRefreshToken;
        }
    }
}
