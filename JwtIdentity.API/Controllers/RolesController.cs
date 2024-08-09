using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtIdentity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(RoleManager<IdentityRole> roleManager) : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var role = new IdentityRole
            {
                Name = roleName,
            };


            var result = await roleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                return Ok($"{roleName} rolü oluşturuldu");
            }

            return BadRequest("Bir hata oluştu");

        }
    }
}
