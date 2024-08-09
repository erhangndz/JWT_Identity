using JwtIdentity.API.Models;
using JwtIdentity.API.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace JwtIdentity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService _userService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            var result = await _userService.RegisterAsync(createUserDto);
            return Ok(result);
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken(TokenRequestModel model)
        {
            var result = await _userService.GetTokenAsync(model);
            return Ok(result);
        }

        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDto createRoleDto)
        {
            var result = await _userService.AddRoleAsync(createRoleDto);
            return Ok(result);
        }
    }
}
