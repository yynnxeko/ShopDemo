using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ShopDemo.Application.DTOs.AuthDtos;
using ShopDemo.Application.Interfaces.IServices;
using ShopDemo.Application.Services;
using System.Security.Claims;

namespace ShopDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOTPService _otpService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        public AuthController(IAuthService authService, IOTPService otpService, IEmailService emailService, IUserService userService, IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _otpService = otpService;
            _emailService = emailService;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("User không tồn tại");

            var otp = _otpService.GenerateOtp();
            await _otpService.StoreOtpAsync(request.Email, otp);
            await _emailService.SendOtpEmailAsync(request.Email, otp);

            return Ok("OTP đã được gửi đến email của bạn");
        }

        [HttpPost("reset-password-otp")]
        public async Task<IActionResult> ResetPasswordWithOtp([FromBody] ResetPasswordWithOtpDto request)
        {
            if (!await _otpService.VerifyOtpAsync(request.Email, request.Otp))
                return BadRequest("OTP không đúng hoặc hết hạn");

            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("User không tồn tại");

            await _userService.UpdatePasswordAsync(user.Id, request.NewPassword);
            await _refreshTokenService.RevokeAllRefreshTokensAsync(user.Id);

            return Ok("Reset mật khẩu thành công");
        }
    }
}
