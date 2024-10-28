using ForumDiscussion.Data.Context;
using ForumDiscussion.Models;
using ForumDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;

namespace ForumDiscussion.Controllers
{
    public class SujetController : Controller
    {
        private readonly ILogger<SujetController> _logger;
        private readonly ForumContext _forumContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SujetController(ILogger<SujetController> logger, ForumContext forumContext, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _forumContext = forumContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult List(int idSection)
        {
            int nbReponses = 0;
            List<Membre> membres = _forumContext.Membre.ToList();
            List<SujetListVM> sujetListVMs = new List<SujetListVM>();
            List<Sujet> sujets = _forumContext.Sujet.Where(suj => suj.SectionId == idSection).ToList();
            List<MessageModel> messages = new List<MessageModel>();


            for (int i = 0; i < sujets.Count; i++)
            {
                // Récupérer tous les messages pour le sujet actuel
                messages = _forumContext.Message.Where(m => m.SujetId == sujets[i].Id).ToList();

                // Récupérer le dernier message ou `null` si aucun message n'existe
                MessageModel dernierMessage = messages
                    .OrderByDescending(dm => dm.DatePublication)
                    .FirstOrDefault();

                // Si des messages existent, calculez le nombre de réponses et récupérez l'auteur
                if (dernierMessage != null)
                {
                    nbReponses = messages.Skip(1).Count(); // Nombre de réponses (tous les messages sauf le premier)
                    dernierMessage.Auteur = _forumContext.Membre
                        .FirstOrDefault(m => m.Id == dernierMessage.AuteurId);
                }
                else
                {
                    nbReponses = 0;
                    dernierMessage = new MessageModel();
                }

                sujetListVMs.Add(new SujetListVM(sujets[i], nbReponses, dernierMessage));
            }

            return View(new SujetListSectionVM(sujetListVMs, idSection));
        }

      

        [HttpGet]
        public IActionResult Create(int idSection)
        {
            return View("CreateEdit", new SujetCreateEditVM(new Sujet(), new MessageModel(), idSection));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SujetCreateEditVM sujetVM)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"Erreur pour {modelStateKey}: {error.ErrorMessage}");
                    }
                }

                return View("CreateEdit", sujetVM);
            }


            if (ModelState.IsValid)
            {
                var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value);
                var auteur = _forumContext.Membre.Find(userId);

                if (auteur == null)
                {
                    return View("SiteMessageVM", new SiteMessageVM("auteur associé à ce sujet intouvable ou incorrecte !"));
                }

                sujetVM.sujet.Auteur = auteur.Pseudo;
                sujetVM.sujet.DateCreation = DateTime.Now;
                sujetVM.sujet.MembreId = userId;  
                sujetVM.sujet.SectionId = sujetVM.IdSection;

                // Création du premier message pour ce sujet
                sujetVM.messageSujet.AuteurId = userId;
                sujetVM.messageSujet.DatePublication = DateTime.Now;
                sujetVM.messageSujet.Sujet = sujetVM.sujet;

                _forumContext.Sujet.Add(sujetVM.sujet);
                _forumContext.Message.Add(sujetVM.messageSujet);
                _forumContext.SaveChanges();

                return RedirectToAction("List", new { idSection = sujetVM.sujet.SectionId });
            }

            return View("CreateEdit", sujetVM);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var sujet = _forumContext.Sujet.Include(s => s.Membre).FirstOrDefault(s => s.Id == id);
            if (sujet == null)
            {
                return View("SiteMessageVM", new SiteMessageVM("sujet intouvable ou incorrecte !"));
            }

            // Récupération du premier message associé au sujet
            var premierMessage = _forumContext.Message.FirstOrDefault(m => m.SujetId == id);

            var sujetVM = new SujetCreateEditVM(sujet, premierMessage ?? new MessageModel());

            return View("CreateEdit", sujetVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SujetCreateEditVM sujetVM)
        {
            if (ModelState.IsValid)
            {
                var sujetOriginal = _forumContext.Sujet.Find(sujetVM.sujet.Id);

                if (sujetOriginal == null)
                {
                    return View("SiteMessageVM", new SiteMessageVM("sujet intouvable ou incorrecte !"));
                }

                sujetOriginal.Titre = sujetVM.sujet.Titre;
                MessageModel messageAssocie = _forumContext.Message.FirstOrDefault(m => m.SujetId == sujetOriginal.Id);

                messageAssocie.Contenu = sujetVM.messageSujet.Contenu;

                _forumContext.SaveChanges();

                return RedirectToAction("List", new { idSection = sujetOriginal.SectionId });
            }

            return View("CreateEdit", sujetVM);
        }

        // Affiche la page de confirmation de suppression
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value);

            var sujet = _forumContext.Sujet
                .FirstOrDefault(s => s.Id == id && s.MembreId == userId);

            if (sujet == null)
            {
                return View("SiteMessageVM", new SiteMessageVM("Vous n'êtes pas autorisé à supprimer ce sujet."));
            }

            return View(sujet); // Renvoie le sujet à la vue pour confirmation
        }

        // Supprime le sujet après confirmation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value);

            var sujet = _forumContext.Sujet
                .FirstOrDefault(s => s.Id == id && s.MembreId == userId);

            if (sujet == null)
            {
                return View("SiteMessageVM", new SiteMessageVM("Vous n'êtes pas autorisé à supprimer ce sujet."));
            }

            _forumContext.Sujet.Remove(sujet);
            _forumContext.SaveChanges();

            return RedirectToAction("List", new { idSection = sujet.SectionId });
        }

    }

}

