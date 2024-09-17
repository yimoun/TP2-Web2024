using Mastermind.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ForumDiscussion.Models;
using System.Security.Claims;
using ForumDiscussion.Helpers;
using System;
using ForumDiscussion.Data.Context;

namespace Mastermind.Controllers
{
    public class AuthController : Controller
    {
        private readonly ForumContext _forumContext;

        public AuthController(ForumContext forumContext)
        {
            _forumContext = forumContext;
        }

        public IActionResult Login()
        {
            AuthLoginVM viewModel = new();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Login(AuthLoginVM viewModel, string? returnurl)
        {
            if (ModelState.IsValid)
            {
                //Membre? user = new DAL().MemberFact.GetByUsername(viewModel.Username);
                Membre? existingMembre = _forumContext.Membre.Where(m => m.Courriel == viewModel.Username).FirstOrDefault();

                if (existingMembre != null)
                {
                    //bool valid = viewModel.Password == user.Password;
                    bool valid = CryptographyHelper.ValidateHashedPassword(viewModel.Password, existingMembre.MotDePasse);

                    if (valid)
                    {
                        var identity = new ClaimsIdentity(new[] {
                                new Claim(ClaimTypes.Sid, existingMembre.Id.ToString()),
                                new Claim(ClaimTypes.Name, existingMembre.Username),
                                new Claim(ClaimTypes.Email, existingMembre.Courriel),
                                new Claim(ClaimTypes.Role, existingMembre.Role)
                            }, CookieAuthenticationDefaults.AuthenticationScheme);

                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                        if (!string.IsNullOrWhiteSpace(returnurl) && Url.IsLocalUrl(returnurl))
                        {
                            if (existingMembre.Role == Membre.ROLE_STANDARD && returnurl.ToLower().StartsWith("/admin"))
                            {
                                return RedirectToAction("Index", "Home", new { Area = "" });
                            }

                            return LocalRedirect(returnurl);
                        }
                        else if (existingMembre.Role == Membre.ROLE_ADMIN)
                        {
                            return RedirectToAction("Index", "Home", new { Area = "Admin" });
                        }

                        return RedirectToAction("Index", "Home", new { Area = "" });
                    }
                }

                ModelState.AddModelError("Password", "Username ou Mot de passe invalide");
            }

            return View(viewModel);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}
