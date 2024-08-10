using JwtIdentity.WebUI.Models;
using JwtIdentity.WebUI.Services.ProtectedService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JwtIdentity.WebUI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class HomeController : Controller
    {
       
        private readonly IProtectedService _protectedService;

        public HomeController( IProtectedService protectedService)
        {
           
            _protectedService = protectedService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _protectedService.GetInfo();
            ViewBag.value = values;
            return View();
        }

       
    }
}
