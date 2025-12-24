using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopDemo.Application.DTOs.RefreshTokenDtos;
using ShopDemo.Application.Interfaces.IServices;
using System.Security.Claims;

namespace ShopDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetAllTokenByMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Không tìm thấy user");
            }
            var listToken = await _refreshTokenService.GetAllByUserIdAsync(userId);
            return Ok(listToken);
        }

        [HttpGet("id/all")]
        public async Task<IActionResult> GetAllTokenByUserId(Guid userId) 
        {
            var listToken = await _refreshTokenService.GetAllByUserIdAsync(userId);
            return Ok(listToken);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto request)
        {
            var (success, response, message) = await _refreshTokenService.RefreshAsync(request);

            if (!success)
                return Unauthorized(new { Message = message });

            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Không tìm thấy user");
            }

            await _refreshTokenService.RevokeAllRefreshTokensAsync(userId);

            return Ok(new { Message = "Logout thành công" });
        }
    }
}
