using JwtIdentity.API.Models;
using JwtIdentity.API.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtIdentity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService _userService,UserManager<AppUser> _userManager) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto createUserDto)
        {
            var result = await _userService.RegisterAsync(createUserDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(TokenRequestModel model)
        {
            var succeed =  await _userService.Login(model);

            if (succeed)
            {
                return Ok("Kullanıcı Sisteme Giriş başarılı");
            }

            return BadRequest("Kullanıcı adı veya şifre hatalı");
           
        }

        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDto createRoleDto)
        {
            var result = await _userService.AddRoleAsync(createRoleDto);
            return Ok(result);
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetToken()
        {

            
            var currentUserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserID != null)
            {
                var user = await _userManager.FindByIdAsync(currentUserID);
                var result = await _userService.GetAccessToken(user);

                return Ok(result);
            }

            return NotFound("Giriş yapan kullanıcı bulunamadı");

            
            
           
        }


    }
}
