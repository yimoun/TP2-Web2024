using Microsoft.AspNetCore.Mvc;

namespace ForumDiscussion.Controllers
{
    public class SujetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
