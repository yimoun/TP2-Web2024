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


        public  IActionResult Index()
        {
            List<SectionListVM> sectionListVMs = new List<SectionListVM>();

            List<Section> sections = _forumContext.Section.ToList();
            Dictionary<int, List<MessageModel>> messagesParSujets = new Dictionary<int, List<MessageModel>>();
            List<MessageModel> messages = new List<MessageModel>();
            List<MessageModel> derniersMessages = new List<MessageModel>();
            List<Membre> membres = _forumContext.Membre.ToList();

            for (int i = 0; i < sections.Count; i++)
            {
                
                List<Sujet> sujets = _forumContext.Sujet.Where(x => x.SectionId == sections[i].Id).ToList();
                int nbSujets = sujets.Count;
                MessageModel dernierMessagePossible = new MessageModel();
                MessageModel dernierMessage = new MessageModel();
                for (int j = 0; j < nbSujets; j++)
                {
                    messages = _forumContext.Message.Where(m => m.SujetId == sujets[j].Id).ToList();
                    messagesParSujets.Add(j, messages);
                }
                int nbMessages = messagesParSujets.Count;
                for (int k = 0; k < nbMessages; k++)
                {
                    dernierMessagePossible = messagesParSujets[k].OrderByDescending(dm => dm.DatePublication).First();
                    derniersMessages.Add(dernierMessagePossible);
                }
                for (int L = 0;L < derniersMessages.Count; L++)
                {
                    dernierMessage = derniersMessages.OrderByDescending(dm => dm.DatePublication).First();
                }
                dernierMessage.Auteur = _forumContext.Membre.Where(m => m.Id == dernierMessage.AuteurId).FirstOrDefault();
                messagesParSujets.Clear();
                derniersMessages.Clear();
                sectionListVMs.Add(new SectionListVM(sections[i], nbSujets, nbMessages, dernierMessage));
            }

            
             return  View(sectionListVMs);
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