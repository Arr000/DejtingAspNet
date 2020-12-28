using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
   
        private UserManager<AppUser> userManager;

        public HomeController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Display(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return View(user);
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
