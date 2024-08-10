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
           var succeed =  await _userService.Login(loginDto);
            if (succeed)
            {

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            return View(loginDto);
           
        }
    }
}
