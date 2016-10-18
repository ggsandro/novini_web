using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Novini.Repository;

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
                await HttpContext.Authentication.SignInAsync("CustomLogin", HttpContext.User);
                return LocalRedirect("/");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("CustomLogin");
            return LocalRedirect("/");
        }

        public IActionResult TakeAll()
        {
            return View();
        }

        public IActionResult UpdateAll()
        {
            return View();
        }
    }
}
