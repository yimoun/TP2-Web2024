using ForumDiscussion.Data.Context;
using ForumDiscussion.Models;
using ForumDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ForumDiscussion.Models;
using static System.Collections.Specialized.BitVector32;
using Section = ForumDiscussion.Models.Section;
using System.Collections.Generic;

namespace ForumDiscussion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForumContext _forumContext;    

        public HomeController(ILogger<HomeController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }


        public IActionResult Index()
        {
            List<SectionListVM> sectionListVMs = new List<SectionListVM>();

            List<Section> sections = _forumContext.Section.ToList();

            if (sections == null || sections.Count == 0)
                return View(sectionListVMs); // Retourne une vue vide si aucune section n'est trouvée

            foreach (var section in sections)
            {
                // Obtenir tous les sujets de la section
                List<Sujet> sujets = _forumContext.Sujet.Where(s => s.SectionId == section.Id).ToList();
                int nbSujets = sujets.Count; // Nombre de sujets dans la section

                // Récupérer tous les messages de chaque sujet de la section
                List<MessageModel> messages = sujets
                    .SelectMany(su => _forumContext.Message.Where(m => m.SujetId == su.Id))
                    .ToList();

                int nbMessages = messages.Count; // Nombre total de messages dans la section

                // Trouver le dernier message par date de publication
                MessageModel dernierMessage = messages
                    .Where(m => m != null)
                    .OrderByDescending(m => m.DatePublication)
                    .FirstOrDefault();

                // Si un dernier message est trouvé, obtenir les informations de l'auteur
                if (dernierMessage != null)
                {
                    dernierMessage.Auteur = _forumContext.Membre
                        .FirstOrDefault(m => m.Id == dernierMessage.AuteurId);
                }

                sectionListVMs.Add(new SectionListVM(section, nbSujets, nbMessages, dernierMessage));
            }

            return View(sectionListVMs);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}