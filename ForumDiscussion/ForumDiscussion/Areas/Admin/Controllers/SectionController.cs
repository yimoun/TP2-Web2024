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
    [Area("Admin")]
    public class SectionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForumContext _forumContext;

        public SectionController(ILogger<HomeController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }

        public IActionResult List(string sortOrder, string filter)
        {
            // Conserve le tri et le filtre dans ViewData pour réutilisation dans la vue
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = filter;

            var sections = _forumContext.Section.AsQueryable();

            //Appliquer le filtre si séclectionné
            if (!string.IsNullOrEmpty(filter))
            {
                sections = sections.Where(s => s.Titre.Contains(filter));
            }

            //Appliquer le tri en fonction du paramètre
            sections = sortOrder switch
            {
                "ordre_asc" => sections.OrderBy(s => s.Titre),
                "ordre_desc" => sections.OrderByDescending(s => s.Titre),
                _ => sections.OrderBy(s => s.Titre) // Tri par défaut
            };



            return View(sections.ToList());
        }

        public IActionResult Create()
        {

            Section defautSection = new();

            return View("CreateEdit", defautSection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Section section)
        {
            if(section != null)
            {
                // Vérification pour éviter les doublons de titre
                Section? existingSection = _forumContext.Section.FirstOrDefault(s => s.Titre == section.Titre);

                if (existingSection != null)
                {
                    ModelState.AddModelError("Titre", "Ce titre de section existe déjà !");
                }

                if (!ModelState.IsValid)
                {
                    return View("CreateEdit", section);
                }

                // Ajout de la nouvelle section à la base de données
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
                Section? existingSection = _forumContext.Section.FirstOrDefault(s => s.Titre == sectionChoice.Titre && s.Id != sectionChoice.Id);

                if (existingSection != null)
                {
                    ModelState.AddModelError("Titre", "Le titre de cette section existe déjà.");
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

            return View("AdminMessage", new AdminMessageVM("L'identifiant de cette section est introuvable ou cette section n'existe pas ."));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            Section? section = _forumContext.Section.Find(id);
            
            if(section == null || id < 0)
            {
                return View("AdminMessage", new AdminMessageVM("L'identifiant de cette section est introuvable ou invalide ou cette sectionn'existe pas ."));
            }

            try
            {
                _forumContext.Section.Remove(section);
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
