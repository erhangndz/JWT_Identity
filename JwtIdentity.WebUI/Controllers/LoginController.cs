using JwtIdentity.WebUI.Models;
using JwtIdentity.WebUI.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtIdentity.WebUI.Controllers
{
    [AllowAnonymous]
    public class LoginController(IUserService _userService) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            await _userService.Login(loginDto);
            return RedirectToAction("Index","Home");
        }
    }
}
