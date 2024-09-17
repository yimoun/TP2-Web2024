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

            List<Sujet> sujets = _forumContext.Sujet.Where(x => x.SectionId == idSection).ToList();

            SujetListVM vm = new SujetListVM(sujets);

            return View(vm);
        }
    }
}
