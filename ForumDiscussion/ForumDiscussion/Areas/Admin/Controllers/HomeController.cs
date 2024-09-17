using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ForumDiscussion.Models;

namespace ForumDiscussion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Membre.ROLE_ADMIN)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
