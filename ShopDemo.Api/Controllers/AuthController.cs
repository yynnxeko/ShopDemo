using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ShopDemo.Application.DTOs.AuthDtos;
using ShopDemo.Application.Interfaces.IServices;
using System.Security.Claims;

namespace ShopDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")] 
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterRequestDto registerRequest)
        {
            var (success, message) = await _authService.RegisterAsync(registerRequest);

            if (!success)
                return BadRequest(new { Message = message });

            return Ok(new { Message = message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequest)
        {
            var (success, token, refreshToken, message) = await _authService.LoginAsync(loginRequest);

            if(!success)
                return Unauthorized(new {Message = message});

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken, 
                Message = message
            });
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequest) 
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized("Không tìm thấy user");

            var result = await _authService.ChangePasswordAsync(userId, changePasswordRequest);
            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { result.Message });
        }
    }
}
