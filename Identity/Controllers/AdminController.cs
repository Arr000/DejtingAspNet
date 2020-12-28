using Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash, IWebHostEnvironment webHostEnvironment)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                string stringFileName = UploadFile(user);
                AppUser appUser = new AppUser
                {
                    UserName = user.Name,
                    Email = user.Email,
                    Country = user.Country,
                    Age = user.Age,
                    Salary = user.Salary,
                    ProfileImage = stringFileName
                    
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        private string UploadFile(User user)
        {
            string fileName = null;

            if (user.ProfileImage != null)
            {
                string uploadDir = Path.Combine(webHostEnvironment.WebRootPath, "Images1");
                fileName = Guid.NewGuid().ToString() + "-" + user.ProfileImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    user.ProfileImage.CopyTo(fileStream);
                }


            }
            return fileName;
        }

        public async Task<IActionResult> Update(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password, int age, string country, string salary)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                user.Age = age;


                

                Country myCountry;
                Enum.TryParse(country, out myCountry);
                user.Country = myCountry;

                if (!string.IsNullOrEmpty(salary))
                    user.Salary = salary;
                else
                    ModelState.AddModelError("", "Salary cannot be empty");

                 if (email != null && password != null && !string.IsNullOrEmpty(salary))
                 {
                   IdentityResult result = await userManager.UpdateAsync(user);
                   if (result.Succeeded)
                   return RedirectToAction("Index");
                   else
                   Errors(result);
        }


                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }
    }
}
