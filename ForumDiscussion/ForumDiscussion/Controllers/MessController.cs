using ForumDiscussion.Data.Context;
using ForumDiscussion.Models;
using ForumDiscussion.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ForumDiscussion.Controllers
{
    public class MessController : Controller
    {
        private readonly ILogger<MessController> _logger;
        private readonly ForumContext _forumContext;
        private readonly UserManager<IdentityUser> _userManager;
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
            return View("CreateEdit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(messListVM vm)
        {
            int AuteurId = 0;
            if (vm.Reponse != null)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.Sid);
                if (int.TryParse(userIdString, out AuteurId))
                {

                    vm.Reponse.AuteurId = AuteurId;
                }

                //À revoir !
               // vm.Messages = _forumContext.Message.Where(x => x.SujetId == vm.IdSujet).ToList();

                _forumContext.Add(vm.Reponse);
                _forumContext.SaveChanges();

                return View("List");
            }

            return View("List", vm);
        }
    }
}
