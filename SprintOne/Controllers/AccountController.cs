using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SprintOne.Models;
using SprintOne.Models.ViewModels;
using System.Security.Claims;
using SprintOne.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace SprintOne.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private readonly MatchContext _context;

        public AccountController(UserManager<User> userMngr,
           SignInManager<User> signInMngr, MatchContext context)
        {
            userManager = userMngr;
            signInManager = signInMngr;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new SprintOne.Models.User
                {
                    UserName = model.Username
                };

                using (var registrationTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var result = await userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            var profile = new SprintOne.Models.Profile
                            {
                                ProfileID = user.UserID,
                                FirstName = model.Firstname,
                                LastName = model.Lastname,
                                CreationDate = DateTime.Now,
                                UserID = user.UserID
                            };
                            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Profiles ON");
                            _context.Add(profile);
                            _context.SaveChanges();
                            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Profiles OFF");
                            registrationTransaction.Commit();
                            await signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Success", "Home");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        registrationTransaction.Rollback();
                        ModelState.AddModelError("", "Unable to register, please contact your administrator for more details.");
                        return View(model);
                    } 
                }
            }
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult LogIn(string returnURL = "")
        {
            var model = new LoginViewModel { ReturnUrl = returnURL };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Username, model.Password, isPersistent: model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) &&
                        Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid username/password.");
            return View(model);
        }

        public ViewResult AccessDenied()
        {
            return View();
        }
    }
}
