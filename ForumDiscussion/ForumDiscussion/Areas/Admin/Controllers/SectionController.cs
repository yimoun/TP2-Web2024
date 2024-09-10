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
    public class SectionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForumContext _forumContext;

        public SectionController(ILogger<HomeController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }

        public IActionResult List()
        {
            List<Section> sections = new List<Section>();
            sections = _forumContext.Section.ToList();

            return View(sections);
        }

        /*C'est à ce niveau qu'on se rend premièrement avant d'aller dans la page d'édition/ajout*/
        public IActionResult Create()
        {

            Section defautSection = new Section("Titre de la section", "Description de la section");

            return View("CreateEdit", defautSection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Section section)
        {
            if(section != null)
            {
                //On ne peut pas avoir deux sextions avec le meme titre
                Section? existingsection = _forumContext.Section.Where(s => s.Titre == section.Titre).FirstOrDefault();

                if (existingsection != null)
                {
                    ModelState.AddModelError("Section.Titre", "Ce titre de section existe déja !");
                }

                if (!ModelState.IsValid)
                {
                    // Si le modèle n'est pas valide, on retourne à la vue CreateEdit où les messages seront affichés.
                    // Le ViewModèle reçu en POST n'est pas complet (seulement les info dans le <form> sont conservées),
                    // il faut donc réaffecter le choix de Section.

                    return View("CreateEdit", section);
                }

                _forumContext.Add(section); 
                _forumContext.SaveChanges();
            }

            return RedirectToAction("List");
        }

        public IActionResult Edit(int id)
        {
            if(id > 0)
            {
                Section? section = _forumContext.Section.Find(id);

                if (section != null)
                {
                    return View("CreateEdit", section);
                }
            }

            return View("AdminMessage", new AdminMessageVM("L'identifiant de ce choix de section est introuvable ou ce choix de section n'existe pas ."));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Section sectionChoice)
        {
            if (sectionChoice != null)
            {
                // On ne peut pas avoir deux sextions avec le meme titre
                Section? existingsection = _forumContext.Section.Where(s => s.Titre == sectionChoice.Titre).FirstOrDefault();

                //On s'assure que dans la BD il n'existe pas déja un choix de Menu avec la meme description
                if (existingsection != null && existingsection.Id != sectionChoice.Id)
                {
                    ModelState.AddModelError("Description", "Le titre de cette section existe déjà.");
                }

                if (!ModelState.IsValid)
                {
                    return View("CreateEdit", sectionChoice);
                }

                _forumContext.Update(sectionChoice);
                _forumContext.SaveChanges();
            }

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                Section? section = _forumContext.Section.Find(id);

                if (section != null)
                {
                    return View(section);
                }
            }

            return View("AdminMessage", new AdminMessageVM("L'identifiant de cette section est introuvable ou cette sectionn'existe pas ."));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        /*àprès avoir confirmer la suppression*/
        public IActionResult Delete(int id, IFormCollection collection)
        {
            if (id > 0)
            {
                _forumContext.Remove(id);
                _forumContext.SaveChanges();
            }

            return RedirectToAction("List");
        }

    }

       
    
}
