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
            membres = _forumContext.Membre.ToList();

            return View(membres);
        }

        public IActionResult Create()
        {

            Membre defautMember = new Membre(0, "", "", "", "", "", "");

            MembreCreateEditVM vm = new MembreCreateEditVM(defautMember, new List<string> { "Admin", "Standard"});

            return View("CreateEdit", vm);
        }

        [HttpPost]
        public IActionResult Create(Membre membre, IFormFile uploadfile)
        {
            if (membre != null && uploadfile != null && uploadfile.Length > 0)
            {
                //On ne peut pas avoir deux membres avec le meme username
                Membre? existingMembre = _forumContext.Membre.Where(m => m.Username == membre.Username).FirstOrDefault();

                if (existingMembre == null)
                {
                    string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\Members", uploadfile.FileName);

                    if (!System.IO.File.Exists(pathToSave))     //Si le fichier n'existe pas pour le path spécifié il est crée
                    {
                        using FileStream stream = System.IO.File.Create(pathToSave);
                        uploadfile.CopyTo(stream);
                    }
                    membre.Profil = uploadfile.FileName;

                    ModelStateEntry? imagePathModelState = ModelState["member.Profil"];
                    if (imagePathModelState != null)
                    {
                        imagePathModelState.ValidationState = ModelValidationState.Valid;
                    }
                }

                

                //if (ModelState.IsValid) //Si toutes les validations du modèle réussissent
                
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
        public IActionResult Edit(Membre membreChoice)
        {
            if (membreChoice != null)
            {
                // On ne peut pas avoir deux membres avec le meme username
                Membre? existingmembre = _forumContext.Membre.Where(m => m.Username == membreChoice.Username).FirstOrDefault();

                //On s'assure que dans la BD il n'existe pas déja un membre avec le meme username
                if (existingmembre != null && existingmembre.Id != membreChoice.Id)
                {
                    ModelState.AddModelError("Description", "Cette username existe déjà.");
                }

                _forumContext.Update(membreChoice);
                _forumContext.SaveChanges();
            }

            return RedirectToAction("List");
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
