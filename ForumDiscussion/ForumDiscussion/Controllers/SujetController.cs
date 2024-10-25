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
            List<SujetListVM> sujetListVMs = new List<SujetListVM>();
            List<Sujet> sujets = _forumContext.Sujet.Where(ss => ss.Id == idSection).ToList();
            List<MessageModel> messages = new List<MessageModel>();
            for (int i = 0; i < sujets.Count; i++)
            {
                MessageModel dernierMessage = new MessageModel();
                
                for (int j = 0; j < sujets.Count; j++)
                {
                    if (sujets[j].Messages != null)
                    {
                        dernierMessage = sujets[j].Messages.OrderByDescending(dm => dm.DatePublication).First();
                        nbReponses = sujets[j].Messages.OrderBy(dm => dm.DatePublication).Skip(1).Count();
                    }
                    sujetListVMs.Add(new SujetListVM(sujets[j], nbReponses, dernierMessage));
                }
            }
            return View(sujetListVMs);
        }
    }
}
