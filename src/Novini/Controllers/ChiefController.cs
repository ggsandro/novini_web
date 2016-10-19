using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Novini.Repository;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Novini.Controllers
{
    public class ChiefController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var repo = new UserRepository();
            if (repo.CheckLoginPassword(login, password))
            {
                var claims = new Claim[] { new Claim(ClaimTypes.Name, "Admin"), new Claim(ClaimTypes.Role, "Administrator") };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Basic"));
                await HttpContext.Authentication.SignInAsync("CustomLogin", principal);
                return LocalRedirect("/");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CustomLogin");
            return LocalRedirect("/");
        }
    }
}
