using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using ShopDemo.Application.DTOs.UserDtos;
using ShopDemo.Application.Interfaces.IServices;

namespace ShopDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        { 
            var users = await _userService.GetAllUserAsync();
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UserUpdatedDto dto)
        {
            var updateUser = await _userService.UpdateAsync(id, dto);
            return Ok(updateUser);
        }
    }
}
