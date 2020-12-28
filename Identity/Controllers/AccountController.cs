using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
      
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private AppIdentityDbContext _appIdentityDb;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr, AppIdentityDbContext appIdentityDb)
        {
            _userManager = userMgr;
            _signInManager = signinMgr;
            _appIdentityDb = appIdentityDb;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            
            //ead from db 
            
            login.ExistingUsers = _appIdentityDb.Users.Select(u => u.Email);
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(login.Email);
                if (appUser != null)
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, login.Password, false, false);
                    if (result.Succeeded)
                        return Redirect(login.ReturnUrl ?? "/");
                }
                ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
            }
            return View(login);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string SearchKey)
        {
            var users = _appIdentityDb.Users.Where(u => u.UserName.Contains(SearchKey));
            return View(new SearchModel() { Users = users });
        }

    }
    public class SearchModel
    {
        public string SearchKey { get; set; }
        public IEnumerable<AppUser> Users { get; set; }

    }
}
