﻿using Microsoft.AspNetCore.Mvc;

namespace ForumDiscussion.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
