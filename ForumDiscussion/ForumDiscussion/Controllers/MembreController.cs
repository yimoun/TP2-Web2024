
using ForumDiscussion.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ForumDiscussion.Models;
using ForumDiscussion.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ForumDiscussion.ViewModels;
using System.Security.Claims;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System;
using ForumDiscussion.Data.Context;
using Humanizer.Localisation;

namespace ForumDiscussion.Controllers
{
    public class MembreController : Controller
    {
        private readonly ILogger<MembreController> _logger;
        private readonly ForumContext _forumContext;

        public MembreController(ILogger<MembreController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }

        public IActionResult Create()
        {
            Membre membre = new Membre();

            return View("CreateEdit", membre);
        }

        [HttpPost]
        public IActionResult Create(Membre? membre, IFormFile uploadfile)
        {
            if (membre != null && uploadfile != null && uploadfile.Length > 0)
            {
                //On ne peut pas avoir deux membres avec le meme username
                Membre? existingMembre = _forumContext.Membre.Where(m => m.Username == membre.Username).FirstOrDefault();

                if (existingMembre != null)  //Je pense que le "remote" le fait déja !
                {
                    return View("SiteMessage", new SiteMessageVM
                    {
                        Message = "Ce nom d'utilisateur est déja existant !"
                    });
                }


                string extension = Path.GetExtension(uploadfile.FileName).ToLower();
                string filename = string.Format("{0}{1}", Guid.NewGuid().ToString(), extension);
                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Members", filename);

                using FileStream stream = System.IO.File.Create(pathToSave);
                uploadfile.CopyTo(stream);
                if (filename != null) {membre.Profil = filename; }
                else
                {
                    return View("SiteMessage", new SiteMessageVM
                    {
                        Message = "L'image de profil est inexistant!"
                    });
                }
                

                ModelStateEntry? imagePathModelState = ModelState["Profil"];
                if (imagePathModelState != null)
                {
                    imagePathModelState.ValidationState = ModelValidationState.Valid;
                }
            }

            if (ModelState.IsValid)
            {
                membre.Role = Membre.ROLE_STANDARD; //Ne donner aucun choix pour le rôle : obligatoirement « Standard ».

                membre.MotDePasse = CryptographyHelper.HashPassword(membre.MotDePasse);

                _forumContext.Add(membre);
                _forumContext.SaveChanges();

                //Directement l'utilisateur est invité à se loguer avec son nouveau compte !
                return RedirectToAction("Login", "Auth", new { Area = "" });
            }
            return View("CreateEdit", membre);
        }


        [AuthorizeLoggedIn] //Filtre uniquement pour les utilisateurs connectés
        public IActionResult Filter()
        {
            string userId = User.FindFirst(ClaimTypes.Sid)?.Value;

            int id = 0;

            if (int.TryParse(userId, out id))
            {

                Membre? existingMembre = _forumContext.Membre.Find(id);

                if (existingMembre != null)
                {
                    return View("CreateEdit", existingMembre);
                }
            }

            return View("SiteMessage", new SiteMessageVM
            {
                Message = "Id de membre intouvable ou incorrecte !"
            });


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Membre membre, IFormFile uploadfile)
        {
            if (membre != null)
            {
                Membre? existingMembre = _forumContext.Membre.Find(membre.Id);
                if (existingMembre != null && ModelState.IsValid)
                {
                    // Déconnexion de l'utilisateur
                    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


                    //Le membre change de mot de passe: comme modification de profil
                    membre.MotDePasse = CryptographyHelper.HashPassword(membre.MotDePasse);

                    _forumContext.SaveChanges();

                    //Directement l'utilisateur est invité à se loguer avec son nouveau compte !
                    return RedirectToAction("Login", "Auth", new { Area = "" });
                }
                else
                {
                    return View("SiteMessage", new SiteMessageVM
                    {
                        Message = "Id de l'utilisateur introuvabkle ou inexistant"
                    });
                }
            }

            return RedirectToAction("List");
        }
    }
}

