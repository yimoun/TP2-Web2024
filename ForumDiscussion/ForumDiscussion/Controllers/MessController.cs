using ForumDiscussion.Data.Context;
using ForumDiscussion.Models;
using ForumDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace ForumDiscussion.Controllers
{
    public class MessController : Controller
    {
        private readonly ILogger<MessController> _logger;
        private readonly ForumContext _forumContext;
        public MessController(ILogger<MessController> logger, ForumContext forumContext)
        {
            _logger = logger;
            _forumContext = forumContext;
        }
        public IActionResult List(int idSujet)
        {
            List<MessageModel> messages = _forumContext.Message.Where(x => x.SujetId == idSujet).ToList();
            messListVM vm = new messListVM(messages, idSujet);
            return View(vm);
        }

        public IActionResult Create()
        {
            MessageModel message = new MessageModel();

            return View("CreateEdit", message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MessageModel message)
        {
            if (message != null)
            {
                _forumContext.Add(message);
                _forumContext.SaveChanges();
                
                return RedirectToAction("List");
            }
            return View("CreateEdit", message);
        }
    }
}
