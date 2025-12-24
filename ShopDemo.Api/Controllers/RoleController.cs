using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopDemo.Application.DTOs.RoleDtos;
using ShopDemo.Application.Interfaces.IServices;
using ShopDemo.Core.Entities;

namespace ShopDemo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleCreatedDto role)
        {
            try
            {
                await _roleService.AddRoleAsync(role);
                return Ok(new { message = "Create role succcessfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleByIdAsync(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync() {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoleAsync(int id,RoleUpdatedDto role) { 
            var result = await _roleService.UpdateRoleAsync(id, role);
            return Ok(result);  
        } 
    }
}
