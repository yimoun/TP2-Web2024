using ForumDiscussion.Data.Context;
using Microsoft.AspNetCore.Mvc;
using ForumDiscussion.Areas.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Humanizer;
using static System.Collections.Specialized.BitVector32;
using Section = ForumDiscussion.Models.Section;
using ForumDiscussion.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ForumDiscussion.Helpers;
using System;

namespace ForumDiscussion.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = Membre.ROLE_ADMIN)]    À décomnter plus tard !
    public class MembreController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForumContext _forumContext;

        public MembreController(ILogger<HomeController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }

        public IActionResult List()
        {
            List<Membre> membres = new List<Membre>();
            membres = _forumContext.Membre.OrderBy(m => m.Username).ToList();  //Trier en ordre alphabétique la liste des membres.


            return View(membres);
        }

        public IActionResult Create()
        {

            Membre defautMember = new Membre();

            MembreCreateEditVM vm = new MembreCreateEditVM(defautMember, new List<string> { "Admin", "Standard"});

            return View("CreateEdit", vm);
        }

        [HttpPost]
        public IActionResult Create(Membre membre)
        {
            if (membre != null)
            {
                //On ne peut pas avoir deux membres avec le meme username
                Membre? existingMembre = _forumContext.Membre.Where(m => m.Username == membre.Username).FirstOrDefault();

                if(existingMembre != null)
                {
                    ModelState.AddModelError("Username", "Ce nom d'utilisateur existe déjà !");
                }

                if (!ModelState.IsValid)
                {
                    return View("CreateEdit", new MembreCreateEditVM(membre, new List<string> { "Admin", "Standard" }));
                }

                membre.MotDePasse = CryptographyHelper.HashPassword(membre.MotDePasse);

                _forumContext.Add(membre);
                _forumContext.SaveChanges();

                return RedirectToAction("List", "Membre");
                
            }

            return View("CreateEdit", new MembreCreateEditVM(new Membre(), new List<string> { "Admin", "Standard" }));
        }

        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                Membre? membre = _forumContext.Membre.Find(id);

                if (membre != null)
                {
                    return View("CreateEdit", new MembreCreateEditVM(membre, new List<string> { "Admin", "Standard" }));
                }
            }

            return View("AdminMessage", new AdminMessageVM("L'identifiant de ce membre est introuvable ou ce membre n'existe pas ."));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MembreCreateEditVM membreChoiceVM)
        {
            if (membreChoiceVM != null && membreChoiceVM.Membre != null)
            {
                // Vérifie s'il existe un autre membre avec le même username
                var existingMembre = _forumContext.Membre
                    .FirstOrDefault(m => m.Username == membreChoiceVM.Membre.Username);

                if (existingMembre != null && existingMembre.Id != membreChoiceVM.Membre.Id)
                {
                    ModelState.AddModelError("Membre.Username", "Ce nom d'utilisateur existe déjà pour un autre utilisateur.");
                }

                var originalMembre = _forumContext.Membre.Find(membreChoiceVM.Membre.Id);
                if (originalMembre != null)
                {
                    // Mettre à jour les champs nécessaires
                    originalMembre.Username = membreChoiceVM.Membre.Username;
                    originalMembre.Courriel = membreChoiceVM.Membre.Courriel;
                    originalMembre.Role = membreChoiceVM.Membre.Role;

                    // Mettre à jour le mot de passe uniquement si une nouvelle valeur est fournie
                    if (!string.IsNullOrWhiteSpace(membreChoiceVM.Membre.MotDePasse))
                    {
                        originalMembre.MotDePasse = CryptographyHelper.HashPassword(membreChoiceVM.Membre.MotDePasse);
                    }

                    _forumContext.SaveChanges();
                }

                return RedirectToAction("List");
            }

            return View("AdminMessage", new AdminMessageVM("Une erreur est survenue lors de la mise à jour du membre."));
        }

        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                Membre? membre = _forumContext.Membre.Find(id);

                if (membre != null)
                {
                    return View(membre);
                }
            }

            return View("AdminMessage", new AdminMessageVM("L'identifiant de ce membre est introuvable ou ce membre'existe pas ."));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            Membre? membre = _forumContext.Membre.Find(id);

            if (membre == null || id < 0)
            {
                return View("AdminMessage", new AdminMessageVM("L'identifiant de ce membre est introuvable ou ce membre'existe pas ."));
            }

            try
            {
                _forumContext.Membre.Remove(membre);
                _forumContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return View("AdminMessage", new AdminMessageVM(ex.Message));
            }

            return RedirectToAction("List");
        }
    }
}
