using ForumDiscussion.Data.Context;
using ForumDiscussion.Models;
using ForumDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ForumDiscussion.Models;
using static System.Collections.Specialized.BitVector32;
using Section = ForumDiscussion.Models.Section;

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
            List<MessageModel> messages = new List<MessageModel>();


            for (int i = 0; i < sections.Count; i++)
            {
                List<Sujet> sujets = _forumContext.Sujet.Where(x => x.SectionId == sections[i].Id).ToList();
                int nbSujets = sujets.Count;
                int nbMessages = 0;

                foreach (Sujet sujet in sujets)
                {
                    messages.Add(_forumContext.Sujet.)
                }
                MessageModel dernierMessage = new MessageModel();
                //for (int j = 0; j < sujets.Count; j++)
                //{
                //    List<MessageModel> messages = _forumContext.Message.Where(y => y.SujetId == sujets[j].Id).ToList();
                //}

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