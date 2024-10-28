
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
            if (membre != null)
            {
                //On ne peut pas avoir deux membres avec le meme username
                Membre? existingMembre = _forumContext.Membre.Where(m => m.Username == membre.Username).FirstOrDefault();

                if (existingMembre != null)  //Je pense que le "remote" le fait déja !
                {
                    ModelState.AddModelError("Username", "Ce nom d'utilisateur existe déjà !");
                }
            }

            if (uploadfile != null && uploadfile.Length > 0)
            {
                string extension = Path.GetExtension(uploadfile.FileName).ToLower();
                string filename = string.Format("{0}{1}", Guid.NewGuid().ToString(), extension);

                string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Members", filename);

                using FileStream stream = System.IO.File.Create(pathToSave);
                uploadfile.CopyTo(stream);

                membre.Profil = filename;
            }


            if (!ModelState.IsValid)
            {

                return View("CreateEdit", membre);
            }

            membre.Role = Membre.ROLE_STANDARD; //Ne donner aucun choix pour le rôle : obligatoirement « Standard ».
            membre.MotDePasse = CryptographyHelper.HashPassword(membre.MotDePasse);

            _forumContext.Add(membre);
            _forumContext.SaveChanges();

            //Directement l'utilisateur est invité à se loguer avec son nouveau compte !
            return RedirectToAction("Login", "Auth", new { Area = "" });
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

            return View("SiteMessageVM", new SiteMessageVM("\"Id de membre intouvable ou incorrecte !"));

        }

        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                Membre? membre = _forumContext.Membre.Find(id);

                if (membre != null)
                {
                    return View("CreateEdit", membre);
                }
            }

            return View("SiteMessageVM", new SiteMessageVM("L'identifiant de ce membre est introuvable ou ce membre n'existe pas ."));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Membre membre, IFormFile uploadfile)
        {
            
                //Vérifie s'il n'existe pas un autre memebre avec le même username
                Membre? existingMembre = _forumContext.Membre.FirstOrDefault(m=> m.Username == membre.Username);
                if (existingMembre != null && existingMembre.Id != membre.Id)
                {
                    ModelState.AddModelError("Membre.Username", "Ce nom d'utilisateur existe déjà pour un autre utilisateur.");
                }

                if (!ModelState.IsValid)
                {

                    return View("CreateEdit", membre);
                }

                var originalMembre = _forumContext.Membre.Find(membre.Id);
                if(originalMembre != null)
                {
                    // Mettre à jour les champs nécessaires
                    originalMembre.Username = membre.Username;
                    originalMembre.Courriel = membre.Courriel;
                    originalMembre.Role = membre.Role;

                    // Mettre à jour le mot de passe uniquement si une nouvelle valeur est fournie
                    if (!string.IsNullOrWhiteSpace(membre.MotDePasse))
                    {
                        originalMembre.MotDePasse = CryptographyHelper.HashPassword(membre.MotDePasse);
                    }


                //Mettre le profil à jour !
                if (uploadfile != null && uploadfile.Length > 0)
                {
                    string extension = Path.GetExtension(uploadfile.FileName).ToLower();
                    string filename = string.Format("{0}{1}", Guid.NewGuid().ToString(), extension);

                    string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Members\\", filename);

                    using FileStream stream = System.IO.File.Create(pathToSave);
                    uploadfile.CopyTo(stream);

                    originalMembre.Profil = filename;
                }

                _forumContext.SaveChanges();

                    // Déconnexion de l'utilisateur
                    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


                    //Directement l'utilisateur est invité à se loguer avec son nouveau compte !
                    return RedirectToAction("Login", "Auth", new { Area = "" });
                }
                else
                {
                    return View("SiteMessageVM", new SiteMessageVM("\"Id de membre intouvable ou incorrecte !"));
                }
            }
    }
}

