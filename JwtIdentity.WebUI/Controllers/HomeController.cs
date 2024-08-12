using JwtIdentity.WebUI.Models;
using JwtIdentity.WebUI.Services.ProtectedService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JwtIdentity.WebUI.Controllers
{

    public class HomeController : Controller
    {
       
        private readonly IProtectedService _protectedService;

        public HomeController( IProtectedService protectedService)
        {
           
            _protectedService = protectedService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _protectedService.GetInfo();
            if (result == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.values = result;
            return View();
        }

       
    }
}
