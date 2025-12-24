using ShopDemo.Application.DTOs.AuthDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopDemo.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<(bool, string)> RegisterAsync(RegisterRequestDto request);
        Task<(bool Success, string? Token, string? RefreshToken, string Message)> LoginAsync(LoginRequestDto dto);
        Task<(bool Success, string Message)> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto request);
    }
}
