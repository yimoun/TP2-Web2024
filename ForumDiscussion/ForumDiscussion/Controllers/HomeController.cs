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
            int scount = 0;
            List<Section> sections = _forumContext.Section.ToList();

            List<Sujet> sujets = new List<Sujet>();

           

            foreach (Section section in sections)
            {
                scount = section.Sujets.Count();
            }
            SectionListVM vm = new SectionListVM(sections, scount);

            return  View(vm);
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