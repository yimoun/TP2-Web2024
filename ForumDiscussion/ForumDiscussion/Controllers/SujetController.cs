using ForumDiscussion.Data.Context;
using ForumDiscussion.Models;
using ForumDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult List()
        {
            List<Sujet> sujets = _forumContext.Sujet.ToList();
            SujetListVM vm = new SujetListVM(sujets);

            return View(vm);
        }
    }
}
