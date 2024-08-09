using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtIdentity.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]

    public class ProtectedController : ControllerBase
    {

       
        [HttpGet]
        public IActionResult GetData()
        {
            return Ok("Sadece yetkiniz varsa burayı görebilirsiniz.");
        }
    }
}
