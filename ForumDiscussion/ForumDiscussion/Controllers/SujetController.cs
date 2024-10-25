using ForumDiscussion.Data.Context;
using ForumDiscussion.Models;
using ForumDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace ForumDiscussion.Controllers
{
    public class SujetController : Controller
    {
        private readonly ILogger<SujetController> _logger;
        private readonly ForumContext _forumContext;

        public SujetController(ILogger<SujetController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }
        public IActionResult List(int idSection)
        {
            int nbReponses = 0;
            List<Membre> membres = _forumContext.Membre.ToList();
            List<SujetListVM> sujetListVMs = new List<SujetListVM>();
            List<Sujet> sujets = _forumContext.Sujet.Where(ss => ss.Id == idSection).ToList();
            List<MessageModel> messages = new List<MessageModel>();
            for (int i = 0; i < sujets.Count; i++)
            {
                MessageModel dernierMessage = new MessageModel();
                
                messages = _forumContext.Message.Where(m => m.SujetId == sujets[i].Id).ToList();
                dernierMessage = messages.OrderByDescending(dm => dm.DatePublication).First();
                nbReponses = messages.OrderBy(dm => dm.DatePublication).Skip(1).Count();
                dernierMessage.Auteur = _forumContext.Membre.Where(m => m.Id == dernierMessage.AuteurId).FirstOrDefault();
                messages.Clear();
                sujetListVMs.Add(new SujetListVM(sujets[i], nbReponses, dernierMessage));
            }
            return View(sujetListVMs);
        }
    }
}
