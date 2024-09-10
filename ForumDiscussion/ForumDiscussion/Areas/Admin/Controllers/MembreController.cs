using ForumDiscussion.Data.Context;
using Microsoft.AspNetCore.Mvc;
using ForumDiscussion.Areas.Admin.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Humanizer;
using static System.Collections.Specialized.BitVector32;
using Section = ForumDiscussion.Models.Section;
using ForumDiscussion.Models;

namespace ForumDiscussion.Areas.Admin.Controllers
{
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

            Membre defautMember = new Membre();

            return View("CreateEdit", defautMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Membre membre)
        {
            if (membre != null)
            {
                //On ne peut pas avoir deux membres avec le meme couriel
                Membre? existingMembre = _forumContext.Membre.Where(m => m.Courriel == membre.Courriel).FirstOrDefault();

                if (existingMembre != null)
                {
                    ModelState.AddModelError("Membre.Titre", "Ce courriel de membre existe déja !");
                }

                if (!ModelState.IsValid)
                {
                    // Si le modèle n'est pas valide, on retourne à la vue CreateEdit où les messages seront affichés.
                    // Le ViewModèle reçu en POST n'est pas complet (seulement les info dans le <form> sont conservées),
                    // il faut donc réaffecter le choix de Membre.

                    return View("CreateEdit", membre);
                }

                _forumContext.Add(membre);
                _forumContext.SaveChanges();
            }

            return RedirectToAction("List");
        }
    }
}
