using Microsoft.AspNetCore.Mvc;

namespace ForumDiscussion.Controllers
{
    public class MessController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
