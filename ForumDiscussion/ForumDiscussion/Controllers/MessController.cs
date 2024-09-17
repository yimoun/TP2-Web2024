using ForumDiscussion.Data.Context;
using ForumDiscussion.Models;
using ForumDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;

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

            messListVM vm = new messListVM(messages);
            return View(vm);
        }

        public IActionResult Post(MessageModel reponse)
        {
            _forumContext.Message.Add(reponse);
            _forumContext.SaveChanges();

            return CreatedAtAction("List", new {id = reponse.Id}, reponse);
        }
    }
}
